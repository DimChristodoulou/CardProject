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

	public void moveTo(List<Pair<int,int>> newPos) {
		//this function is created to add here animations or anything else related to moves
		GameState.setSquares(GetComponentInParent<monsterInfo>().coords, true);
		Vector3 boardPos = GameState.getPositionRelativeToBoard(newPos);
		gameObject.transform.position = boardPos;
		GetComponentInParent<monsterInfo>().setPosition(newPos);
		GameState.setSquares(GetComponentInParent<monsterInfo>().coords, false);
	}
}
