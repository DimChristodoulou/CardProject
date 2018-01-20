using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeInfo : MonoBehaviour {
	//contains info regarding nodes

	//checks if node is free to place unit
	private Renderer mRenderer;
	public int xpos, ypos;
	public bool isFree;
	public Color activeColor = new Color32(0x00, 0x99, 0x00, 0xFF); // RGBA
	public Color startColor = new Color32(0xFF, 0xFF, 0xFF, 0x00); // RGBA
	public Color hoverColor = new Color32(0x2C, 0x2C, 0xFF, 0x8F); //RGBA
	public Color currentColor; //current color, ignoring temporary effects like hovers

	// Use this for initialization
	void Start () {
		mRenderer = this.GetComponentInParent<Renderer>();
		currentColor = startColor;
		mRenderer.material.color = currentColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter() {
		colorHovered ();
	}

	void OnMouseExit() {
		colorHovered ();
	}

	void OnMouseDown() {
		GameObject selectedMonster = GameState.players [GameState.activePlayerIndex].selected;
		if (selectedMonster == null) {
			return; //no selected monster for movement
		}
		selectedMonster.GetComponent<movement> ().moveTo (xpos, ypos);
	}

	public void switchHoverColor() {
		if (mRenderer.material.color == hoverColor) {
			mRenderer.material.color = currentColor;
		} else {
			mRenderer.material.color = hoverColor;
		}
	}

	public void switchActiveColor() {
		if (currentColor != activeColor) {
			currentColor = activeColor;
		}
		else {
			currentColor = startColor;
		}
		mRenderer.material.color = currentColor;
	}

	//colors/uncolors the hovered squares for the movement, if the player has a monster selected
	private void colorHovered() {
		GameObject selectedMonster = GameState.players [GameState.activePlayerIndex].selected;
		if (selectedMonster == null) {
			return; //no selected monster for movement
		}
		int monsterSize = (int)Mathf.Sqrt ((float)selectedMonster.GetComponent<monsterInfo> ().coords.Count);
		if (xpos + monsterSize >= GameState.dimensionX || ypos + monsterSize >= GameState.dimensionY) {
			return; //out of table placement
		}
		int i;
		GameState.boardTable [xpos, ypos].GetComponent<nodeInfo> ().switchHoverColor ();
		for (i = 1; i < monsterSize; i++) {
			GameState.boardTable [xpos+i, ypos].GetComponent<nodeInfo> ().switchHoverColor ();
			GameState.boardTable [xpos, ypos+i].GetComponent<nodeInfo> ().switchHoverColor ();
			GameState.boardTable [xpos+i, ypos+i].GetComponent<nodeInfo> ().switchHoverColor ();
		}
	}
}
