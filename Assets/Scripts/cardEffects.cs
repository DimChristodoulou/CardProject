using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class cardEffects : MonoBehaviour
{
    private cardEventHandler cardEvents;
    public static bool disableOtherInput = false;
    public static cardEffects instance = null;
    private graveyard graveyardGO;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private GameObject target;
    private List<RaycastResult> results;
    private bool selected;

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
        graveyardGO = GameObject.Find("graveyard").GetComponent<graveyard>();
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
            case "Pyra, the Elemental Lord":
            {
                cardEventHandler.onSummon += pyra;
                break;
            }
            case "The Emperor's Fool":
            {
                cardEventHandler.onSummon += emperorsFool;
                break;
            }
            case "The Emperor's Knight":
            {
                cardEventHandler.onSummon += emperorsKnight;
                break;
            }
            case "Iron Resolve":
            {
                //TODO: Casting
                cardEventHandler.onSummon += ironResolve;
                break;
            }
            case "Idol of Fire":
            {
                cardEventHandler.onSummon += idolOfFire;
                break;
            }
        }
    }

    public void idolOfFire(string minionName){
        GameState.getActivePlayer().boardMinions.Last().GetComponent<monsterInfo>().cannotMove = true;
        GameState.getActivePlayer().boardMinions.Last().GetComponent<monsterInfo>().cannotAttack = true;
        Debug.Log(GameState.getActivePlayer().boardMinions.Last().GetComponent<monsterInfo>().cannotMove);
        Debug.Log(GameState.getActivePlayer().boardMinions.Last().GetComponent<monsterInfo>().cannotAttack);
        cardEventHandler.onSummon -= idolOfFire;
    }

    public void ironResolve(string minionName)
    {
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(targetedIronResolve());
    }

    private IEnumerator targetedIronResolve()
    {
        while (!selected && target == null)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (target.CompareTag("Model"))
        {
            buffs ironResolveBuff = new buffs();
            ironResolveBuff.buffAmount = 3;
            ironResolveBuff.buffCardId = 10;
            ironResolveBuff.buffName = "Iron Resolve";

            target.GetComponent<monsterInfo>().refreshBuffs(ironResolveBuff);
            GameState.getActivePlayer().cardSelected = false;

            GameState.getActivePlayer().handCards.Remove(GameState.getActivePlayer().selectedCard);
            GameState.getActivePlayer().graveyard.Add(GameState.getActivePlayer().selectedCard);

            GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];
            topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;
            Debug.Log("TGC" + topGraveyardCard.GetComponent<Card>().cardName.text);
            topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
            topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);
            topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);
            topGraveyardCard.SetActive(true);

            cardEventHandler.onSummon -= ironResolve;

            //Destroy(GameState.getActivePlayer().selectedCard);
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    //selected = true; // stop coroutine
                    cardEventHandler.onSummon -= ironResolve;
                    break;
                }
            }
        }
    }

    public void emperorsKnight(string minionName)
    {
        cardEventHandler.onSummon -= emperorsKnight;
    }

    public void emperorsHound(string minionName)
    {
        cardEventHandler.onSummon -= emperorsHound;
    }

    public void firewraith(string minionName)
    {
        cardEventHandler.onSummon -= firewraith;
    }

    public void emperorsFool(string minionName)
    {
        cardEventHandler.onSummon -= emperorsFool;
    }

    /*
     * Function used to handle the effect of the flamesprite card (card ID = 7)
     */
    public void flamesprite(string minionName)
    {
        GameState.getActivePlayer().currentMana -= jsonparse.cardTemplates[1].card_manacost;
        Player opponent;
        if (GameState.activePlayerIndex == 0)
            opponent = GameState.players[1];
        else
            opponent = GameState.players[0];

        DealDamageToPlayer(opponent, 5);
        cardEventHandler.onSummon -= flamesprite;
    }

    public void fireball(string spellName)
    {
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(waitAndDestroyTarget());
    }

    public void pyra(string minionName)
    {
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(waitAndPyra());
    }

    private IEnumerator waitForUserToSelectTarget()
    {
        selected = false;
        disableOtherInput = true;

        yield return new WaitForSeconds(1);

        while (!selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_PointerEventData = new PointerEventData(m_EventSystem) {position = Input.mousePosition};
                List<RaycastResult> results = new List<RaycastResult>();

                m_Raycaster.Raycast(m_PointerEventData, results);
                this.results = results;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    target = hit.collider.gameObject;
                    selected = true;
                }
            }

            yield return null;
        }

        disableOtherInput = false;
//        yield return target;
    }

    IEnumerator waitAndDestroyTarget()
    {
        while (!selected)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (target.CompareTag("Model"))
        {
            GameState.getActivePlayer().boardMinions.Remove(target);
            GameState.getActivePlayer().graveyard.Add(target.GetComponent<monsterInfo>().card);
            Debug.Log("TARGET IS: " + target.name);
//            Debug.Log("ID IS : " + target.GetComponent<monsterInfo>().card.GetComponent<Card>().id);
            GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];



            topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;

            topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
            topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);

            topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);

            topGraveyardCard.SetActive(true);

            GameState.getActivePlayer().handCards.Remove(GameState.getActivePlayer().selectedCard);
            GameState.getActivePlayer().graveyard.Add(GameState.getActivePlayer().selectedCard);
            Destroy(GameState.getActivePlayer().selectedCard);


            Debug.Log("HAND CARDS SIZE: " + GameState.getActivePlayer().handCards.Count);
            target.GetComponent<monsterInfo>().parentPlayer.boardMinions.Remove(target);
            Destroy(target);


            //then we get the coordinates of the monster and set its square to free...
            GameState.boardTable[target.GetComponent<monsterInfo>().coords[0].First, target.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().isFree = true;

            cardEventHandler.onSummon -= fireball;
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
//                    selected = true; // stop coroutine
                    cardEventHandler.onSummon -= fireball;
                    break;
                }
            }
        }
    }

    private IEnumerator waitAndPyra()
    { 
        while (!selected && target == null)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (target.CompareTag("Model"))
        {
            //Get the minion to be destroyed's power
            int minionPower = target.GetComponent<monsterInfo>().power;
            //Deal its damage to both players
            DealDamageToPlayer(GameState.players[0], minionPower);
            DealDamageToPlayer(GameState.players[1], minionPower);
            //THIS IS WRONG: should add to parent player's graveyard
            GameState.getActivePlayer().graveyard.Add(target.GetComponent<monsterInfo>().card);

            GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];

            //This part shows the top graveyard card
            topGraveyardCard.SetActive(true);

            topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;

            topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
            topGraveyardCard.transform
                .localPosition = new Vector3(0, 0, 0);

            topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);

            //Remove the target from the board minions
            target.GetComponent<monsterInfo>().parentPlayer.boardMinions.Remove(target);
            Destroy(target);

            //then we get the coordinates of the monster and set its square to free...
            GameState.boardTable[target.GetComponent<monsterInfo>().coords[0].First,
                target.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().isFree = true;
            //then we destroy the card

            //GameState.getActivePlayer().handCards.RemoveAt(GameState.getActivePlayer().selectedCardIndex);

            GameState.getActivePlayer().cardSelected = false;

            cardEventHandler.onSummon -= pyra;
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    //                    selected = true; // stop coroutine
                    cardEventHandler.onSummon -= pyra;
                    break;
                }
            }
        }
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