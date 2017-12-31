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
		if (myInfo.clickable) {
			toggleChange ();
		} else {
			//todo show a "cant do that" message
		}
	}

	public void toggleChange() {
		toggle = !toggle;
		if (toggle) {
			//if now this item is active, remove previous active item
			myInfo.parentPlayer.untogglePreviousClicked();
			myInfo.parentPlayer.setPlayerItemClicked (this.gameObject);
			mRenderer.material.color = hoverColorActive;
		}
		else {
			mRenderer.material.color = startColor;
		}
	}
}
