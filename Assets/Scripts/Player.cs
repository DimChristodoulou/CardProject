using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
	//class for player. note that it is not MonoBehaviour extension because player is an abstraction, not a physical game object.


	private GameObject monsterPrefab;
	public int playingPos; //which player am i, 1st or 2nd
	public string pName;
	public GameObject hero; //hero is a seperate unit due to its win condition checks and the fact that he is instantiated from beginning
	public List<GameObject> handCards;
	public List<GameObject> boardMinions;
	public List<GameObject> graveyard;
	public List<GameObject> deck;
	public bool isPlaying = false;
	//when a player selects an item, we need to remove selection if we click another
	public GameObject selected;

	public Player (int pPos, string pName)
	{
		selected = null;
		monsterPrefab = ((GameObject)Resources.Load ("Monster"));
		this.playingPos = pPos;
		this.pName = pName;
	}

	public void switchPlayState() {
		updateClickedItem (null);
		isPlaying = !isPlaying;
	}

	public bool heroAlive() {
		return true;
	}

	public void updateClickedItem(GameObject newclick) {
		//if another element was clicked before, update selection
		if (selected != null && selected != newclick)
			selected.GetComponent<clickableV2>().toggleChange();
		selected = newclick;
	}

	public void clearClickedItem() {
		selected = null;
	}

	public void InstantiateHero() {
		//create and position hero
		//hero = Instantiate(blah blah);
		if (playingPos == 1) {
			//summon for p1
			hero = summonMonster("test1", 1, 1, 1, 2, new List<Pair<int,int>>{ new Pair<int,int> (GameState.dimensionX / 2, 0) });
			//test monster for debug purposes
			GameObject testobj = summonMonster ("test1", 1, 1, 1, 2, new List<Pair<int,int>>{ new Pair<int,int> (GameState.dimensionX / 2, 1) });
		} else {
			//summon for p2
			hero = summonMonster ("test2", 1, 1, 1, 2, new List<Pair<int,int>>{ new Pair<int,int> (GameState.dimensionX / 2, GameState.dimensionY - 1) });
		}
	}

	public GameObject summonMonster(string mName, int att, int def, int mcost, int mspeed, List<Pair<int,int>> summonPos) {
		GameObject myObj = null;
		if (GameState.allocateBoardPosition(summonPos)) {
			myObj = GameObject.Instantiate(monsterPrefab, GameState.getPositionRelativeToBoard(summonPos), new Quaternion(0,0,0,0));
			myObj.GetComponent<monsterInfo>().setPosition(summonPos);
			myObj.GetComponent<monsterInfo>().setData(mName, att, def, mcost, mspeed, this);
		}
		return myObj;
	}

}
