using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {

	public Player player;
	private Player[] players;
	private int numOfPlayers;
	private int activePlayerIndex;
	private float moveTime;
	private float currentMoveTime;

	// Use this for initialization
	void Start () {
		setPlayers (2);
		moveTime = 10.0f;
        //
        GameObject cardsample = Instantiate<GameObject>( (GameObject)Resources.Load("CardDisplaySample") );
        cardsample.transform.SetParent(GameObject.Find("Main UI").transform, false);

        cardsample.GetComponent<Renderer>().enabled = true;
        //cardsample.transform.SetParent(GameObject.Find("Canvas").transform, false);
        //
		activePlayerIndex = -1;
		nextPlayerTurn ();
	}
	
	// Update is called once per frame
	void Update () {
		currentMoveTime -= Time.deltaTime;
		if (currentMoveTime <= 0) { //end of turn
			nextPlayerTurn();
		}
		//to get seconds in int value use:
		//Mathf.Ceil(currentMoveTime);
		//Debug.Log("Playing " + players[activePlayerIndex].getName() + " time " + Mathf.Ceil(currentMoveTime));
	}

	public void setPlayers(int playersNum) { //will probably change to array of Player later on
		numOfPlayers = playersNum;
		players = new Player[numOfPlayers];
		int i = 0;
		for (; i < numOfPlayers; i++) {
			players [i] = Instantiate (player);
			players [i].setName ("Player" + (i + 1));
		}
	}

	public void nextPlayerTurn() {
		if (activePlayerIndex == -1) {
			activePlayerIndex = 0;
			players [activePlayerIndex].switchPlayingState ();
		}
		else {
			players [activePlayerIndex].switchPlayingState ();
			activePlayerIndex = (activePlayerIndex + 1) % numOfPlayers;
			players [activePlayerIndex].switchPlayingState ();
		}
		currentMoveTime = moveTime; //reset time
		//add Player functions to start turn properly here, using players[activePlayerIndex]
	}
}
