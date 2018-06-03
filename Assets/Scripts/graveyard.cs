using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class graveyard : MonoBehaviour {

    private GameObject graveyardGO;
    private GameObject clonedCard;
    private Card topGraveyardCard;

    // Use this for initialization
    void Start () {
        graveyardGO = GameObject.Find("graveyard");
        topGraveyardCard = new Card();
    }
	
    public void refreshTopGraveyardCard()
    {
        Debug.Log("x:" + graveyardGO.transform.position.x + "y:" + graveyardGO.transform.position.y + "z:" + graveyardGO.transform.position.z);
//        Debug.Log("graveyard count: " + GameState.getActivePlayer().graveyard.Count.ToString() + " graveyard contains: " + GameState.getActivePlayer().graveyard[0].GetComponent<Card>().id.ToString());
        clonedCard = topGraveyardCard.initializeCard(graveyardGO.transform.localPosition.x, graveyardGO.transform.localPosition.y, graveyardGO.transform.localPosition.z, GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1].GetComponent<Card>().id);
        clonedCard.GetComponent<Card>().pointerEventsEnabled = false;
    }

	// Update is called once per frame
	void Update () {
       
	}

    private void OnMouseDown()
    {

    }
}
