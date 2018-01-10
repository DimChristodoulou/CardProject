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

	void OnSceneGUI() {
		Vector3 startV = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
		Vector3 endV = new Vector3 (0,0,0);
		Vector3 startT = (endV + startV) / 2;
		Vector3 endT = (endV + startV) / 2;
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
	}
}
