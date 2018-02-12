using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour {
    

    public GameObject card;

    public Text cardName;
    public Text description;
    public Text manaCost;

    public Image artwork;
    public Image element;

    public Text meleeAttack;
    public Text rangedAttack;

    void Start(){
        DisplayCard(0);
    }

    public void DisplayCard (int cardId)
    {
        Debug.Log(jsonparse.cards[cardId].card_name);
        cardName.GetComponent<Text>();
        cardName.text = jsonparse.cards[cardId].card_name;
        description.text = jsonparse.cards[cardId].card_text;
        manaCost.text = jsonparse.cards[cardId].card_manacost.ToString();
        string isMinion = jsonparse.cards[cardId].card_type.ToString();
        if (isMinion.Equals("Minion"))
        {
            meleeAttack.text = jsonparse.cards[cardId].card_actionpoints.ToString();
        }
        artwork.sprite = Resources.Load(jsonparse.cards[cardId].card_image.ToString(), typeof(Sprite)) as Sprite;
    }
    
}
