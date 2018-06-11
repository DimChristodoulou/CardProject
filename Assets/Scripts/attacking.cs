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

    public void executeAttack(GameObject enemy)
    {
		//dont attack if the attacking monster can't attack
		if (!GetComponentInParent<monsterInfo> ().attackable || !Utilities.distanceWithinRange(gameObject, enemy))
			return;
		int dmgDiff = GetComponentInParent<monsterInfo> ().power - enemy.GetComponent<monsterInfo> ().power;
		if (dmgDiff>0) {
			Debug.Log ("enemy dies");
			enemy.GetComponent<monsterInfo> ().Die (dmgDiff);
		} else if (dmgDiff==0) {
			enemy.GetComponent<monsterInfo> ().Die ();
			GetComponentInParent<monsterInfo> ().Die ();
			Debug.Log ("both die");
		} else {
			Debug.Log ("attacker dies");
			GetComponentInParent<monsterInfo> ().Die (-dmgDiff);
		}
    }
}
