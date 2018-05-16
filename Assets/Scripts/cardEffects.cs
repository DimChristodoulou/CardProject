using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;

public class coroutineClass : MonoBehaviour {
    //Generic async Enumerator that checks the clicked GO and returns it to the main effector function.
    public IEnumerator waitForUserToSelect(GameObject target)
    {
        bool selected = false;
        RaycastHit hit = new RaycastHit();
        while (selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.Log(hit.transform.gameObject.name);
                    target = hit.transform.gameObject;
                }
                selected = true;
            }
        }
        yield return target;
    }
}

public class cardEffects : MonoBehaviour {
    public coroutineClass coroutineInstance = null;
    private cardEventHandler cardEvents;
    private cardEffects effector = null;

    private void Awake()
    {
        if(coroutineInstance == null)
            coroutineInstance = new coroutineClass();
        if (effector == null)
            effector = new cardEffects();
    }

    // Use this for initialization
    void Start () {
        cardEventHandler.onSummon += effector.flamesprite;
        cardEventHandler.onSummon += effector.fireball;
    }

    /*
     * Function used to handle the effect of the flamesprite card (card ID = 7)
     */
    public void flamesprite(string minionName)
    {
        GameState.getActivePlayer().currentMana -= jsonparse.cards[1].card_manacost;
        Player opponent;
        if (GameState.activePlayerIndex == 0)
            opponent = GameState.players[1];
        else
            opponent = GameState.players[0];
        
        DealDamageToPlayer(opponent,5);
    }


    public void fireball(string spellName)
    {
        GameObject target = null;
        Debug.Log("coroutine: " + coroutineInstance);
        StartCoroutine(coroutineInstance.waitForUserToSelect(target));
        //Debug.Log(target);
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


