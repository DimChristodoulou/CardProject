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
    private CardDisplay originalCard;
    private GameObject clonedCard;
	//when a player selects an item, we need to remove selection if we click another
	public GameObject selected;

	public Player (int pPos, string pName)
	{
		selected = null;
		monsterPrefab = ((GameObject)Resources.Load ("Monster"));
		this.playingPos = pPos;
		this.pName = pName;
		handCards = new List<GameObject> ();
		boardMinions = new List<GameObject> ();
		graveyard = new List<GameObject> ();
        originalCard = new CardDisplay();
        if (playingPos == 1)
        {
            for(int i=0; i<4; i++)
            {
                clonedCard = originalCard.initandstart(-157+161*i, -330.9f, 0);
                handCards.Add(clonedCard);
            }
        }
        if (playingPos == 2)
        {
            for (int i = 0; i < 4; i++)
            {
                clonedCard = originalCard.initandstart(-157 + 161 * i, 330, 0);
                handCards.Add(clonedCard);
            }
        }
    }

	public void startTurn() {
		foreach (GameObject monster in boardMinions) {
			monster.GetComponent<monsterInfo> ().onStartTurn ();
		}
		isPlaying = true;
	}

	public void endTurn() {
		updateClickedItem (null);
		isPlaying = false;
		foreach (GameObject monster in boardMinions) {
			monster.GetComponent<monsterInfo> ().onEndTurn ();
		}
		//if (playingPos == 1) {
		//	boardMinions [boardMinions.Count - 1].GetComponent<monsterInfo> ().Die ();
		//}
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
			List<Pair<int,int>> summonPos = new List<Pair<int,int>>{ new Pair<int,int> (GameState.dimensionX / 2, 0) };
			if (GameState.allocateBoardPosition (summonPos)) {
				hero = GameObject.Instantiate (monsterPrefab, GameState.getPositionRelativeToBoard (summonPos), new Quaternion (0, 0, 0, 0));
				hero.GetComponent<monsterInfo> ().setPosition (summonPos);
				hero.GetComponent<monsterInfo> ().setData ("test1", 1, 1, 1, 2, 1, this, GameState.turn);
				boardMinions.Add (hero);
			}
		} else {
			//summon for p2
			List<Pair<int,int>> summonPos = new List<Pair<int,int>>{ new Pair<int,int> (GameState.dimensionX / 2, GameState.dimensionY - 1) };
			if (GameState.allocateBoardPosition (summonPos)) {
				hero = GameObject.Instantiate (monsterPrefab, GameState.getPositionRelativeToBoard (summonPos), new Quaternion (0, 0, 0, 0));
				hero.GetComponent<monsterInfo> ().setPosition (summonPos);
				hero.GetComponent<monsterInfo> ().setData ("test1", 1, 1, 1, 2, 1, this, GameState.turn);
				boardMinions.Add (hero);
			}
		}
	}

	public void indicateSummonablePositions() {

	}

	public GameObject summonMonster(string mName, int att, int def, int mcost, int mspeed, int attkrange, List<Pair<int,int>> summonPos, int summonedTurn) {
		GameObject myObj = null;
		if (GameState.allocateBoardPosition(summonPos)) {
			myObj = GameObject.Instantiate(monsterPrefab, GameState.getPositionRelativeToBoard(summonPos), new Quaternion(0,0,0,0));
			myObj.GetComponent<monsterInfo>().setPosition(summonPos);
			myObj.GetComponent<monsterInfo>().setData(mName, att, def, mcost, mspeed, attkrange, this, summonedTurn);
			boardMinions.Add (myObj);
			//also remove the monster from hand or something
		}
		return myObj;
	}

	public void DieMonster(GameObject monster) {
		GameState.setSquares (monster.GetComponent<monsterInfo> ().coords, true); //deallocate tiles
		if (boardMinions.Find (x => monster)!=null) {
			boardMinions.Remove (monster);
		}
		//we add the card to graveyard, not the monster
		GameObject.Destroy(monster); //we can safely delete this since we have created a new card instance for the graveyard
	}

	public void BanishMonster(GameObject monster) { //this is for banishing monsters immidiately from play, not for graveyard cards
		if (boardMinions.Find (x => monster)!=null) {
			boardMinions.Remove (monster);
		}
		//note that we do not send the card to graveyard here
		GameObject.Destroy (monster);
	}

}
