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
		if (myInfo.clickable && GameState.getActivePlayer () == this.GetComponentInParent<monsterInfo> ().parentPlayer)
			mRenderer.material.color = GetComponentInParent<monsterInfo> ().hoverColorActive;
		else if (GameState.getActivePlayer () == this.GetComponentInParent<monsterInfo> ().parentPlayer)
			mRenderer.material.color = GetComponentInParent<monsterInfo> ().hoverColorInactive;
		else if (!(GameState.getActivePlayer () == this.GetComponentInParent<monsterInfo> ().parentPlayer)) {//enemy player is playing
			mRenderer.material.color = GetComponentInParent<monsterInfo>().hoverColorAttacked;
			GameState.applyAttackIndicatorWhenMonsterSelected (this.gameObject);
		}
	}

	void OnMouseExit() {
		if (toggle)
			mRenderer.material.color = GetComponentInParent<monsterInfo>().hoverColorActive;
		else
			mRenderer.material.color = GetComponentInParent<monsterInfo>().startColor;
	}

	void OnMouseDown() {
		if (myInfo.clickable && GameState.getActivePlayer() == this.GetComponentInParent<monsterInfo>().parentPlayer) { //we can only click our monsters
			toggleChange ();
		} else if (GameState.getActivePlayer() == this.GetComponentInParent<monsterInfo>().parentPlayer) {
			//todo show a "cant do that" message
		}
		else if (!(GameState.getActivePlayer() == this.GetComponentInParent<monsterInfo>().parentPlayer)) { //enemy player is playing
			GameState.applyAttackWhenMonsterSelected(this.gameObject);
		}
	}

	public void toggleChange() {
		toggle = !toggle;
		if (toggle) {
			myInfo.parentPlayer.updateClickedItem(gameObject);
			mRenderer.material.color = GetComponentInParent<monsterInfo>().hoverColorActive;
		}
		else {
			myInfo.parentPlayer.clearClickedItem ();
			mRenderer.material.color = GetComponentInParent<monsterInfo>().startColor;
		}
		colorMovableNodes (GameState.availableMonsterMovements (gameObject));
	}

	private void colorMovableNodes(Dictionary<Pair<int,int>, int> availablesquares) {
		foreach (Pair<int,int> coordpair in availablesquares.Keys) {
			GameState.boardTable [coordpair.First, coordpair.Second].GetComponent<nodeInfo> ().switchActiveColor ();
		}
	}
}
