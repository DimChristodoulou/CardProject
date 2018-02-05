using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attacking : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void indicateAttack(GameObject enemy) {
		if (!GetComponentInParent<monsterInfo> ().attackable)
			return;
		Debug.Log ("attkline");
	}

	public void executeAttack(GameObject enemy) {
		if (!GetComponentInParent<monsterInfo> ().attackable)
			return;
		Debug.Log ("attk");
		enemy.GetComponent<monsterInfo> ().defense -= GetComponentInParent<monsterInfo> ().attack; //dummy attacking
		GetComponentInParent<monsterInfo>().onPostAttack();
		if (enemy.GetComponent<monsterInfo>().defense>0) { //if the monster has died we do not execute the postdefense effects
			enemy.GetComponent<monsterInfo> ().onPostDefense ();
		}
	}
}
