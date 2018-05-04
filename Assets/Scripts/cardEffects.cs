using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class cardEffects : MonoBehaviour {

	// Use this for initialization
	void Start () {
        cardEventHandler.onSummon += cardEffects.flamesprite;
    }

    /*
     * Function used to handle the effect of the flamesprite card (card ID = 7)
     */
    public static void flamesprite(string minionName)
    {
        GameState.getActivePlayer().currentMana -= jsonparse.cards[1].card_manacost;
        Player opponent;
        if (GameState.activePlayerIndex == 0)
            opponent = GameState.players[1];
        else
            opponent = GameState.players[0];
        
        DealDamageToPlayer(opponent,5);

    }

    /*
     * Function used to handle the deal damage on player effect
     */
    public static void DealDamageToPlayer(Player targetPlayer, int amountOfDamage)
    {
        targetPlayer.playerHealth -= amountOfDamage;
        targetPlayer.healthGO.GetComponent<Text>().text = "Health: " + targetPlayer.playerHealth;
    }

    /*
     * Function used to handle the freeze effect
     */
    public static void Freeze(GameObject target)
    {

    }



}
