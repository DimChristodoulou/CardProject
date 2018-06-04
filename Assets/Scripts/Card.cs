using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.EventSystems;
using System;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    
    public GameObject card;
    public static List<string> cardKeywords; 
    private bool isInstantiated = false;

    public Text cardName;
    public Text description;
    public Text manaCost;
    public Text type;

    public Image artwork;
    public Image element;

    public Text power;
    public int id;

    // for pointer handlers
    private static bool timesTwo = true;
    public Font m_Font;
    static int cardEntryOffset = 133;
    public bool pointerEventsEnabled;

    void Awake()
    {
        pointerEventsEnabled = true;
    }

    void Start()
    {
        if (this.name.Equals("CardDisplaySample(Clone)"))                                   //Needs checking or the cloned object will clone again.
        {
            isInstantiated = true;
        }
    }

    /*
     * Function used to clone the card prefab and place it in the main UI.
     * Arguments: Vector3 Coordinates(where to instantiate the card)
     */
    public GameObject initializeCard(float x, float y, float z, int cardId)                                                              
    {
            /*This prints the card*/
            GameObject mainui = GameObject.Find("Main UI");
            if(jsonparse.cardTemplates[cardId].card_type.Equals("Minion"))
                card = (GameObject)Instantiate(Resources.Load("CardDisplaySample"));
            else
                card = (GameObject)Instantiate(Resources.Load("CardDisplaySpellSample"));
            card.transform.SetParent(mainui.transform, false);
            card.GetComponent<Card>().DisplayCard(cardId);
            card.transform.localPosition = new Vector3(x, y, z);
            return card;
    }

    /*
     * Main function linking the json attributes to the GUI Elements of the CardTemplate object.
     */
    public void DisplayCard (int cardId)
    {
        id = cardId;
        
        cardName.GetComponent<Text>();
        cardName.text = jsonparse.cardTemplates[cardId].card_name;
        description.text = jsonparse.cardTemplates[cardId].card_text;
        manaCost.text = jsonparse.cardTemplates[cardId].card_manacost.ToString();
        type.text = jsonparse.cardTemplates[cardId].card_type.ToString();
        string isMinion = jsonparse.cardTemplates[cardId].card_type.ToString();
        if (isMinion.Equals("Minion"))
        {
            power.text = jsonparse.cardTemplates[cardId].card_actionpoints.ToString();
        }
        string attribute = jsonparse.cardTemplates[cardId].card_attribute.ToString();
        if (attribute.Equals("Fire"))
        {
            element.sprite = Resources.Load("fire", typeof(Sprite)) as Sprite;
        }
        else if (attribute.Equals("Light"))
        {
            element.sprite = Resources.Load("light", typeof(Sprite)) as Sprite;
        }
        else if (attribute.Equals("Dark"))
        {
            element.sprite = Resources.Load("dark", typeof(Sprite)) as Sprite;
        }
        //TODO: Other attributes...
        artwork.sprite = Resources.Load(jsonparse.cardTemplates[cardId].card_image.ToString(), typeof(Sprite)) as Sprite;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (pointerEventsEnabled)
        {
            Debug.Log("HIIII");
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name.Equals("deckbuilder"))
            {
                Text descText = GameObject.Find("description_text").GetComponent<Text>();
                descText.text = jsonparse.cardTemplates[id].card_flavortext;
            }
            else
            {
                gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                gameObject.transform.localPosition = gameObject.transform.localPosition + new Vector3(0, 75, 0);
                gameObject.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            }
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (pointerEventsEnabled)
        {
            Debug.Log("BYE");

            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name.Equals("deckbuilder"))
            {
                Text descText = GameObject.Find("description_text").GetComponent<Text>();
                descText.text = "";
            }
            else
            {
                gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                gameObject.transform.localPosition = gameObject.transform.localPosition - new Vector3(0, 75, 0);
                gameObject.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            }
        }
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (pointerEventsEnabled)
        {
            GameObject mainui = GameObject.Find("Main UI");
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name.Equals("mainscene"))
                GameState.getActivePlayer().setupPlayCard(gameObject);
            else
            {
                if (deckbuilder.deckBuildActive == true)
                {
                    if (Player.deck.Count < 30)
                    {
                        string clickedCard = gameObject.name;
                        Debug.Log(clickedCard);
                        if (!Player.deck.Contains(gameObject.GetComponent<Card>().id + 1))
                        {
							GameObject cardListEntry = (GameObject) Instantiate(Resources.Load("cardListEntry"));
                            cardListEntry.gameObject.tag = "cardEntry";
							cardListEntry.GetComponent<Text> ().text = "(" + gameObject.GetComponent<Card> ().manaCost.text + ") ";
                            cardListEntry.GetComponent<Text>().text +=
                                gameObject.GetComponent<Card>().cardName.text;
                            cardListEntry.GetComponent<Text>().font =
                                Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                            cardListEntry.GetComponent<Text>().fontSize = 10;

                            cardListEntry = Instantiate(cardListEntry,
                                GameObject.FindGameObjectWithTag("ListWithCards").transform);
                            cardEntryOffset -= 20;
                            cardListEntry.transform.localPosition = new Vector3(3.382453f, cardEntryOffset, 0);
                            ((RectTransform)cardListEntry.transform).sizeDelta = new Vector2(118.89f, 17.58f);
                            Player.deck.Add(gameObject.GetComponent<Card>().id + 1);
                            timesTwo = false;
                        }
                        else if (Player.deck.Contains(gameObject.GetComponent<Card>().id + 1) && timesTwo == false)
                        {
                            GameObject[] cardEntries = GameObject.FindGameObjectsWithTag("cardEntry");
                            foreach (GameObject card in cardEntries)
                            {
								if (card.GetComponent<Text>().text.Equals("(" + gameObject.GetComponent<Card> ().manaCost.text + ") " + gameObject.GetComponent<Card>().cardName.text))
                                {
                                    card.GetComponent<Text>().text += " x2";
                                    timesTwo = true;
                                }
                            }
                            Player.deck.Add(gameObject.GetComponent<Card>().id + 1);
                        }
                    }
                }
            }
        }
    }
}

