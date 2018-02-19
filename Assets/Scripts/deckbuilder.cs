using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deckbuilder : MonoBehaviour {

    private CardDisplay originalCard;
    private GameObject clonedCard;
    private jsonparse cardsJson;

    // Use this for initialization
    void Start () {

        cardsJson = new jsonparse();
        originalCard = new CardDisplay();
        originalCard.initializeCard(-330, 86, 0, 0);
        originalCard.initializeCard(-160.5f, 86, 0, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
