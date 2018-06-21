using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;


public class cardEffects : MonoBehaviour
{
    private int curX, curY;
    private Vector3 currentWorldPos;
    private cardEventHandler cardEvents;
    public static bool disableOtherInput = false;
    public static cardEffects instance = null;
    private graveyard graveyardGO;
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private GameObject target;
    private List<RaycastResult> results;
    private bool selected;


    // Use this for initialization
    void Start()
    {
        if (instance == null)
            instance = this;
        //        cardEventHandler.onSummon += flamesprite;
        //        cardEventHandler.onSummon += fireball;
        graveyardGO = GameObject.Find("graveyard").GetComponent<graveyard>();
        m_Raycaster = GameObject.FindGameObjectWithTag("Main UI").GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    public void setUpDelegate(string minionName)
    {
        switch (minionName)
        {
            case "Flamesprite":
            {
                cardEventHandler.onSummon += flamesprite;
                break;
            }
            case "Fireball":
            {
                cardEventHandler.onSummon += fireball;
                break;
            }
            case "The Emperor's Hound":
            {
                cardEventHandler.onSummon += emperorsHound;
                break;
            }
            case "Firewraith":
            {
                cardEventHandler.onSummon += firewraith;
                break;
            }
            case "Pyra, the Elemental Lord":
            {
                cardEventHandler.onSummon += pyra;
                break;
            }
            case "The Emperor's Fool":
            {
                //TODO: card effect
                cardEventHandler.onSummon += emperorsFool;
                break;
            }
            case "The Emperor's Knight":
            {
                cardEventHandler.onSummon += emperorsKnight;
                break;
            }
            case "Iron Resolve":
            {
                cardEventHandler.onSummon += ironResolve;
                break;
            }
            case "Idol of Fire":
            {
                cardEventHandler.onSummon += idolOfFire;
                break;
            }
            case "Flame Shaman":
            {
                //TODO: while in play
                cardEventHandler.onSummon += flameShaman;
                break;
            }
            case "Burning Walls":
            {
                cardEventHandler.onSummon += burningWalls;
                break;
            }
            case "Elleron, the Raging Fire":
            {
                //TODO: Fly, Speed 4
                cardEventHandler.onSummon += elleron;
                break;
            }
            case "Firestorm":
            {
                cardEventHandler.onSummon += firestorm;
                break;
            }
            case "Effigy of Flames":
            {
                cardEventHandler.onSummon += effigyOfFlames;
                break;
            }
            case "Flame Warper":
            {
                cardEventHandler.onSummon += flameWarper;
                break;
            }
            case "Fire Elemental":
            {
                cardEventHandler.onSummon += fireElemental;
                break;
            }
            case "Fire Giant":
            {
                cardEventHandler.onSummon += fireGiant;
                break;
            }
        }
    }


    public void fireGiant(string minionName){
        GameObject thisMinion = GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1];
        List<GameObject> adjacentMinions = getAdjacentMinions(GameState.boardTable, thisMinion.GetComponent<monsterInfo>().coords[0].First, thisMinion.GetComponent<monsterInfo>().coords[0].Second);
        foreach(GameObject minionGO in adjacentMinions){
            minionGO.GetComponent<monsterInfo>().power -= 5;
            updatePowerTooltip(minionGO);
        }
    }

