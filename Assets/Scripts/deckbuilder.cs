using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deckbuilder : MonoBehaviour {

    public static bool deckBuildActive = false;

    private CardDisplay originalCard;
    private GameObject clonedCard;
    private jsonparse cardsJson;
    public GameObject main_ui;
    public GameObject deck_canvas_ui;
    private GameObject buttonA;
    private int currentEightCards = 0;

    private GameObject leftArrow;
    private GameObject rightArrow;

    private int[] cardsToBeDisplayed;


    // Use this for initialization
    void Start () {
        //TODO: Button as prefab
        cardsToBeDisplayed = new int[jsonparse.cardids.Length];
        jsonparse.cardids.CopyTo(cardsToBeDisplayed, 0);
        GameObject mainui = GameObject.Find("Main UI");
        cardsJson = new jsonparse();
        originalCard = new CardDisplay();
        int i = 0;
        DisplayCurrentEight(mainui, cardsToBeDisplayed);
    }

    public void nextScreen(GameObject mainui, int[] cardsToOutput)
    {
        GameObject[] cardsToBeDestroyed;
        cardsToBeDestroyed = GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject card in cardsToBeDestroyed)
        {
            Destroy(card);
        }
        GameObject leftArrow = GameObject.Find("leftArrowButton(Clone)");
        Destroy(leftArrow);
        GameObject rightArrow = GameObject.Find("rightArrowButton(Clone)");
        Destroy(rightArrow);
        currentEightCards++;
        DisplayCurrentEight(mainui, cardsToOutput);
    }

    public void lastScreen(GameObject mainui, int[] cardsToOutput)
    {
        GameObject[] cardsToBeDestroyed;
        cardsToBeDestroyed = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cardsToBeDestroyed)
        {
            Destroy(card);
        }
        GameObject leftArrow = GameObject.Find("leftArrowButton(Clone)");
        Destroy(leftArrow);
        GameObject rightArrow = GameObject.Find("rightArrowButton(Clone)");
        Destroy(rightArrow);
        currentEightCards--;
        DisplayCurrentEight(mainui, cardsToOutput);
    }

    public void filterByAttribute(String attribute)
    {
        GameObject mainui = GameObject.Find("Main UI");
        int[] lightCards = new int[jsonparse.cards.Length];
        int j = 0;
        for(int i=0; i< jsonparse.cards.Length; i++)
        {
            if (jsonparse.cards[i].card_attribute.Equals(attribute))
                lightCards[j++] = jsonparse.cards[i].card_id;
        }
        DisplayCurrentEight(mainui, lightCards);
    }


    void DisplayCurrentEight(GameObject mainui, int[] cardsToOutput)
    {
        int i;
        if (currentEightCards == 0)
        {
            rightArrow = (GameObject)Instantiate(Resources.Load("rightArrowButton"));
            rightArrow.transform.SetParent(mainui.transform, false);
            rightArrow.transform.localPosition = new Vector3(230f, 0, 0);
            Button rAButton = rightArrow.GetComponent<Button>();
            rAButton.onClick.AddListener(() => nextScreen(mainui, cardsToOutput));
        }
        else if(currentEightCards*8 >= cardsToOutput.Length)
        {
            leftArrow = (GameObject)Instantiate(Resources.Load("leftArrowButton"));
            leftArrow.transform.SetParent(mainui.transform, false);
            leftArrow.transform.localPosition = new Vector3(-370f, 0, 0);
            Button lAButton = leftArrow.GetComponent<Button>();
            lAButton.onClick.AddListener(() => lastScreen(mainui, cardsToOutput));
        }
        else
        {
            rightArrow = (GameObject)Instantiate(Resources.Load("rightArrowButton"));
            rightArrow.transform.SetParent(mainui.transform, false);
            rightArrow.transform.localPosition = new Vector3(230f, 0, 0);
            Button rAButton = rightArrow.GetComponent<Button>();
            rAButton.onClick.AddListener(() => nextScreen(mainui, cardsToOutput));

            leftArrow = (GameObject)Instantiate(Resources.Load("leftArrowButton"));
            leftArrow.transform.SetParent(mainui.transform, false);
            leftArrow.transform.localPosition = new Vector3(-370f, 0, 0);
            Button lAButton = leftArrow.GetComponent<Button>();
            lAButton.onClick.AddListener(() => lastScreen(mainui, cardsToOutput));
        }
        for(i = 0; i < 8; i++)
        {
            if((currentEightCards*8)+i < cardsToOutput.Length)
            {
                if ((i / 4) % 2 == 0)
                    originalCard.initializeCard(-280f + (140 * (i % 4)), 70, 0, (currentEightCards * 8) + cardsToOutput[i]);
                else
                    originalCard.initializeCard(-280f + (140 * (i % 4)), -105, 0, (currentEightCards * 8) + cardsToOutput[i]);
            }
        }
    }

    public void deck_build()
    {
        main_ui.SetActive(false);
        deck_canvas_ui.SetActive(true);
    }

    public void onClickButtonDeckBuild(String attribute)
    {
        deck_canvas_ui.SetActive(false);
        main_ui.SetActive(true);
        filterByAttribute(attribute);
        deckBuildActive = true;
        GameObject saveDeck = (GameObject)Instantiate(Resources.Load("Button"));
        Debug.Log("Created!");
        saveDeck.transform.SetParent(main_ui.transform);
        saveDeck.transform.localPosition = new Vector3(83.3f, 174.89f, 0);
        ((RectTransform)saveDeck.transform).sizeDelta = new Vector2(100, 30);
        saveDeck.GetComponentInChildren<Text>().text = "SAVE DECK";
        saveDeck.GetComponent<Button>().onClick.AddListener(clickHandler.SaveDeck);
    }

}
