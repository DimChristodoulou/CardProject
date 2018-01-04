using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private string pName;
	private GameObject hero; //hero is a seperate unit due to its win condition checks and the fact that he is instantiated from beginning
	private List<GameObject> handCards;
	private List<GameObject> boardMinions;
	private List<GameObject> graveyard;
	private List<GameObject> deck;
	private bool isPlaying;

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

	void Update() {

	}

	public bool getPlayingState() {
		return this.isPlaying;
	}

	public void switchPlayingState() {
		this.isPlaying = !this.isPlaying;
	}

	public string getName() {
		return this.pName;
	}

	public void setName(string mName) {
		name = mName;
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