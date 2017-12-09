using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHandler : MonoBehaviour {

	public GameObject node;
	public GameObject[,] boardTable;

	// Use this for initialization
	void Start () {
		boardTable = createBoard (11, 7);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject[,] createBoard(int sizex, int sizey) {
		GameObject[,] myTable = new GameObject[sizey,sizex];
		int i, j;
		float offsetx = 0.0f, offsety = 0.0f;
		for (i = 0; i < sizex; i++) {
			for (j = 0; j < sizey; j++) {
				GameObject cube = Instantiate (node);
				cube.transform.SetParent (GameObject.Find("Board").transform, false);
				cube.transform.localPosition = new Vector3 (-1.2f+offsety, 0.0f, 0+offsetx);
				cube.name = "node " + (j+1) + "," + (i+1);
				myTable [j,i] = cube;
				offsety += 1.2f;
			}
			offsetx += 1.2f;
			offsety = 0;
		}
		return boardTable;
	}
}
