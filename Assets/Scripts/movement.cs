using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    void OnMouseDown()
    {
        
    }

	// Update is called once per frame
	void Update () {
		
	}

	public void moveTo(int xpos, int ypos) {
		if (!GetComponentInParent<monsterInfo> ().movable)
			return;
		int monsterSize = (int)Mathf.Sqrt ((float)gameObject.GetComponent<monsterInfo> ().coords.Count);
		List<Pair<int,int>> newPos = new List<Pair<int,int>> ();
		int i,j;
		for (i = 0; i < monsterSize; i++) {
			for (j = 0; j < monsterSize; j++) {
				newPos.Add (new Pair<int,int> (xpos + i, ypos+j));
			}
		}
		if (!Utilities.BFS (gameObject.GetComponent<monsterInfo> ().coords, newPos, gameObject.GetComponent<monsterInfo> ().movspeed)) {
			//untoggle the element from player that clicked it, we already have the reference from the item he selected as selectedMonster
			return;
		}
		gameObject.GetComponent<monsterInfo>().parentPlayer.updateClickedItem(null);
		GameState.setSquares(GetComponentInParent<monsterInfo>().coords, true);
		Vector3 boardPos = GameState.getPositionRelativeToBoard(newPos);
		gameObject.transform.position = boardPos;
		GetComponentInParent<monsterInfo>().setPosition(newPos);
		GameState.setSquares(GetComponentInParent<monsterInfo>().coords, false);
		GetComponentInParent<monsterInfo> ().onPostMove ();
	}

	public void highlightMovableSquares() {
		if (!GetComponentInParent<monsterInfo>().movable)
			return;
		Dictionary<Pair<int,int>, int> availablesquares = availableMonsterMovements (gameObject);
		foreach (Pair<int,int> coordpair in availablesquares.Keys) {
            {
				GameState.boardTable[coordpair.First, coordpair.Second].GetComponent<nodeInfo>().makeActive();   
            }
        }
	}

	public Dictionary<Pair<int,int>, int> availableMonsterMovements(GameObject monster) {
		//monster param to get the movspeed and/or additional movement effects (flying etc)
		Dictionary<Pair<int,int>, int> availableMoves = new Dictionary<Pair<int,int>, int> ();
		List<Pair<int,int>> startPos = monster.GetComponent<monsterInfo> ().coords;
		List<Pair<int,int>> goalPos = new List<Pair<int,int>>();
		int maxmoves = monster.GetComponent<monsterInfo>().movspeed;
		int i, j, temp=1;
		for (i = -maxmoves; i <= maxmoves; i++) {
			for (j = -maxmoves+Mathf.Abs(i); j <= maxmoves-Mathf.Abs(i); j++) {
				goalPos.Clear ();
				foreach (Pair<int,int> startpair in startPos) {
					if (startpair.First + i >= 0 && startpair.First + i < GameState.dimensionX && startpair.Second + j >= 0 && startpair.Second + j < GameState.dimensionY && 
						(GameState.boardTable[startpair.First+i,startpair.Second+j].GetComponent<nodeInfo>().isFree || Utilities.ContainsPair(startPos, new Pair<int,int>(startpair.First+i,startpair.Second+j)))) {
						//Debug.Log ((startpair.First + i) + "," + (startpair.Second + j));
						goalPos.Add (new Pair<int,int> (startpair.First + i, startpair.Second + j));
					}
					else {
						goalPos.Clear ();
						break;
					}
				}
				if (goalPos.Count > 0) {
					int pr = startPos.Count;
					if (Utilities.BFS (startPos, goalPos, maxmoves)) {
						foreach (Pair<int,int> pair in goalPos) {
							if (!availableMoves.ContainsKey (pair)) {
								availableMoves.Add (pair, 1);
							}
						}
					}
				}
				temp++;
			}
		}
		return availableMoves;
	}
}
