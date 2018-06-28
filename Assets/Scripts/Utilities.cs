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

	static public bool BFS(List<Pair<int,int>> curPos, List<Pair<int,int>> goalPos, int maxmoves) {
		List<Pair<int,int>> node = new List<Pair<int,int>> ();
		Queue q = new Queue ();
		q.Enqueue (new Pair <List<Pair<int,int>>, int> (curPos, 0)); //queue stores the list of position nodes and the moves that were needed for that
		Dictionary<List<Pair<int,int>>, int> explored = new Dictionary<List<Pair<int,int>>, int> ();
		while (q.Count > 0) {
			node = ((Pair <List<Pair<int,int>>, int>)q.Peek ()).First;
			int moves = ((Pair <List<Pair<int,int>>, int>)q.Peek ()).Second;
			q.Dequeue ();
			if (ComparePairLists (goalPos, node)) {
				return true;
			}
			if (moves >= maxmoves) {
				continue;
			}
			for (int i = -1; i <= 1; i++) {
				for (int j = -1 + Mathf.Abs (i); j <= 1 - Mathf.Abs (i); j++) {
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
						explored.Add (child, moves + 1);
						q.Enqueue (new Pair <List<Pair<int,int>>, int> (child, moves+1));
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
		foreach (Pair<int,int> m2pair in monster2.GetComponent<monsterInfo>().coords) {
			foreach (Pair<int,int> m1pair in monster1.GetComponent<monsterInfo>().coords) {
				int distX = Mathf.Abs(m2pair.First - m1pair.First);
				int distY = Mathf.Abs(m2pair.Second - m1pair.Second);
				if (monster1.GetComponent<monsterInfo>().attkrange >= Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(distX, 2) + Mathf.Pow(distY, 2)))) {
					Debug.Log("ATTKINFO " + Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(distX, 2) + Mathf.Pow(distY, 2))) + " " + distX + " " + distY + " " + monster1.GetComponent<monsterInfo>().attkrange);
					return true;
				}
			}
		}
		return false;
	}

}
