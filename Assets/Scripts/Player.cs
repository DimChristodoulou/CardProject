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
		hero = (GameObject)Resources.Load ("Monster");
		if (playingPos == 1) {
			//summon for p1
			hero = GameState.summonMonster ((GameObject)Resources.Load ("Monster"), new List<Pair<int,int>>{ new Pair<int,int> (GameState.dimensionX / 2, 0) });
			//test monster for debug purposes
			//GameObject testobj = GameState.summonMonster ((GameObject)Resources.Load ("Monster"), new List<Pair<int,int>>{ new Pair<int,int> (GameState.dimensionX / 2, 1) });
			//testobj.GetComponent<monsterInfo> ().setData ("test1", 1, 1, 1, 2, this);
			if (hero != null) {
				hero.GetComponent<monsterInfo> ().setData ("test1", 1, 1, 1, 2, this);
			}
		} else {
			//summon for p2
			hero = GameState.summonMonster (hero, new List<Pair<int,int>>{ new Pair<int,int> (GameState.dimensionX / 2, GameState.dimensionY - 1) });
			if (hero != null) {
				hero.GetComponent<monsterInfo> ().setData ("test2", 1, 1, 1, 2, this);
			}
		}
	}

}
