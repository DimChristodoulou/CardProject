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
		Vector3 boardPos = GameState.getPositionRelativeToBoard(newPos);
		gameObject.transform.position.Set(boardPos.x, boardPos.y, boardPos.z);
		GetComponentInParent<monsterInfo>().setPosition(newPos);
	}
}
