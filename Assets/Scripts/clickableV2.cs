using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clickableV2 : MonoBehaviour {

	//this class was created for 2 reasons:
	//1) it only highlights monster, since we may not be able to move, no need to show nodes in that example case
	//2) it does NOT trigger on each update (only on frames when onMouseOver is active), reducing workload on each frame

	public bool toggle;
	private monsterInfo myInfo;
	private Renderer mRenderer;
	public Color hoverColorActive = new Color32(0x00, 0x99, 0x00, 0xFF); // RGBA
	public Color hoverColorInactive = new Color32(0xCC, 0x00, 0x00, 0xFF); // RGBA
	public Color startColor = new Color32(0xFF, 0xFF, 0xFF, 0x00); // RGBA

	// Use this for initialization
	void Start () {
		toggle = false;
		mRenderer = this.GetComponentInParent<Renderer>();
		myInfo = this.GetComponentInParent<monsterInfo>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseOver() {
		if (myInfo.clickable)
			mRenderer.material.color = hoverColorActive;
		else
			mRenderer.material.color = hoverColorInactive;
	}

	void OnMouseExit() {
		if (toggle)
			mRenderer.material.color = hoverColorActive;
		else
			mRenderer.material.color = startColor;
	}

	void OnMouseDown() {
		if (myInfo.clickable && GameState.getActivePlayer() == this.GetComponentInParent<monsterInfo>().parentPlayer) { //we can only click our monsters
			toggleChange ();
		} else {
			//todo show a "cant do that" message
		}
	}

	public void toggleChange() {
		toggle = !toggle;
		if (toggle) {
			myInfo.parentPlayer.updateClickedItem(gameObject);
			mRenderer.material.color = hoverColorActive;
		}
		else {
			myInfo.parentPlayer.clearClickedItem ();
			mRenderer.material.color = startColor;
		}
		colorMovableNodes (GameState.availableMonsterMovements (gameObject));
	}

	private void colorMovableNodes(Dictionary<Pair<int,int>, int> availablesquares) {
		foreach (Pair<int,int> coordpair in availablesquares.Keys) {
			GameState.boardTable [coordpair.First, coordpair.Second].GetComponent<nodeInfo> ().switchActiveColor ();
		}
	}
}