    public void fireElemental(string minionName){
        GameObject thisMinion = GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1];
        List<GameObject> adjacentMinions = getAdjacentMinions(GameState.boardTable, thisMinion.GetComponent<monsterInfo>().coords[0].First, thisMinion.GetComponent<monsterInfo>().coords[0].Second);
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(destroyTargetIfAdjacent(adjacentMinions));
    }

    public void flameWarper(string minionName){
        curX = GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1].GetComponent<monsterInfo>().coords[0].First;
        curY = GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1].GetComponent<monsterInfo>().coords[0].Second;
        currentWorldPos = GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1].transform.position;
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(switchPosWithTarget());
    }

    public IEnumerator switchPosWithTarget(){
        while (!selected && target == null)
        {
            yield return new WaitForSeconds(0.2f);
        }
        if (target.CompareTag("Minion"))
        {
            int targetX = target.GetComponent<monsterInfo>().coords[0].First;
            int targetY = target.GetComponent<monsterInfo>().coords[0].Second;
            Vector3 targetWorldPos = target.transform.position;
            Vector3 temp = currentWorldPos;
            GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1].transform.position = targetWorldPos;
            target.transform.position = temp;
            List<Pair<int, int>> summonNodes = new List<Pair<int, int>>() { new Pair<int, int>(curX, curY) };
            List<Pair<int, int>> summonTargetNodes = new List<Pair<int, int>>() { new Pair<int, int>(targetX, targetY) };
            GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1].GetComponent<monsterInfo>().setPosition(summonTargetNodes);
            target.GetComponent<monsterInfo>().setPosition(summonNodes);
            cardEventHandler.onSummon -= flameWarper;
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    //selected = true; // stop coroutine
                    cardEventHandler.onSummon -= flameWarper;
                    break;
                }
            }
        }
        target = null;

    }

    public void effigyOfFlames(string minionName){
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(copyTarget());
    }


    public void firestorm(string minionName){
        GameObject[] allMinions = GameObject.FindGameObjectsWithTag("Minion");

        GameState.getActivePlayer().cardSelected = false;

        GameState.getActivePlayer().graveyard.Add(GameState.getActivePlayer().selectedCard);

        for(int i=0; i<allMinions.Length; i++){
            GameObject minionGO = allMinions[i];
            if(minionGO.GetComponent<monsterInfo>().power <= 12){
                //Remove the minion from its player's board minions
                minionGO.GetComponent<monsterInfo>().parentPlayer.boardMinions.Remove(minionGO);
                //Add the minion to its player's graveyard cards.
                minionGO.GetComponent<monsterInfo>().parentPlayer.graveyard.Add(minionGO.GetComponent<monsterInfo>().card);
                //Make the minion's node free.
                GameState.boardTable[minionGO.GetComponent<monsterInfo>().coords[0].First, minionGO.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().isFree = true;
                GameObject topGraveyardCard = minionGO.GetComponent<monsterInfo>().parentPlayer.graveyard[minionGO.GetComponent<monsterInfo>().parentPlayer.graveyard.Count - 1];
                Destroy(minionGO);
                Destroy(minionGO.GetComponent<monsterInfo>().powerTooltipOfMonster);
                topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;
                topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
                topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);
                topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);
                topGraveyardCard.SetActive(true);
            }
        }
        GameState.getActivePlayer().reorderHandCards();
        GameState.getActivePlayer().handCards.Remove(GameState.getActivePlayer().selectedCard);
        cardEventHandler.onSummon -= firestorm;
    }

    public void elleron(string minionName){
        GameObject[] allMinions = GameObject.FindGameObjectsWithTag("Minion");
        for(int i=0; i<allMinions.Length; i++){
            GameObject minionGO = allMinions[i];
            if(i!=allMinions.Length-1){
                //Remove the minion from its player's board minions
                minionGO.GetComponent<monsterInfo>().parentPlayer.boardMinions.Remove(minionGO);
                //Add the minion to its player's graveyard cards.
                minionGO.GetComponent<monsterInfo>().parentPlayer.graveyard.Add(minionGO.GetComponent<monsterInfo>().card);
                //Make the minion's node free.
                GameState.boardTable[minionGO.GetComponent<monsterInfo>().coords[0].First, minionGO.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().isFree = true;
                GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];
                Destroy(minionGO);
                Destroy(minionGO.GetComponent<monsterInfo>().powerTooltipOfMonster);
                topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;
                topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
                topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);
                topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);
                topGraveyardCard.SetActive(true);
            }
        }
        GameState.getActivePlayer().handCards.Remove(GameState.getActivePlayer().selectedCard);
        cardEventHandler.onSummon -= elleron;
    }

    public void burningWalls(string minionName){
        int x,y;
        int maxX = GameState.boardTable.GetLength(0);
        int maxY = GameState.boardTable.GetLength(1);
        foreach(GameObject coords in GameState.boardTable){
            x = coords.GetComponent<nodeInfo>().xpos;
            y = coords.GetComponent<nodeInfo>().ypos;
            if(x == 0 || x == maxX-1 || y == 0 || y == maxY-1){
                if(!coords.GetComponent<nodeInfo>().isFree && coords.GetComponent<nodeInfo>().monsterOnNode != null){
                    Destroy( coords.GetComponent<nodeInfo>().monsterOnNode);
                    Destroy( coords.GetComponent<nodeInfo>().monsterOnNode.GetComponent<monsterInfo>().powerTooltipOfMonster);
                    GameState.getActivePlayer().boardMinions.Remove(coords.GetComponent<nodeInfo>().monsterOnNode);
                    GameState.getActivePlayer().graveyard.Add(coords.GetComponent<nodeInfo>().monsterOnNode.GetComponent<monsterInfo>().card);
                }
            }
        }

        GameState.getActivePlayer().cardSelected = false;

        GameState.getActivePlayer().handCards.Remove(GameState.getActivePlayer().selectedCard);
        GameState.getActivePlayer().graveyard.Add(GameState.getActivePlayer().selectedCard);

        GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];
        topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;
        topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
        topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);
        topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);
        topGraveyardCard.SetActive(true);

        cardEventHandler.onSummon -= burningWalls;
    }

    public void flameShaman(string minionName){
        //TODO: Make this always while this is in play, instead of one time.
        List<GameObject> allGameObjects = GetAllObjectsInScene();
        foreach(GameObject GO in allGameObjects){
            if(GO.GetComponent<monsterInfo>()!=null && GO.GetComponent<monsterInfo>().card != null){
                if(GO.GetComponent<monsterInfo>().card.GetComponent<Card>().attribute == "Fire"){
                    GO.GetComponent<monsterInfo>().power += 2;
                    updatePowerTooltip(GO);
                }
            }
        }
    }

    public void idolOfFire(string minionName){
        GameState.getActivePlayer().boardMinions.Last().GetComponent<monsterInfo>().cannotMove = true;
        GameState.getActivePlayer().boardMinions.Last().GetComponent<monsterInfo>().cannotAttack = true;
        cardEventHandler.onSummon -= idolOfFire;
    }

    public void ironResolve(string minionName)
    {
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(targetedIronResolve());
    }

    private IEnumerator targetedIronResolve()
    {
        while (!selected && target == null)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (target.CompareTag("Minion"))
        {
            buffs ironResolveBuff = new buffs();
            ironResolveBuff.buffAmount = 3;
            ironResolveBuff.buffCardId = 10;
            ironResolveBuff.buffName = "Iron Resolve";

            target.GetComponent<monsterInfo>().refreshBuffs(ironResolveBuff);
            updatePowerTooltip(target);
            GameState.getActivePlayer().cardSelected = false;

            GameState.getActivePlayer().handCards.Remove(GameState.getActivePlayer().selectedCard);
            GameState.getActivePlayer().graveyard.Add(GameState.getActivePlayer().selectedCard);

            GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];
            topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;
            Debug.Log("TGC" + topGraveyardCard.GetComponent<Card>().cardName.text);
            topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
            topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);
            topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);
            topGraveyardCard.SetActive(true);
            
            //Reorder cards here until we do better refactoring
            GameState.getActivePlayer().reorderHandCards();

            cardEventHandler.onSummon -= ironResolve;

            //Destroy(GameState.getActivePlayer().selectedCard);
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    //selected = true; // stop coroutine
                    cardEventHandler.onSummon -= ironResolve;
                    break;
                }
            }
        }
        target = null;
    }

    public void emperorsKnight(string minionName)
    {
        cardEventHandler.onSummon -= emperorsKnight;
    }

    public void emperorsHound(string minionName)
    {
        cardEventHandler.onSummon -= emperorsHound;
    }

    public void firewraith(string minionName)
    {
        cardEventHandler.onSummon -= firewraith;
    }

    public void emperorsFool(string minionName)
    {
        cardEventHandler.onSummon -= emperorsFool;
    }

    /*
     * Function used to handle the effect of the flamesprite card (card ID = 7)
     */
    public void flamesprite(string minionName)
    {
        //GameState.getActivePlayer().currentMana -= jsonparse.cardTemplates[1].card_manacost;
        Player opponent;
        if (GameState.activePlayerIndex == 0)
            opponent = GameState.players[1];
        else
            opponent = GameState.players[0];

        DealDamageToPlayer(opponent, 5);
        cardEventHandler.onSummon -= flamesprite;
    }

    public void fireball(string spellName)
    {
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(waitAndDestroyTarget());
    }

    public void pyra(string minionName)
    {
        StartCoroutine(waitForUserToSelectTarget());
        StartCoroutine(waitAndPyra());
    }



    private IEnumerator waitAndPyra()
    { 
        while (!selected && target == null)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (target.CompareTag("Minion"))
        {
            //Get the minion to be destroyed's power
            int minionPower = target.GetComponent<monsterInfo>().power;
            //Deal its damage to both players
            DealDamageToPlayer(GameState.players[0], minionPower);
            DealDamageToPlayer(GameState.players[1], minionPower);
            //THIS IS WRONG: should add to parent player's graveyard
            GameState.getActivePlayer().graveyard.Add(target.GetComponent<monsterInfo>().card);

            GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];

            //This part shows the top graveyard card
            topGraveyardCard.SetActive(true);

            topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;

            topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
            topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);

            topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);

            //Remove the target from the board minions
            target.GetComponent<monsterInfo>().parentPlayer.boardMinions.Remove(target);
            Destroy(target);
            Destroy(target.GetComponent<monsterInfo>().powerTooltipOfMonster);

            //then we get the coordinates of the monster and set its square to free...
            GameState.boardTable[target.GetComponent<monsterInfo>().coords[0].First,
                target.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().isFree = true;
            //then we destroy the card

            //GameState.getActivePlayer().handCards.RemoveAt(GameState.getActivePlayer().selectedCardIndex);

            GameState.getActivePlayer().cardSelected = false;
            //Reorder cards here until we do better refactoring
            GameState.getActivePlayer().reorderHandCards();
            cardEventHandler.onSummon -= pyra;
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    //                    selected = true; // stop coroutine
                    cardEventHandler.onSummon -= pyra;
                    break;
                }
            }
        }
    }


    /* ************************************************************************************************************************************************************************************* */
    /* ************************************************************************************************************************************************************************************* */
    /* ************************************************************************************************************************************************************************************* */
    /* *******************************************************************GENERAL GAME MECHANICS AND HELPER FUNCTIONS*********************************************************************** */
    /* ************************************************************************************************************************************************************************************* */
    /* ************************************************************************************************************************************************************************************* */
    /* ************************************************************************************************************************************************************************************* */

    /*
     * Function used to handle the deal damage on player effect
     */
    public static void DealDamageToPlayer(Player targetPlayer, int amountOfDamage)
    {
        targetPlayer.playerHealth -= amountOfDamage;
        targetPlayer.healthGO.GetComponentInChildren<Text>().text = targetPlayer.playerHealth.ToString();
    }

    public IEnumerator waitAndDestroyTarget()
    {
        while (!selected)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (target.CompareTag("Minion"))
        {
            GameState.getActivePlayer().boardMinions.Remove(target);
            GameState.getActivePlayer().graveyard.Add(target.GetComponent<monsterInfo>().card);
            
            GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];

            topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;

            topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
            topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);

            topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);

            topGraveyardCard.SetActive(true);

            GameState.getActivePlayer().handCards.Remove(GameState.getActivePlayer().selectedCard);
            GameState.getActivePlayer().graveyard.Add(GameState.getActivePlayer().selectedCard);
            Destroy(GameState.getActivePlayer().selectedCard);


            Debug.Log("HAND CARDS SIZE: " + GameState.getActivePlayer().handCards.Count);
            target.GetComponent<monsterInfo>().parentPlayer.boardMinions.Remove(target);
            Destroy(target);
            Destroy(target.GetComponent<monsterInfo>().powerTooltipOfMonster);

            //then we get the coordinates of the monster and set its square to free...
            GameState.boardTable[target.GetComponent<monsterInfo>().coords[0].First, target.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().isFree = true;
            //Reorder cards here until we do better refactoring
            GameState.getActivePlayer().reorderHandCards();
            cardEventHandler.onSummon -= fireball;
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    //selected = true; // stop coroutine
                    cardEventHandler.onSummon -= fireball;
                    break;
                }
            }
        }
    }

    public IEnumerator destroyTargetIfAdjacent(IEnumerable<GameObject> adjacentMinions){
        while (!selected && target == null)
        {
            yield return new WaitForSeconds(0.2f);
        }
        if (target.CompareTag("Minion") && adjacentMinions.Contains(target))
        {
            //Remove the minion from its player's board minions
            target.GetComponent<monsterInfo>().parentPlayer.boardMinions.Remove(target);
            //Add the minion to its player's graveyard cards.
            target.GetComponent<monsterInfo>().parentPlayer.graveyard.Add(target.GetComponent<monsterInfo>().card);
            //Make the minion's node free.
            GameState.boardTable[target.GetComponent<monsterInfo>().coords[0].First, target.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().isFree = true;
            GameObject topGraveyardCard = GameState.getActivePlayer().graveyard[GameState.getActivePlayer().graveyard.Count - 1];
            Destroy(target);
            Destroy(target.GetComponent<monsterInfo>().powerTooltipOfMonster);
            topGraveyardCard.GetComponent<Card>().pointerEventsEnabled = false;
            topGraveyardCard.transform.SetParent(GameObject.Find("graveyard").transform, false);
            topGraveyardCard.transform.localPosition = new Vector3(0, 0, 0);
            topGraveyardCard.transform.localScale = new Vector3(0.52f, 0.5f, 0.75f);
            topGraveyardCard.SetActive(true);
            cardEventHandler.onSummon -= fireElemental;
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    //selected = true; // stop coroutine
                    cardEventHandler.onSummon -= fireElemental;
                    break;
                }
            }
        }
        target = null;
    }

    public static List<GameObject> getAdjacentMinions(GameObject[,] arr, int row, int column){
        int rows = arr.GetLength(0);
        int columns = arr.GetLength(1);
        Debug.Log(rows+columns);
        List<GameObject> adjacentMinions = new List<GameObject>();
        for (int j = row - 1; j <= row + 1; j++)
            for (int i = column - 1; i <= column + 1; i++)
                if (i >= 0 && j >= 0 && i < columns && j < rows && !arr[j,i].GetComponent<nodeInfo>().isFree){
                    Debug.Log(j + i);
                    adjacentMinions.Add(arr[j,i].GetComponent<nodeInfo>().monsterOnNode);
                }
        return adjacentMinions;
    }

    public IEnumerator copyTarget(){    
        while (!selected && target == null)
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (target.CompareTag("Minion"))
        {
            int currentXpos = GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1].GetComponent<monsterInfo>().coords[0].First;
            int currentYpos = GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1].GetComponent<monsterInfo>().coords[0].Second;
            
            Vector3 currentWorldPos = GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1].transform.position;

            Destroy(GameState.getActivePlayer().boardMinions[GameState.getActivePlayer().boardMinions.Count - 1]);
            GameState.getActivePlayer().boardMinions.RemoveAt(GameState.getActivePlayer().boardMinions.Count - 1);

            GameObject copiedMinion = Instantiate(target, currentWorldPos, Quaternion.identity);

            List<Pair<int, int>> summonNodes = new List<Pair<int, int>>() { new Pair<int, int>(currentXpos, currentYpos) };

            copiedMinion.GetComponent<monsterInfo>().setPosition(summonNodes);
            copiedMinion.GetComponent<monsterInfo>().parentPlayer = GameState.getActivePlayer();
            copiedMinion.GetComponent<monsterInfo>().playedturn = GameState.turn;
            copiedMinion.GetComponent<monsterInfo>().movable = false;
            copiedMinion.GetComponent<monsterInfo>().clickable = false;

            GameState.getActivePlayer().boardMinions.Add(copiedMinion);
            GameState.boardTable[currentXpos, currentYpos].GetComponent<nodeInfo>().powerTooltip.GetComponentInChildren<Text>().text = copiedMinion.GetComponent<monsterInfo>().power.ToString();


            cardEventHandler.onSummon -= effigyOfFlames;
        }
        else if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("Card"))
                {
                    //selected = true; // stop coroutine
                    cardEventHandler.onSummon -= effigyOfFlames;
                    break;
                }
            }
        }
        target = null;
    }

    /*
     * Function used to handle the freeze effect
     */
    public static void Freeze(GameObject target)
    {
        //TODO: Freeze
    }

    /*
     * Function that returns a List with all GameObjects in a scene.
     */
    private static List<GameObject> GetAllObjectsInScene()
    {
        List<GameObject> objectsInScene = new List<GameObject>();

        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.hideFlags != HideFlags.None)
                continue;

            if (PrefabUtility.GetPrefabType(go) == PrefabType.Prefab || PrefabUtility.GetPrefabType(go) == PrefabType.ModelPrefab)
                continue;

            objectsInScene.Add(go);
        }
        return objectsInScene;
    }
    
    private IEnumerator waitForUserToSelectTarget()
    {
        selected = false;
        disableOtherInput = true;

        yield return new WaitForSeconds(1);

        while (!selected)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_PointerEventData = new PointerEventData(m_EventSystem) {position = Input.mousePosition};
                List<RaycastResult> results = new List<RaycastResult>();

                m_Raycaster.Raycast(m_PointerEventData, results);
                this.results = results;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    target = hit.collider.gameObject;
                    selected = true;
                }
            }

            yield return null;
        }

        disableOtherInput = false;
//        yield return target;
    }

    private void updatePowerTooltip(GameObject target){
        int x = target.GetComponent<monsterInfo>().coords[0].First;
        int y = target.GetComponent<monsterInfo>().coords[0].Second;
        //THIS IS WRONG, POWER TOOLTIP GETS DESTROYED AND WE CANT ACCESS IT AFTER A MINION MOVES
        if(GameState.boardTable[x,y].GetComponent<nodeInfo>().powerTooltip != null)
            GameState.boardTable[x,y].GetComponent<nodeInfo>().powerTooltip.GetComponentInChildren<Text>().text = target.GetComponent<monsterInfo>().power.ToString();
    }

}