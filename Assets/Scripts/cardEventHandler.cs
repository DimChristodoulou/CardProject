using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardEventHandler : MonoBehaviour
{
    public delegate void minionEventHandler(string minionName);

    public static event minionEventHandler onSummon;
    public static event minionEventHandler onDeath;

    /*
     * Function used to handle the on Summon effect
     */
    public static void onMinionSummon(string minionName)
    {
        cardEffects.instance.setUpDelegate(minionName);

        if (GameState.getActivePlayer().selectedCard.GetComponent<Card>().type.text.Equals("Spell") &&
            GameState.getActivePlayer().availableNodesForSummon != null)
        {
            foreach (Pair<int, int> node in GameState.getActivePlayer().availableNodesForSummon)
                GameState.boardTable[node.First, node.Second].GetComponent<nodeInfo>().makeInactive();
            GameState.getActivePlayer().availableNodesForSummon = null;
        }

		//decrease mana based on card mana cost
		GameState.getActivePlayer().currentMana -= int.Parse(GameState.getActivePlayer().selectedCard.GetComponent<Card>().manaCost.text);

        Debug.Log(minionName);
        onSummon(minionName);

        if (GameState.getActivePlayer().selectedCard.GetComponent<Card>().type.text.Equals("Minion"))
        {
            GameState.getActivePlayer().selectedCard.SetActive(false);         
            GameState.getActivePlayer().handCards.RemoveAt(GameState.getActivePlayer().selectedCardIndex);
        }
    }

    /*
     * Function used to handle the on Death effect
     */
    public static void onMinionDeath(string minionName)
    {
        onDeath(minionName);
    }
}