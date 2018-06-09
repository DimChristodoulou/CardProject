using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player
{
    //class for player. note that it is not MonoBehaviour extension because player is an abstraction, not a physical game object.

    private GameObject mainui = GameObject.Find("Main UI");
    public GameObject healthGO;

    public int currentMana = 0;
    public int maxTurnMana = 0;
    public int playerHealth = 50;
    private GameObject monsterPrefab;
    public int playingPos; //which player am i, 1st or 2nd
    public string pName;

    public GameObject hero; //hero is a seperate unit due to its win condition checks and the fact that he is instantiated from beginning

    public List<GameObject> handCards;
    public List<GameObject> boardMinions;
    public List<GameObject> graveyard;

    public static List<int>
        deck = new List<int>
        {
        }; //Changed from List<GameObject> to static List<int> because the deck will consist of card ids.

    private int deckSize;
    public bool isPlaying = false;
    private Card originalCard;

    private GameObject clonedCard;

    //when a player selects an item, we need to remove selection if we click another
    public GameObject selected;
    public bool cardSelected = false;
    public GameObject selectedCard = null;
    public int selectedCardIndex;
    public List<Pair<int, int>> availableNodesForSummon;

    public void Start()
    {
        cardSelected = false;
    }

    public Player(int pPos, string pName)
    {
        selected = null;
        monsterPrefab = ((GameObject) Resources.Load("Monster"));
        this.playingPos = pPos;
        this.pName = pName;
        this.deckSize = 30;
        handCards = new List<GameObject>();
        boardMinions = new List<GameObject>();
        graveyard = new List<GameObject>();
        originalCard = new Card();

        healthGO = new GameObject();
        healthGO.transform.SetParent(mainui.transform);
        healthGO.AddComponent<Text>();
        healthGO.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        healthGO.GetComponent<Text>().text = "Health: " + playerHealth.ToString();

        if (playingPos == 1)
        {
            healthGO.transform.localPosition = new Vector3(-909, 250, 0);
            for (int i = 0; i < 4; i++)
            {
                int cardDrawn = Random.Range(0, deck.Count);
                DrawCard(-157 + 161 * i, -330.9f, 0, deck[cardDrawn]);
                deck.RemoveAt(cardDrawn);
//                deckSize--;
            }
        }

        if (playingPos == 2)
        {
            healthGO.transform.localPosition = new Vector3(909, 250, 0);
            for (int i = 0; i < 4; i++)
            {
                int cardDrawn = Random.Range(0, deck.Count);
                DrawCard(-157 + 161 * i, 330, 0, deck[cardDrawn]);
                deck.RemoveAt(cardDrawn);
//                deckSize--;
            }
        }
    }

    public void DrawCard(float x, float y, float z, int cardId)
    {
        clonedCard = originalCard.initializeCard(x, y, z, cardId);
        clonedCard.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        handCards.Add(clonedCard);
        //GameObject deck = GameObject.Find("Player_Deck");
        Text remainingCards = GameObject.Find("Remaining_Cards").GetComponent<Text>();
        remainingCards.text = "Remaining:\n" + (--deckSize);
    }

    public void setupPlayCard(GameObject clickedObj)
    {
        if (GameState.getActivePlayer().currentMana >= int.Parse(clickedObj.GetComponent<Card>().manaCost.text))
        {
            GameState.getActivePlayer().playCard(GameState.getActivePlayer().handCards.IndexOf(clickedObj));
            //Event system from here on out...
            string s = clickedObj.GetComponent<Card>().cardName.text.ToString();
            if (clickedObj.GetComponent<Card>().type.text == "Spell")
            {
                cardEventHandler.onMinionSummon(s);
            }
        }
    }

/*
 * Doc: Function used to play a card and resolve its effects.
 */
    public void playCard(int cardIndex)
    {
        selectedCard = handCards[cardIndex];
        selectedCardIndex = cardIndex;

        cardSelected = true;

        if (handCards[cardIndex].GetComponent<Card>().type.text == "Minion")
        {
            availableNodesForSummon = new List<Pair<int, int>>();
            foreach (GameObject minion in boardMinions)
            {
                //minion.GetComponent<movement>().highlightMovableSquares();
                //Dictionary<Pair<int, int>, int> availableNodes = minion.GetComponent<movement>().availableMonsterMovements(minion);
                //foreach (KeyValuePair<Pair<int, int>, int> pair )

                foreach (KeyValuePair<Pair<int, int>, int> pair in minion.GetComponent<movement>()
                    .availableMonsterMovements(minion))
                {
                    if (GameState.boardTable[pair.Key.First, pair.Key.Second].GetComponent<nodeInfo>().isFree)
                    {
                        GameState.boardTable[pair.Key.First, pair.Key.Second].GetComponent<nodeInfo>().makeActive();

                        availableNodesForSummon.Add(pair.Key);
                    }
                }
            }
        }
    }

    public void increaseMana(int amount)
    {
        this.maxTurnMana += amount;
        this.currentMana = this.maxTurnMana;
    }

    public void decreaseMana(int amount)
    {
        this.maxTurnMana -= amount;
        this.currentMana = this.maxTurnMana;
    }

    public void increaseCurrentMana(int amount)
    {
        this.currentMana += amount;
    }

    public void decreaseCurrentMana(int amount)
    {
        this.currentMana -= amount;
    }

    public void startTurn()
    {
        Debug.Log("HAND CARDS SIZE ON START TURN: " + handCards.Count);
        //Increase mana at start of turn.
        this.increaseMana(1);
        //Draw card after increasing mana. (Max number of cardTemplates in hand is 8).
        //if (handCards.Count < 8)
        //{
        //    int cardDrawn = Random.Range(0, deck.Count);
        //    DrawCard(-157 + 161 * handCards.Count, 330, 0, deck[Random.Range(0, deck.Count)]);
        //    deck.RemoveAt(cardDrawn);
        //}
        //UI MANA GOES HERE
        foreach (GameObject monster in boardMinions)
        {
            monster.GetComponent<monsterInfo>().onStartTurn();
        }

        isPlaying = true;
    }

    public void endTurn()
    {
        updateClickedItem(null);
        isPlaying = false;
        foreach (GameObject monster in boardMinions)
        {
            Debug.Log(monster.name);
            monster.GetComponent<monsterInfo>().onEndTurn();
        }

        //if (playingPos == 1) {
        //	boardMinions [boardMinions.Count - 1].GetComponent<monsterInfo> ().Die ();
        //}
    }

    public bool heroAlive()
    {
        return true;
    }

    public void updateClickedItem(GameObject newclick)
    {
        Debug.Log("In UCI");
        //if another element was clicked before, update selection
        if (selected != null && selected != newclick)
        {
            selected.GetComponent<clickableV2>().toggleChange();
        }

        selected = newclick;
    }

    public void clearClickedItem()
    {
        selected = null;
    }

    public void InstantiateHero()
    {
        //create and position hero
        //hero = Instantiate(blah blah);
        if (playingPos == 1)
        {
            //summon for p1
            List<Pair<int, int>> summonPos = new List<Pair<int, int>> {new Pair<int, int>(GameState.dimensionX / 2, 0)};
            if (GameState.allocateBoardPosition(summonPos))
            {
                hero = GameObject.Instantiate(monsterPrefab, GameState.getPositionRelativeToBoard(summonPos),
                    new Quaternion(0, 0, 0, 0));
                hero.GetComponent<monsterInfo>().setPosition(summonPos);
                hero.GetComponent<monsterInfo>().setData("hero1", 0, 0, 2, 1, this, GameState.turn, null, null);
                boardMinions.Add(hero);
            }
        }
        else
        {
            //summon for p2
            List<Pair<int, int>> summonPos = new List<Pair<int, int>>
            {
                new Pair<int, int>(GameState.dimensionX / 2, GameState.dimensionY - 1)
            };
            if (GameState.allocateBoardPosition(summonPos))
            {
                hero = GameObject.Instantiate(monsterPrefab, GameState.getPositionRelativeToBoard(summonPos),
                    new Quaternion(0, 0, 0, 0));
                hero.GetComponent<monsterInfo>().setPosition(summonPos);
                hero.GetComponent<monsterInfo>().setData("hero2", 0, 0, 2, 1, this, GameState.turn, null, null);
                boardMinions.Add(hero);
            }
        }
    }

    public void indicateSummonablePositions()
    {
    }

    public void summonMonster(string mName, List<Pair<int, int>> summonPos, int summonedTurn)
    {
        GameObject myObj = null;
        if (GameState.allocateBoardPosition(summonPos))
        {
            myObj = GameObject.Instantiate(monsterPrefab, GameState.getPositionRelativeToBoard(summonPos),
                new Quaternion(0, 0, 0, 0));

            myObj.name = mName;
            myObj.GetComponent<monsterInfo>().setPosition(summonPos);

            boardMinions.Add(myObj);

            //also remove the monster from hand or something
        }

        return;
    }

    public void DieMonster(GameObject monster)
    {
        GameState.setSquares(monster.GetComponent<monsterInfo>().coords, true); //deallocate tiles
        if (boardMinions.Find(x => monster) != null)
        {
            boardMinions.Remove(monster);
        }

        //we add the card to graveyard, not the monster
        GameObject.Destroy(
            monster); //we can safely delete this since we have created a new card instance for the graveyard
    }

    public void BanishMonster(GameObject monster)
    {
        //this is for banishing monsters immidiately from play, not for graveyard cardTemplates
        if (boardMinions.Find(x => monster) != null)
        {
            boardMinions.Remove(monster);
        }

        //note that we do not send the card to graveyard here
        GameObject.Destroy(monster);
    }
}