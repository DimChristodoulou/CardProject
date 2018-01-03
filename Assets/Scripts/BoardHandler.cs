using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHandler : MonoBehaviour {

	//public GameObject node;
	public static Node[,] boardTable;

	// Use this for initialization
	void Start () {
        //print("null\n");
        boardTable = createBoard (11, 7);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Node[,] createBoard(int sizex, int sizey) {
		Node[,] myTable = new Node[sizey,sizex];
		int i, j;
		float offsetx = 0.0f, offsety = 0.0f;

        GameObject node = new GameObject();
        for (i = 0; i < sizex; i++) {
			for (j = 0; j < sizey; j++) {
                myTable[j, i] = new Node();
                GameObject cube = Instantiate (myTable[j,i].getNodeObj());
                
				cube.transform.SetParent (GameObject.Find("Board").transform, false);
				cube.transform.localPosition = new Vector3 (-1.2f+offsety, 0.0f, 0+offsetx);
				cube.name = "node " + (j+1) + "," + (i+1);
				myTable[j,i].setNodeObj(cube);
				offsety += 1.2f;
			}
			offsetx += 1.2f;
			offsety = 0;
		}
		return myTable;
	}


}
