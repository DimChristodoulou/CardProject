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
		GameState.applyHoverWhenMonsterSelected (xpos, ypos);
	}

	void OnMouseExit() {
		GameState.applyHoverWhenMonsterSelected (xpos, ypos);
	}

	void OnMouseDown() {
		GameState.applyMovementWhenMonsterSelected (xpos, ypos);
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
}
