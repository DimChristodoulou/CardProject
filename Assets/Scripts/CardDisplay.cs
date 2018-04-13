using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {
    

    public GameObject card;

    private bool isInstantiated = false;

    public Text cardName;
    public Text description;
    public Text manaCost;
    public Text type;

    public Image artwork;
    public Image element;

    public Text power;


    void Start()
    {
        if (this.name.Equals("CardDisplaySample(Clone)"))                                   //Needs checking or the cloned object will clone again.
        {
            isInstantiated = true;
            //DisplayCard(0);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && isInstantiated==false)                           //Sample test for creating cards
        {
            initializeCard(-157,-150.9f,0,1);
            initializeCard(5, -150.9f, 0,1);
            initializeCard(166, -150.9f, 0,1);
            initializeCard(332, -150.9f, 0,1);
        }
    }

    /*
     * Function used to clone the card prefab and place it in the main UI.
     * Arguments: Vector3 Coordinates(where to instantiate the card)
     */
    public GameObject initializeCard(float x, float y, float z, int cardId)                                                              
    {
            GameObject mainui = GameObject.Find("Main UI");
            if(jsonparse.cards[cardId].card_type.Equals("Minion"))
                card = (GameObject)Instantiate(Resources.Load("CardDisplaySample"));
            else
                card = (GameObject)Instantiate(Resources.Load("CardDisplaySpellSample"));
            card.transform.SetParent(mainui.transform, false);
            card.GetComponent<CardDisplay>().DisplayCard(cardId);
            card.transform.localPosition = new Vector3(x, y, z);
            return card;
    }

    /*
     * Main function linking the json attributes to the GUI Elements of the Card object.
     */
    public void DisplayCard (int cardId)
    {
        cardName.GetComponent<Text>();
        cardName.text = jsonparse.cards[cardId].card_name;
        description.text = jsonparse.cards[cardId].card_text;
        manaCost.text = jsonparse.cards[cardId].card_manacost.ToString();
        type.text = jsonparse.cards[cardId].card_type.ToString();
        string isMinion = jsonparse.cards[cardId].card_type.ToString();
        if (isMinion.Equals("Minion"))
        {
            power.text = jsonparse.cards[cardId].card_actionpoints.ToString();
        }
        string attribute = jsonparse.cards[cardId].card_attribute.ToString();
        if (attribute.Equals("Fire"))
        {
            element.sprite = Resources.Load("fire", typeof(Sprite)) as Sprite;
        }
        else if (attribute.Equals("Light"))
        {
            element.sprite = Resources.Load("light", typeof(Sprite)) as Sprite;
        }
        //TODO: Other attributes...
        artwork.sprite = Resources.Load(jsonparse.cards[cardId].card_image.ToString(), typeof(Sprite)) as Sprite;
    }

}

