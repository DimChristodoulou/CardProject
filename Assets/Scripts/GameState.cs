using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour {
	//This is the class to handle board, check board conditions (can monster move/summon) where it wants etc
	//Also initiates and controls game flow (who plays, time etc).
	//all vars are static because we will only have one game running anyway

	static public Player[] players;
	static public int numOfPlayers;
	static public int activePlayerIndex;
	static private float moveTime = 30.0f;
	static public float currentMoveTime;
	static public int turn;
	static public GameObject[,] boardTable;
	//general info regarding board
	static public int dimensionX=7, dimensionY=11;
	static private float xspacing = 1.2f, yspacing = 1.2f; //offset for board view

	// Use this for initialization
	void Start () {
		turn = 0;
		boardTable = createBoard (dimensionX, dimensionY);
		setPlayers (2);
		moveTime = 10.0f;
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
		Mathf.Ceil(currentMoveTime);
		Debug.Log("Turn " + turn + " : Playing " + players[activePlayerIndex].pName + " time " + Mathf.Ceil(currentMoveTime));

	}

	public GameObject[,] createBoard(int sizex, int sizey) {
		GameObject[,] myTable = new GameObject[sizex,sizey];
		int i, j;
		float offsetx = 0.0f, offsety = 0.0f;
		GameObject node = ((GameObject)Resources.Load ("node"));
		for (i = 0; i < sizey; i++) {
			for (j = 0; j < sizex; j++) {
				GameObject cube = Instantiate(node);
				cube.transform.SetParent (GameObject.Find("Board").transform, false);
				cube.transform.localPosition = new Vector3 (-yspacing+offsety, 0.0f, 0+offsetx);
				cube.GetComponent<nodeInfo> ().isFree = true;
				cube.GetComponent<nodeInfo> ().xpos = j;
				cube.GetComponent<nodeInfo> ().ypos = i;
				cube.name = "node " + (j+1) + "," + (i+1);
				myTable [j,i] = cube;
				offsety += yspacing;
			}
			offsetx += xspacing;
			offsety = 0;
		}
		return myTable;
	}

	public void setPlayers(int playersNum) { //will probably change to array of Player later on
		numOfPlayers = playersNum;
		players = new Player[numOfPlayers];
		int i = 0;
		for (; i < numOfPlayers; i++) {
			players [i] = new Player (i+1, "Player" + (i + 1));
		}
		for (i=0; i < numOfPlayers; i++) {
			players [i].InstantiateHero ();
		}
	}

	static public void nextPlayerTurn() {
		if (activePlayerIndex!=-1)
			players [activePlayerIndex].endTurn ();
		activePlayerIndex = (activePlayerIndex + 1) % numOfPlayers;
		if (activePlayerIndex == 0) { //full cycle of players
			turn++;
		}
		players [activePlayerIndex].startTurn ();
		currentMoveTime = moveTime; //reset time
		//add Player functions to start turn properly here, using players[activePlayerIndex]
	}

	static public Player getActivePlayer() {
		return players [activePlayerIndex];
	}

	static public bool allocateBoardPosition(List<Pair<int,int>> allocPos) {
		foreach (Pair<int,int> pair in allocPos) {
			if (boardTable [pair.First, pair.Second].GetComponent<nodeInfo> ().isFree == false)
				return false;
		}
		foreach (Pair<int,int> pair in allocPos) {
			boardTable [pair.First, pair.Second].GetComponent<nodeInfo> ().isFree = false; //allocating the board space
		}
		return true;
	}

	static public Vector3 getPositionRelativeToBoard(List< Pair<int,int> > pos) {
		//get average of positions in list, centering the object
		Pair<float,float> avgpos = new Pair<float,float>(0f,0f);
		foreach (Pair<int,int> pair in pos) {
			avgpos.First += pair.First;
			avgpos.Second += pair.Second;
		}
		avgpos.First /= pos.Count;
		avgpos.Second /= pos.Count;
		//move relative to node [0,0], based on avg and offsets
		return boardTable[0,0].transform.position + new Vector3(12*avgpos.First, 5, 12*avgpos.Second);
	}

	static public void setSquares(List<Pair<int,int>> squarecoords, bool value) {
		foreach (Pair<int,int> coord in squarecoords) {
			boardTable [coord.First, coord.Second].GetComponent<nodeInfo> ().isFree = value;
		}
	}
}
