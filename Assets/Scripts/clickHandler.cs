using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;


/*
 *  Generic class used to identify User-GUI interaction.
 */

public class clickHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private static bool timesTwo = true;
    public Font m_Font;

    static int cardEntryOffset = 133;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    public void Start()
    {
        m_Raycaster = GameObject.FindGameObjectWithTag("Main UI").GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    /*
     * Called the moment cursor is clicked.
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag Begin");
    }

    /*
     *  Called while cursor is held down and moving.
     */
    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        //eventData.pointerCurrentRaycast.gameObject.transform.parent.position = Input.mousePosition;
    }


    /*
     *  Called when cursor stops being held down.
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Drag Ended");
    }

    /*
     *  Called when the user clicks.
     */
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject mainui = GameObject.Find("Main UI");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("mainscene"))
        {
<<<<<<< HEAD
            //RaycastHit hit;
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("CardLayer")))
            GameObject clickedObj = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
            if (clickedObj == null || clickedObj.name == "Main UI")
                clickedObj = eventData.pointerCurrentRaycast.gameObject;
            string clickedCard = clickedObj.name;
            if (clickedCard.Equals("CardDisplaySample(Clone)"))
            {
                GameState.getActivePlayer().setupPlayCard(clickedObj);
            }
            else if (clickedCard.Equals("CardDisplaySpellSample(Clone)"))
            {
                //Check for mana, wait for player to specify target and resolve effect.
            }
=======
            RaycastHit hit;
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    Debug.Log("RAYCAST COLLIDED WITH: " + result.gameObject.tag);
                    GameObject clickedObj = result.gameObject;
                    string clickedCard = clickedObj.name;
                    Debug.Log(clickedCard);
                    GameState.getActivePlayer().setupPlayCard(clickedObj);
                }
            }

