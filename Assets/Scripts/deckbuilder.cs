using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deckbuilder : MonoBehaviour {

    private CardDisplay originalCard;
    private GameObject clonedCard;
    private jsonparse cardsJson;
    public GameObject main_ui;
    public GameObject deck_canvas_ui;
    private GameObject buttonA;
    private int currentEightCards = 0;

    private GameObject leftArrow;
    private GameObject rightArrow;


    // Use this for initialization
    void Start () {
        //TODO: Button as prefab
        GameObject mainui = GameObject.Find("Main UI");
        cardsJson = new jsonparse();
        originalCard = new CardDisplay();
        int i = 0;
        DisplayCurrentEight(mainui);
        /*while(i<jsonparse.cards.Length)
        {
            if (i < jsonparse.cards.Length)
            {
                rightArrow = (GameObject)Instantiate(Resources.Load("rightArrowButton"));
                rightArrow.transform.SetParent(mainui.transform, false);
            }
            if (i > currentEightCards){
                leftArrow = (GameObject)Instantiate(Resources.Load("leftArrowButton"));
                leftArrow.transform.SetParent(mainui.transform, false);
            }
            if ((i / 4) % 2 == 0)
                originalCard.initializeCard(-330f + (169.5f * (i % 4)), 86, 0, i);
            else
                originalCard.initializeCard(-330f + (169.5f * (i % 4)), -86, 0, i);
            i++;
        }*/
    }

    public void nextScreen(GameObject mainui)
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
        DisplayCurrentEight(mainui);
    }

    public void lastScreen(GameObject mainui)
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
        DisplayCurrentEight(mainui);
    }

    void DisplayCurrentEight(GameObject mainui)
    {
        int i;
        if (currentEightCards == 0)
        {
            rightArrow = (GameObject)Instantiate(Resources.Load("rightArrowButton"));
            rightArrow.transform.SetParent(mainui.transform, false);
            rightArrow.transform.localPosition = new Vector3(250f, 0, 0);
            Button rAButton = rightArrow.GetComponent<Button>();
            rAButton.onClick.AddListener(() => nextScreen(mainui));
        }
        else if(currentEightCards*8 >= jsonparse.cards.Length)
        {
            leftArrow = (GameObject)Instantiate(Resources.Load("leftArrowButton"));
            leftArrow.transform.SetParent(mainui.transform, false);
            leftArrow.transform.localPosition = new Vector3(-320f, 0, 0);
            Button lAButton = leftArrow.GetComponent<Button>();
            lAButton.onClick.AddListener(() => lastScreen(mainui));
        }
        else
        {
            rightArrow = (GameObject)Instantiate(Resources.Load("rightArrowButton"));
            rightArrow.transform.SetParent(mainui.transform, false);
            rightArrow.transform.localPosition = new Vector3(250f, 0, 0);
            Button rAButton = rightArrow.GetComponent<Button>();
            rAButton.onClick.AddListener(() => nextScreen(mainui));

            leftArrow = (GameObject)Instantiate(Resources.Load("leftArrowButton"));
            leftArrow.transform.SetParent(mainui.transform, false);
            leftArrow.transform.localPosition = new Vector3(-320f, 0, 0);
            Button lAButton = leftArrow.GetComponent<Button>();
            lAButton.onClick.AddListener(() => lastScreen(mainui));
        }
        for(i = 0; i < 8; i++)
        {
            if((currentEightCards*8)+i < jsonparse.cards.Length)
            {
                if ((i / 4) % 2 == 0)
                    originalCard.initializeCard(-330f + (169.5f * (i % 4)), 86, 0, (currentEightCards * 8) + i);
                else
                    originalCard.initializeCard(-330f + (169.5f * (i % 4)), -86, 0, (currentEightCards * 8) + i);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void deck_build()
    {
        main_ui.SetActive(false);
        deck_canvas_ui.SetActive(true);
    }

}
