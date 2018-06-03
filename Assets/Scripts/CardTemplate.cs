using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CardTemplate{

    /*DB Independent*/
    public GameObject cardObject;

    /*DB Dependent*/
    public int card_id;
    public string card_name;
    public int card_manacost;
    public string card_image;
    public string card_ingameModel;
    public string card_text;
    public string card_set;
    public string card_attribute;
    public string card_type;
    public string card_keywords;
    public int card_actionpoints;
    public string card_flavortext;
    public int card_movement;
    public string card_rarity;
    public string card_tribe;
}
