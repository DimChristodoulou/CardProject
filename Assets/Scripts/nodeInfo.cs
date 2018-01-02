using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeInfo : MonoBehaviour {
	//contains info regarding nodes


	//checks if node is free to place unit
	private Renderer mRenderer;
	public bool isFree;
	public Color activeColor = new Color32(0x00, 0x99, 0x00, 0xFF); // RGBA
	public Color startColor = new Color32(0xFF, 0xFF, 0xFF, 0x00); // RGBA

	// Use this for initialization
	void Start () {
		mRenderer = this.GetComponentInParent<Renderer>();
		mRenderer.material.color = startColor;
		isFree = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void switchColor() {
		if (mRenderer.material.color == startColor)
			mRenderer.material.color = activeColor;
		else if (mRenderer.material.color == activeColor) {
			mRenderer.material.color = startColor;
		}
	}
}
