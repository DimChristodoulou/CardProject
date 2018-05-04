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
	void Awake () {
		toggle = false;
		mRenderer = this.GetComponentInParent<Renderer>();
        myInfo = gameObject.GetComponent<monsterInfo>(); //this.GetComponentInParent<monsterInfo>();
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
			if (GameState.players [GameState.activePlayerIndex].selected != null) {
				GameState.players [GameState.activePlayerIndex].selected.GetComponent<attacking> ().indicateAttack (gameObject);
			}
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
		else if (!(GameState.getActivePlayer() == this.GetComponentInParent<monsterInfo>().parentPlayer)) { //enemy player is playing, so the click means attack
			if (GameState.players [GameState.activePlayerIndex].selected != null) {
				GameState.players [GameState.activePlayerIndex].selected.GetComponent<attacking> ().executeAttack (gameObject);
			}
		}
	}

	public void toggleChange() {
        if (!myInfo.parentPlayer.cardSelected)
        {
            toggle = !toggle;
            if (toggle)
            {
                //if (myInfo == null)
                //    Debug.Log("gameobject name: "+  gameObject.name);

                myInfo.parentPlayer.updateClickedItem(gameObject);
                mRenderer.material.color = GetComponentInParent<monsterInfo>().hoverColorActive;

            }
            else
            {

                myInfo.parentPlayer.clearClickedItem();
                mRenderer.material.color = GetComponentInParent<monsterInfo>().startColor;

            }
        }
		GetComponentInParent<movement> ().highlightMovableSquares ();
	}
}
