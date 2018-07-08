﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class deckbuilder : MonoBehaviour
{
    public InputField searchCard;

    public static bool deckBuildActive = false;

    private Card originalCard;
    private GameObject clonedCard;
    private jsonparse cardsJson;
    public GameObject main_ui;
    public GameObject deck_canvas_ui;
    private GameObject buttonA;
    private int currentEightCards = 0;

    private GameObject leftArrow;
    private GameObject rightArrow;
    public InputField myInputField;

    private string path;

    private int[] cardsToBeDisplayed;

    public TextAsset textFile;


    // Use this for initialization
    void Start()
    {

        Directory.CreateDirectory("Decks");
        path = "Decks//";
        textFile = (TextAsset) Resources.Load("playerDecks.txt");
        //TODO: Button as prefab
        cardsToBeDisplayed = new int[jsonparse.cardids.Length];
        jsonparse.cardids.CopyTo(cardsToBeDisplayed, 0);
        GameObject mainui = GameObject.Find("Main UI");
        cardsJson = new jsonparse();
        originalCard = new Card();
        int i = 0;
        DisplayCurrentEight(mainui, cardsToBeDisplayed);
    }

    /*
     * Function used to show the next eight cardTemplates on the screen
     */
    public void nextScreen(GameObject mainui, int[] cardsToOutput)
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
        currentEightCards++;
        DisplayCurrentEight(mainui, cardsToOutput);
    }

    /*
    * Function used to show the previous eight cardTemplates on the screen
    */
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

    /*
    * Function used to filter cardTemplates by their Attribute
    */
    public void filterByAttribute(string attribute)
    {
        GameObject mainui = GameObject.Find("Main UI");

        GameObject[] cardsToBeDestroyed;
        cardsToBeDestroyed = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cardsToBeDestroyed)
        {
            Destroy(card);
        }

        int[] filteredCards = new int[jsonparse.cardTemplates.Length];
        int j = 0;
        for (int i = 0; i < jsonparse.cardTemplates.Length; i++)
        {
            if (jsonparse.cardTemplates[i].card_attribute.Equals(attribute))
                filteredCards[j++] = jsonparse.cardTemplates[i].card_id;
        }


        // int[] shortFilteredCards = filteredCards.Where(i => Array.IndexOf(filteredCards, i) <= j).ToArray();
        //Array.Clear(filteredCards, j, filteredCards.Length - j);
        int[] shortFilteredCards = new int[j];
        Array.Copy(filteredCards, shortFilteredCards, j);
        Debug.Log("SIZE OF FILTERED CARDS: " + shortFilteredCards.Length);

        currentEightCards = 0;
        GameObject leftArrow = GameObject.Find("leftArrowButton(Clone)");
        Destroy(leftArrow);
        GameObject rightArrow = GameObject.Find("rightArrowButton(Clone)");
        Destroy(rightArrow);
        DisplayCurrentEight(mainui, shortFilteredCards);
    }

    /*
    * Function used to search the card database by a keyword
    */
    public void filterBySearchKeyword()
    {
        GameObject mainui = GameObject.Find("Main UI");
        GameObject[] cardsToBeDestroyed;
        cardsToBeDestroyed = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject card in cardsToBeDestroyed)
        {
            Destroy(card);
        }

        int[] filteredCards = new int[jsonparse.cardTemplates.Length];
        int j = 0;

        for (int i = 0; i < jsonparse.cardTemplates.Length; i++)
        {
            if (String.Equals(searchCard.GetComponent<InputField>().text, jsonparse.cardTemplates[i].card_type, StringComparison.OrdinalIgnoreCase))
                filteredCards[j++] = jsonparse.cardTemplates[i].card_id;
            else if (jsonparse.cardTemplates[i].card_name.ToLower().Contains(searchCard.GetComponent<InputField>().text.ToLower()) || jsonparse.cardTemplates[i].card_text.ToLower().Contains(searchCard.GetComponent<InputField>().text.ToLower()))
                filteredCards[j++] = jsonparse.cardTemplates[i].card_id;
        }

        int[] shortFilteredCards = filteredCards.Where(i => i != 0).ToArray();
        currentEightCards = 0;
        GameObject leftArrow = GameObject.Find("leftArrowButton(Clone)");
        Destroy(leftArrow);
        GameObject rightArrow = GameObject.Find("rightArrowButton(Clone)");
        Destroy(rightArrow);
        DisplayCurrentEight(mainui, shortFilteredCards);
    }

    /*
    * Function used to display 8 cardTemplates from the cardsToOutput array to the player.
    */
    void DisplayCurrentEight(GameObject mainui, int[] cardsToOutput)
    {
        int len = jsonparse.cardids.Length;

        int i;
        if ((currentEightCards + 1) * 8 < cardsToOutput.Length)
        {
            rightArrow = (GameObject) Instantiate(Resources.Load("rightArrowButton"));
            rightArrow.transform.SetParent(mainui.transform, false);
            rightArrow.transform.localPosition = new Vector3(230f, 0, 0);
            Button rAButton = rightArrow.GetComponent<Button>();
            rAButton.onClick.AddListener(() => nextScreen(mainui, cardsToOutput));
        }

        if (currentEightCards >= 1)
        {
            leftArrow = (GameObject) Instantiate(Resources.Load("leftArrowButton"));
            leftArrow.transform.SetParent(mainui.transform, false);
            leftArrow.transform.localPosition = new Vector3(-370f, 0, 0);
            Button lAButton = leftArrow.GetComponent<Button>();
            lAButton.onClick.AddListener(() => lastScreen(mainui, cardsToOutput));
        }

        for (i = 0; i < 8; i++)
        {
            if ((currentEightCards * 8) + i < cardsToOutput.Length)
            {
                if ((i / 4) % 2 == 0)
                    originalCard.initializeCard(-280f + (140 * (i % 4)), 70, 0, cardsToOutput[(currentEightCards * 8) + i]);
                else
                    originalCard.initializeCard(-280f + (140 * (i % 4)), -105, 0, cardsToOutput[(currentEightCards * 8) + i]);
            }
        }
    }

    /*
    * Function used to switch the active scene to the deck builder
    */
    public void deck_build()
    {
        main_ui.SetActive(false);
        deck_canvas_ui.SetActive(true);
    }

    public void onClickButtonDeckBuild(string attribute)
    {
        BuildDeck(attribute);
    }

    public void BuildDeck(string attribute)
    {
        deck_canvas_ui.SetActive(false);
        main_ui.SetActive(true);
        filterByAttribute(attribute);
        deckBuildActive = true;
        GameObject saveDeck = (GameObject) Instantiate(Resources.Load("Button"));
        saveDeck.transform.SetParent(main_ui.transform);
        saveDeck.transform.localPosition = new Vector3(83.3f, 174.89f, 0);
        ((RectTransform) saveDeck.transform).sizeDelta = new Vector2(100, 30);
        saveDeck.GetComponentInChildren<Text>().text = "SAVE DECK";
        saveDeck.GetComponent<Button>().onClick.AddListener(SaveDeck);
    }

    void SaveDeck()
    {
        string deckCode = myInputField.text + "☼";
        foreach (int cardId in GameState.deck)
        {
            deckCode += (cardId).ToString() + " ";
        }
        var file = File.Open(path + "Decks.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        StreamReader reader = new StreamReader(file);
        string allText = reader.ReadToEnd();
        reader.Close();

//        StreamWriter writer = new StreamWriter(file);
        System.IO.File.WriteAllText(path + "Decks.txt", allText + "\n" + deckCode + "\n");
//        writer.WriteLine(deckCode);
//        writer.Close();
    }
}