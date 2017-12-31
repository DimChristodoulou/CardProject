using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class monsterInfo : MonoBehaviour {
	//utility class for general info regarding monster

	//card stats saved in the class
	public string monsterName;
	public int attack, defense, manacost, movspeed;
	//stores pairs of coordinates that this monster sits on
	public List< Pair<int,int> > coords;

	//a monster is controlled by a player
	public Player parentPlayer;

	//a monster can be clicked, move, attack
	public bool clickable, movable, attackable;

	//todo need to find a way to store enchants without changing originals, making silence applicable

	public void setData(string mName, int att, int def, int mcost, int mspeed, Player parent) {
		monsterName = mName; attack = att; defense = def; manacost = mcost; movspeed = mspeed;
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
		
	}

}
