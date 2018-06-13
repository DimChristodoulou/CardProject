using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Wrapper for buffs, need it to store info for buffs inside monsterInfo
public class buffs
{
    public string buffName;
    public int buffAmount = 0;
    public int buffCardId;
}

public class monsterInfo : MonoBehaviour
{
    //utility class for general info regarding monster

    //card stats saved in the class
    public string monsterName;
    public string monsterKeywords;
    public GameObject card; //the card of the monster
    public List<buffs> monsterBuffs = new List<buffs>();

    public int
        power,
        manacost,
        movspeed,
        attkrange; //these will affect offsets to card values, but the card is needed for that to be implemented (TODO post-merge)

    //stores pairs of coordinates that this monster sits on, indexed [0,size-1]
    public List<Pair<int, int>> coords;
    public Color hoverColorAttacked = new Color32(0xFF, 0x00, 0x00, 0x8F); // RGBA
    public Color hoverColorActive = new Color32(0x00, 0x99, 0x00, 0x8F); // RGBA
    public Color hoverColorInactive = new Color32(0x99, 0x99, 0x00, 0x8F); // RGBA
    public Color startColor = new Color32(0xFF, 0xFF, 0xFF, 0x00); // RGBA

    //a monster is controlled by a player
    public Player parentPlayer;

    //a monster can be clicked, move, attack
    public bool clickable, movable, attackable;

    public bool cannotMove = false, cannotAttack = false;

    //the turn the monster was played
    public int playedturn;
    public GameObject powerTooltipOfMonster;

    //todo need to find a way to store enchants without changing originals, making silence applicable

    public void addBuff(string name, int amount, int cardID)
    {
        buffs tempBuff = new buffs();
        tempBuff.buffName = name;
        tempBuff.buffAmount = amount;
        tempBuff.buffCardId = cardID;
        monsterBuffs.Add(tempBuff);
    }

    public void setData(string mName, int pow, int mcost, int mspeed, int attkrange, Player parent, int summonTurn, string keywords, GameObject monsterCard)
    {
        monsterName = mName;
        power = pow;
        manacost = mcost;
        movspeed = mspeed;
        this.attkrange = attkrange;
        parentPlayer = parent;
        playedturn = summonTurn;
        monsterKeywords = keywords;
        if (monsterCard != null)
            Debug.Log("SELECTED CARD NAME: " + monsterCard.GetComponent<Card>().cardName.text);
        card = new GameObject();
        card = monsterCard;
    }

    public void setPosition(List<Pair<int, int>> myList)
    {
        coords = myList;
    }

    // Use this for initialization
    void Start()
    {
        powerTooltipOfMonster = GameState.boardTable[this.coords[0].First, this.coords[0].Second].GetComponent<nodeInfo>().powerTooltip;
        if (monsterKeywords != null)
        {
            if (monsterKeywords.Contains("Charge"))
            {
                clickable = true;
                movable = true;
                attackable = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(powerTooltipOfMonster!=null){
            powerTooltipOfMonster.transform.position = Camera.main.WorldToScreenPoint(this.transform.localPosition) + new Vector3(0, -50 - (CameraMovement.startingZoomLevel - CameraMovement.zoomLevel), 0);
        }
    }

    public void refreshBuffs(buffs buff)
    {
        if (buff.buffAmount != 0)
            this.power += buff.buffAmount;
    }

    public void onStartTurn()
    {
        //effects when starting a turn
        if (!cannotAttack && !cannotMove && playedturn != GameState.turn)
        {
            clickable = true;
            movable = true;
            attackable = true;
        }
        else
        {
            clickable = false;
            movable = false;
            attackable = false;
        }
    }

    public void onEndTurn()
    {
        //effects when ending a turn
    }

    public void onPostMove()
    {
        //effects after moving
        movable = false;
    }

    public void onPostAttack()
    {
        //effects after attacking once
        attackable = false;
    }

    public void onPostDefense()
    {
        //effects after defending once
        Debug.Log("help");
    }

    public void onPostHeal()
    {
        //effects after heal
    }

    public void onPostDeath()
    {
        //deathrattles
    }

	public void Die(int dmgDiff = 0)
    {
		parentPlayer.DieMonster(this.gameObject, dmgDiff);
        onPostDeath();
    }

    public void Banish()
    {
        parentPlayer.BanishMonster(this.gameObject);
    }
}