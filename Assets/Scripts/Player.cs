using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private string name;
	private GameObject hero; //hero is a seperate unit due to its win condition checks and the fact that he is instantiated from beginning
	private List<GameObject> handCards;
	private List<GameObject> boardMinions;
	private List<GameObject> graveyard;
	private List<GameObject> deck;
	private bool isPlaying;

	void Start() {
		isPlaying = false;
	}

	void Update() {

	}

	public bool getPlayingState() {
		return this.isPlaying;
	}

	public void switchPlayingState() {
		this.isPlaying = !this.isPlaying;
	}

	public string getName() {
		return this.name;
	}

	public void setName(string mName) {
		name = mName;
	}

}
