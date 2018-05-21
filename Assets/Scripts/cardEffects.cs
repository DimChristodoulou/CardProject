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

    public static cardEffects instance = null;
//    GraphicRaycaster m_Raycaster;
//    PointerEventData m_PointerEventData;
//    EventSystem m_EventSystem;

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

//        m_Raycaster = GameObject.FindGameObjectWithTag("Main UI").GetComponent<GraphicRaycaster>();
//        m_EventSystem = GetComponent<EventSystem>();
    }

    public void setUpDelegate(string minionName)
    {
        Debug.Log("name of card: " + minionName);
        switch (minionName)
        {
            case "Flamesprite":
            {
                Debug.Log("Flamesprite function added to delegate");
                cardEventHandler.onSummon += flamesprite;
                break;
            }
            case "Fireball":
            {
                cardEventHandler.onSummon += fireball;
                break;
            }
        }
    }

    /*
     * Function used to handle the effect of the flamesprite card (card ID = 7)
     */
    public void flamesprite(string minionName)
    {
//        cardEventHandler.onSummon -= fireball;
        GameState.getActivePlayer().currentMana -= jsonparse.cards[1].card_manacost;
        Player opponent;
        if (GameState.activePlayerIndex == 0)
            opponent = GameState.players[1];
        else
            opponent = GameState.players[0];

        DealDamageToPlayer(opponent, 5);
//        cardEventHandler.onSummon += fireball;
    }

    private IEnumerator waitForUserToSelect()
    {
        GameObject target = null;
        Debug.Log("IN IENUMERATOR");
        bool selected = false;
        while (!selected)
        {
//            Debug.Log("NOT SELECTED");

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("MOUSE BUTTON PRESSED");

//                m_PointerEventData = new PointerEventData(m_EventSystem) {position = Input.mousePosition};
//                List<RaycastResult> results = new List<RaycastResult>();
//
//                m_Raycaster.Raycast(m_PointerEventData, results);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
//                foreach (RaycastResult result in results)
//                {
                    Debug.Log("result tag: " + hit.collider.gameObject.tag);
                    Debug.Log("result layer: " + hit.collider.gameObject.layer);
                    if (hit.collider.gameObject.CompareTag("Model"))
                    {
                        Debug.Log("TARGET CHANGED");
                        target = hit.collider.gameObject;
                        selected = true;
                        Debug.Log(target);
                        Destroy(target);
                        cardEventHandler.onSummon -= fireball;
                    }
                    else if (hit.collider.gameObject.layer == 9)
                    {
                        selected = true;
                        cardEventHandler.onSummon -= fireball;
//                        StopCoroutine(waitForUserToSelect());
                    }

//                }
                }
            }

            yield return null;
        }

        yield return target;
    }

    public void fireball(string spellName)
    {
        Debug.Log("IN FIREBALL");
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