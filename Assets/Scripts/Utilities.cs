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

}
