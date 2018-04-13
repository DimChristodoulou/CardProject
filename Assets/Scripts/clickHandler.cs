using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*
 *  Generic class used to identify User-GUI interaction.
 */

public class clickHandler : MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
    IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Font m_Font;

    static int cardEntryOffset = 133;

    /*
     * Called the moment cursor is clicked.
     */
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Begin");
    }

    /*
     *  Called while cursor is held down and moving.
     */
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
    }


    /*
     *  Called when cursor stops being held down.
     */
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Drag Ended");
    }

   /*
    *  Called when the user clicks.
    */
    public void OnPointerClick(PointerEventData eventData)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("mainscene"))
        {
            //string clickedCard = eventData.pointerCurrentRaycast.gameObject.transform.parent.name;
            GameObject clickedObj = eventData.pointerCurrentRaycast.gameObject;
            string clickedCard = clickedObj.name;
            Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.transform.parent.name);
            if (clickedCard.Equals("CardDisplaySample(Clone)") || clickedObj.transform.parent.name.Equals("CardDisplaySample(Clone)") || clickedCard.Equals("CardDisplaySpellSample(Clone)") || clickedObj.transform.parent.name.Equals("CardDisplaySpellSample(Clone)"))
            {
                GameObject mainui = GameObject.Find("Main UI");
                GameObject creatureText = new GameObject("Summon_Creature_Text");
                creatureText.transform.SetParent(mainui.transform);
                Text newtext = creatureText.AddComponent<Text>();
                newtext.font = m_Font;
                newtext.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 50);
                newtext.fontSize = 36;
                newtext.text = "Choose where to summon the creature";
                newtext.color = UnityEngine.Color.black;
                newtext.transform.localPosition = new Vector3(100, 52, 0);
            }
        }
        else
        {
            //Debug.Log("DECK BUILD ACTIVE = " + deckbuilder.deckBuildActive);
            if (deckbuilder.deckBuildActive == true)
            {
                if (Player.deck.Count < 30)
                {
                    GameObject clickedObj = eventData.pointerCurrentRaycast.gameObject;
                    string clickedCard = clickedObj.name;
                    Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.transform.parent.name);
                    if (clickedCard.Equals("CardDisplaySample(Clone)") || clickedObj.transform.parent.name.Equals("CardDisplaySample(Clone)") || clickedCard.Equals("CardDisplaySpellSample(Clone)") || clickedObj.transform.parent.name.Equals("CardDisplaySpellSample(Clone)"))
                    {
                        GameObject cardListEntry = new GameObject("cardListEntry");
                        //cardListEntry = Instantiate(cardListEntry, GameObject.FindGameObjectWithTag("ListWithCards").transform);
                        cardListEntry.AddComponent<LayoutElement>();
                        cardListEntry.AddComponent<Text>();
                        //cardListEntry.GetComponent<Text>().font.name = "Arial";
                        cardListEntry.GetComponent<Text>().text = clickedObj.GetComponent<CardDisplay>().cardName.text;
                        cardListEntry.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                        cardListEntry.GetComponent<Text>().fontSize = 10;

                        //cardListEntry.transform.parent = GameObject.FindGameObjectWithTag("ListWithCards").transform;
                        Debug.Log("card list entry created!");
                        cardListEntry = Instantiate(cardListEntry, GameObject.FindGameObjectWithTag("ListWithCards").transform);
                        cardEntryOffset -= 20;
                        cardListEntry.transform.localPosition = new Vector3(3.382453f, cardEntryOffset, 0);
                        ((RectTransform)cardListEntry.transform).sizeDelta = new Vector2(118.89f, 17.58f);


                        Player.deck.Add(clickedObj.GetComponent<CardDisplay>().id);
                    }
                }
            }
        }
    }

   /*
    *  Called while cursor is stationary and held down.
    */
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
    }

   /*
    *  Called when cursor enters the space allocated to the object attached to this script.
    */
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse Exit");
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Equals("deckbuilder"))
        {
            Debug.Log("Mouse Enter");
            Transform hoveredCardName = eventData.pointerCurrentRaycast.gameObject.transform.parent.Find("name");
            Text textHoveredCardName = hoveredCardName.GetComponent<Text>();
            string s = textHoveredCardName.text.ToString();
            Text descText = GameObject.Find("description_text").GetComponent<Text>();
            int cardIndex = 0;
            for (; cardIndex < jsonparse.cards.Length; cardIndex++)
            {
                if (jsonparse.cards[cardIndex].card_name.Equals(s))
                    break;
            }
            Debug.Log("INDEX IS" + cardIndex);
            descText.text = jsonparse.cards[cardIndex].card_flavortext;
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
        Debug.Log("Mouse Up");
    }
}
