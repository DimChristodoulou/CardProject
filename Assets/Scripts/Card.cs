using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Card{

    /*DB Independent*/
    public GameObject cardObject;

    /*DB Dependend*/
    public int card_id;
    public string card_name;
    public int card_manacost;
    public string card_image;
    public string card_ingameModel;
    public string card_text;
    public string card_set;
    public string card_attribute;
    public string card_type;
    public string card_tags;
    public string card_actionpoints;
}
