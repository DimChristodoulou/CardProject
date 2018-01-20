using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
	//class for player. note that it is not MonoBehaviour extension because player is an abstraction, not a physical game object.

    private void shuffleDeck()
    {
        int randInt;

        List<GameObject> temp = new List<GameObject>();

        for (int i = 0; i < deck.Count; i++)
        {
            randInt = Random.Range(0, deck.Count);

            deck.Insert(randInt, deck[i]);
        }

        // copy shuffled deck
        deck = new List<GameObject>(temp);
    }

    // probably unnecessary constructor
    /*public Player()
    {
        handCards = new List<GameObject>(10); // 10: maximum hand size
        graveyard = new List<GameObject>();
        deck = new List<GameObject>();
        // here add all cards to deck


        // shuffle
        shuffleDeck();
    }*/

	void Start() {
		isPlaying = false;

        handCards = new List<GameObject>(10); // 10: maximum hand size
        graveyard = new List<GameObject>();
        deck = new List<GameObject>();
        // here add all cards to deck


        // shuffle
        shuffleDeck();
    }
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
		handCards = new List<GameObject> ();
		boardMinions = new List<GameObject> ();
		graveyard = new List<GameObject> ();
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
            summonMonster("test", 1, 1, 1, 3, 1, new List<Pair<int, int>> { new Pair<int, int>(4, 1) }, GameState.turn);
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

    
    // functions for deck, graveyard and hand interaction

    public void drawCard(List<GameObject> cardList, int index = 0) // draw card from deck/graveyard/banish at position "index", default card to draw: card at position 0
    {
        if (cardList.Count > 0 && handCards.Count < 7)
        { 
            handCards.Add(cardList[index]);
            handCards.Sort(/*sort function here*/);
            
            // add UI element

            deck.RemoveAt(index);
        }
    }
    /*
    public void playCard(List<GameObject> cardList, /*instead of GameObject it will be class Card** GameObject card, int boardPosX, int boardPosY)
    {
        int x = boardPosX, y = boardPosY;

        Node[,] board = BoardHandler.boardTable;

        if (cardList.Count > 0 && board[x, y].isEmpty())
        {
            board[x, y].setModel(/*here get model GameObject of card "card"**);

            // remove card from cardList
            cardList.Remove(/*object of class Card**);
        }
    }*/
}