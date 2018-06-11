using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities {

	//This function is used to take a list of pairs(starting position), a list of pairs(final position) and the max moves to be made, and return whether a path exists to reach there
	static public bool DFS(List<Pair<int,int>> curPos, List<Pair<int,int>> goalPos, int maxmoves) {
		List<Pair<int,int>> node = new List<Pair<int,int>> ();
		Stack frontier = new Stack ();
		frontier.Push (new Pair <List<Pair<int,int>>, int> (curPos, 0)); //stack stores the list of position nodes and the moves that were needed for that
		Dictionary<List<Pair<int,int>>, int> explored = new Dictionary<List<Pair<int,int>>, int> ();
		while (frontier.Count > 0) {
			node = ((Pair <List<Pair<int,int>>, int>)frontier.Peek ()).First;
			int moves = ((Pair <List<Pair<int,int>>, int>)frontier.Peek ()).Second;
			frontier.Pop ();
			if (moves > maxmoves) {
				continue;
			}
			if (ComparePairLists (goalPos, node)) {
				return true;
			}
			explored.Add (node, moves);
			int i, j;
			for (i = -1; i <= 1; i++) {
				for (j = -1 + Mathf.Abs (i); j <= 1 - Mathf.Abs (i); j++) {
					List<Pair<int,int>> child = new List<Pair<int,int>> ();
					foreach (Pair<int,int> startpair in node) {
						if (startpair.First+i >= 0 && startpair.First+i < GameState.dimensionX && startpair.Second+j >= 0 && startpair.Second+j < GameState.dimensionY && 
							(GameState.boardTable [startpair.First + i, startpair.Second + j].GetComponent<nodeInfo> ().isFree || Utilities.ContainsPair(curPos, new Pair<int,int>(startpair.First+i,startpair.Second+j)))) {
							child.Add (new Pair<int,int> (startpair.First + i, startpair.Second + j));
						} else {
							child.Clear ();
							break;
						}
					}
					if (child.Count>0 && !explored.ContainsKey(child)) {
						frontier.Push (new Pair <List<Pair<int,int>>, int> (child, moves+1));
					}
				}
			}
		}
		return false;
	}

	//utility O(n^2) function to compare 2 lists of pairs
	//we need better complexity algorithm for that
	static private bool ComparePairLists(List<Pair<int,int>> aListA, List<Pair<int,int>> aListB)
	{
		if (aListA == null || aListB == null || aListA.Count != aListB.Count)
			return false;
		if (aListA.Count == 0)
			return true;
		for (int i = 0; i < aListA.Count; i++) {
			bool flag = false;
			for (int j = 0; j < aListB.Count; j++) {
				if (aListA [i].First == aListB [j].First && aListA [i].Second == aListB [j].Second) {
					flag = true;
					break;
				}
			}
			if (!flag)
				return false;
		}
		return true;
	}

	public static bool ContainsPair(List<Pair<int,int>> mylist, Pair<int,int> myitem) {
		bool result = false;
		foreach (Pair<int,int> item in mylist) {
			if (item.First == myitem.First && item.Second == myitem.Second) {
				result = true;
			}
		}
		return result;
	}

	public static Pair<int,int> findTopLeftCoord(List<Pair<int,int>> coords) {
		if (coords.Count == 0)
			return null;
		Pair<int,int> topLeftItem = coords [0];
		foreach (Pair<int,int> item in coords) {
			if (item.First <= topLeftItem.First && item.Second <= topLeftItem.Second) {
				topLeftItem = item;
			}
		}
		return topLeftItem;
	}

	//does NOT take care of line of sight
	//monster1 is the attacker
	//monster2 is the defender
	public static bool distanceWithinRange(GameObject monster1, GameObject monster2) {
		
		Pair<int,int> topleft1 = findTopLeftCoord (monster1.GetComponent<monsterInfo>().coords);
		Pair<int,int> topleft2 = findTopLeftCoord (monster2.GetComponent<monsterInfo>().coords);
		//suppose only square monsters
		int monster1Size = Mathf.FloorToInt(Mathf.Sqrt (monster1.GetComponent<monsterInfo> ().coords.Count));
		int monster2Size = Mathf.FloorToInt(Mathf.Sqrt (monster2.GetComponent<monsterInfo> ().coords.Count));
		if (monster2Size > monster1Size)
			monster1Size = monster2Size;
		//this decrease happens to account for right side of monster
		int distX = Mathf.Abs (topleft1.First - topleft2.First) - monster1Size;
		int distY = Mathf.Abs (topleft1.Second - topleft2.Second) - monster1Size;
		if (monster1.GetComponent<monsterInfo>().attkrange >= Mathf.Floor(Mathf.Sqrt (distX ^ 2 + distY ^ 2)) ) {
			return true;
		}
		return false;
	}

}
