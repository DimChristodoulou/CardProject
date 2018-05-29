using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class cardEffects : MonoBehaviour
{
    private cardEventHandler cardEvents;
    public static bool disableOtherInput = false;
    public static cardEffects instance = null;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    //    private cardEffects effector = null;

//    private void Awake()
//    {
//        if (effector == null)
//            effector = new cardEffects();
//    }

    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
//        cardEventHandler.onSummon += flamesprite;
//        cardEventHandler.onSummon += fireball;

        m_Raycaster = GameObject.FindGameObjectWithTag("Main UI").GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    public void setUpDelegate(string minionName)
    {
        switch (minionName)
        {
            case "Flamesprite":
            {
                cardEventHandler.onSummon += flamesprite;
                break;
            }
            case "Fireball":
            {
                cardEventHandler.onSummon += fireball;
                break;
            }
            case "The Emperor's Hound":
            {
                cardEventHandler.onSummon += emperorsHound;
                break;
            }
            case "Firewraith":
            {
                cardEventHandler.onSummon += firewraith;
                break;
            }
        }
    }

    public void emperorsHound(string minionName)
    {
        //Do Nothing
    }

    public void firewraith(string minionName)
    {
        //Do Nothing
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

        DealDamageToPlayer(opponent, 5);
        cardEventHandler.onSummon -= flamesprite;
    }

    private IEnumerator waitForUserToSelect()
    {
        GameObject target = null;
        bool selected = false;
        disableOtherInput = true;
        while (!selected) { 

            if (Input.GetMouseButtonDown(0))
            {

                m_PointerEventData = new PointerEventData(m_EventSystem) {position = Input.mousePosition};
                List<RaycastResult> results = new List<RaycastResult>();

                m_Raycaster.Raycast(m_PointerEventData, results);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Model"))
                    {
                        target = hit.collider.gameObject;
                        selected = true;

                        //TODO Change this whenever we need to :)
                        //First, we destroy the GO...
                        Destroy(target);
                        //Then we remove the model from the boardMinions list...
                        GameState.getActivePlayer().boardMinions.Remove(target);
                        //then we get the coordinates of the monster and set its square to free...
                        GameState.boardTable[target.GetComponent<monsterInfo>().coords[0].First, target.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().isFree = true;
                        //then we destroy the card
                        Destroy(GameState.getActivePlayer().selectedCard);
                        GameState.getActivePlayer().handCards.RemoveAt(GameState.getActivePlayer().selectedCardIndex);

                        GameState.getActivePlayer().cardSelected = false;

                        cardEventHandler.onSummon -= fireball;
                    }
                    else if (results.Count > 0)
                    {
                        foreach (RaycastResult result in results)
                        {
                            if (result.gameObject.CompareTag("Card"))
                            {
                                selected = true; // stop coroutine
                                cardEventHandler.onSummon -= fireball;
                                break;
                            }
                        }
                        
//                        StopCoroutine(waitForUserToSelect());
                    }

//                }
                }
            }

            yield return null;
        }
        disableOtherInput = false;
        yield return target;
    }


    public void fireball(string spellName)
    {
        StartCoroutine(waitForUserToSelect());
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