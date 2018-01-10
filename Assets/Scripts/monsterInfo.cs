using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterInfo : MonoBehaviour {
	//utility class for general info regarding monster

	//card stats saved in the class
	public string monsterName;
	public GameObject card; //the card of the monster
	public int attack, defense, manacost, movspeed, attkrange; //these will affect offsets to card values, but the card is needed for that to be implemented (TODO post-merge)
	//stores pairs of coordinates that this monster sits on, indexed [0,size-1]
	public List< Pair<int,int> > coords;
	public Color hoverColorAttacked = new Color32(0xFF, 0x99, 0x00, 0x8F); // RGBA
	public Color hoverColorActive = new Color32(0x00, 0x99, 0x00, 0x8F); // RGBA
	public Color hoverColorInactive = new Color32(0x99, 0x99, 0x00, 0x8F); // RGBA
	public Color startColor = new Color32(0xFF, 0xFF, 0xFF, 0x00); // RGBA

	//a monster is controlled by a player
	public Player parentPlayer;

	//a monster can be clicked, move, attack
	public bool clickable, movable, attackable;

	//todo need to find a way to store enchants without changing originals, making silence applicable

	public void setData(string mName, int att, int def, int mcost, int mspeed, int attkrange, Player parent) {
		monsterName = mName; attack = att; defense = def; manacost = mcost; movspeed = mspeed; attkrange = attkrange;
		parentPlayer = parent;
	}

	public void setPosition(List< Pair<int, int> > myList) {
		coords = myList;
	}

	// Use this for initialization
	void Start () {
		//for testing i enable all monster options
		clickable = true; movable = true; attackable = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (defense <= 0)
			Die ();
	}

	public void Die() {
		parentPlayer.DieMonster (this.gameObject);
	}

	public void Banish() {
		parentPlayer.BanishMonster (this.gameObject);
	}

}