//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            Debug.Log("a");
//            if (Physics2D.Raycast(ray, out hit, 1000/*, LayerMask.GetMask("CardLayer")*/))
//            {
//                Debug.Log("RAYCAST COLLIDED WITH: " + hit.collider.tag);
//                Debug.Log("a");
//                GameObject clickedObj = result.transform.gameObject;
//                //if (clickedObj == null || clickedObj.name == "Main UI")
//                //clickedObj = eventData.pointerCurrentRaycast.gameObject;
//                string clickedCard = clickedObj.name;
//                Debug.Log(clickedCard);
//                GameState.getActivePlayer().setupPlayCard(clickedObj);
//            }
>>>>>>> a43ab902c2fd23c7659cb20ffcb45904d50d54ea
        }
        else
        {
            if (deckbuilder.deckBuildActive == true)
            {
                if (Player.deck.Count < 30)
                {
//                    GameObject clickedObj = eventData.pointerCurrentRaycast.gameObject.transform.parent.gameObject;
//                    if (clickedObj == null)
//                        clickedObj = eventData.pointerCurrentRaycast.gameObject;
//                    RaycastHit hit;
                    m_PointerEventData = new PointerEventData(m_EventSystem);
                    m_PointerEventData.position = Input.mousePosition;
                    List<RaycastResult> results = new List<RaycastResult>();

                    m_Raycaster.Raycast(m_PointerEventData, results);


                    foreach (RaycastResult result in results)
                    {
                        if (result.gameObject.CompareTag("Card"))
                        {
<<<<<<< HEAD
                            GameObject cardListEntry = new GameObject("cardListEntry");
                            cardListEntry.gameObject.tag = "cardEntry";
                            cardListEntry.AddComponent<LayoutElement>();
                            cardListEntry.AddComponent<Text>();
                            cardListEntry.GetComponent<Text>().text = clickedObj.GetComponent<CardDisplay>().manaCost.text + clickedObj.GetComponent<CardDisplay>().cardName.text;
                            cardListEntry.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                            cardListEntry.GetComponent<Text>().fontSize = 10;

                            cardListEntry = Instantiate(cardListEntry, GameObject.FindGameObjectWithTag("ListWithCards").transform);
                            cardEntryOffset -= 20;
                            cardListEntry.transform.localPosition = new Vector3(3.382453f, cardEntryOffset, 0);
                            ((RectTransform)cardListEntry.transform).sizeDelta = new Vector2(118.89f, 17.58f);
                            Player.deck.Add(clickedObj.GetComponent<CardDisplay>().id);
                            timesTwo = false;
                        }
                        else if(Player.deck.Contains(clickedObj.GetComponent<CardDisplay>().id) && timesTwo == false)
                        {
                            GameObject[] cardEntries = GameObject.FindGameObjectsWithTag("cardEntry");
                            foreach (GameObject card in cardEntries)
                            {
                                if (card.GetComponent<Text>().text.Equals(clickedObj.GetComponent<CardDisplay>().manaCost.text + clickedObj.GetComponent<CardDisplay>().cardName.text))
                                {
                                    card.GetComponent<Text>().text = clickedObj.GetComponent<CardDisplay>().manaCost.text + clickedObj.GetComponent<CardDisplay>().cardName.text + " x2";             
                                    timesTwo = true;
=======
//                            Debug.Log("RAYCAST COLLIDED WITH: " + result.gameObject.tag);
                            GameObject clickedObj = result.gameObject;
                            string clickedCard = clickedObj.name;
                            Debug.Log(clickedCard);
                            if (!Player.deck.Contains(clickedObj.GetComponent<CardDisplay>().id))
                            {
                                GameObject cardListEntry = new GameObject("cardListEntry");
                                cardListEntry.gameObject.tag = "cardEntry";
                                cardListEntry.AddComponent<LayoutElement>();
                                cardListEntry.AddComponent<Text>();
                                cardListEntry.GetComponent<Text>().text =
                                    clickedObj.GetComponent<CardDisplay>().cardName.text;
                                cardListEntry.GetComponent<Text>().font =
                                    Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                                cardListEntry.GetComponent<Text>().fontSize = 10;

                                cardListEntry = Instantiate(cardListEntry,
                                    GameObject.FindGameObjectWithTag("ListWithCards").transform);
                                cardEntryOffset -= 20;
                                cardListEntry.transform.localPosition = new Vector3(3.382453f, cardEntryOffset, 0);
                                ((RectTransform) cardListEntry.transform).sizeDelta = new Vector2(118.89f, 17.58f);
                                Player.deck.Add(clickedObj.GetComponent<CardDisplay>().id);
                                timesTwo = false;
                            }
                            else if (Player.deck.Contains(clickedObj.GetComponent<CardDisplay>().id) &&
                                     timesTwo == false)
                            {
                                GameObject[] cardEntries = GameObject.FindGameObjectsWithTag("cardEntry");
                                foreach (GameObject card in cardEntries)
                                {
                                    if (card.GetComponent<Text>().text
                                        .Equals(clickedObj.GetComponent<CardDisplay>().cardName.text))
                                    {
                                        card.GetComponent<Text>().text =
                                            clickedObj.GetComponent<CardDisplay>().cardName.text + " x2";
                                        timesTwo = true;
                                    }
>>>>>>> a43ab902c2fd23c7659cb20ffcb45904d50d54ea
                                }

                                Player.deck.Add(clickedObj.GetComponent<CardDisplay>().id);
                            }

                            break;
                        }
                    }

//                    string clickedCard = clickedObj.name;
                }
            }
        }
    }

    /*
     * Function used to store the player's decks in a .txt file, located in the resources folder.
     */
    public static void SaveDeck()
    {
        string deckCode = "";
        foreach (int cardId in Player.deck)
        {
            deckCode += cardId.ToString() + " ";
        }

        string path = "Assets/Prefabs/Resources/playerDecks.txt";
        StreamReader reader = new StreamReader(path, true);
        string allText = reader.ReadToEnd();
        reader.Close();

        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(deckCode);
        writer.Close();
    }

    /*
     *  Called while cursor is stationary and held down.
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

    /*
     *  Called when cursor enters the space allocated to the object attached to this script.
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Mouse Exit");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("deckbuilder"))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();

            m_Raycaster.Raycast(m_PointerEventData, results);


            foreach (RaycastResult result in results)
            {
                Debug.Log("on pointer enter: " + result.gameObject.name);
                if (result.gameObject.CompareTag("Card"))
                {
                    Transform hoveredCardName =
                        result.gameObject.transform.parent.Find("name");
                    if (hoveredCardName == null)
                        hoveredCardName = result.gameObject.transform.Find("name");

                    Text textHoveredCardName = hoveredCardName.GetComponent<Text>();
                    string s = textHoveredCardName.text.ToString();
                    Text descText = GameObject.Find("description_text").GetComponent<Text>();
                    int cardIndex = 0;
                    for (; cardIndex < jsonparse.cards.Length; cardIndex++)
                    {
                        if (jsonparse.cards[cardIndex].card_name.Equals(s))
                            break;
                    }

                    descText.text = jsonparse.cards[cardIndex].card_flavortext;

                    break;
                }
            }
        }
    }

    /*
     *  Called when cursor exits the space allocated to the object attached to this script.
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("deckbuilder"))
        {
            Text descText = GameObject.Find("description_text").GetComponent<Text>();
            descText.text = "";
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Mouse Up");
    }
}