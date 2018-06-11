using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class nodeInfo : MonoBehaviour
{
	//contains info regarding nodes

    public GameObject monsterOnNode;
	//checks if node is free to place unit
	private Renderer mRenderer;
	public int xpos, ypos;
	public bool isFree;
	public Color activeColor = new Color32(0x00, 0x99, 0x00, 0xFF); // RGBA
	public Color startColor = new Color32(0xFF, 0xFF, 0xFF, 0x00); // RGBA
	public Color hoverColor = new Color32(0x2C, 0x2C, 0xFF, 0x8F); //RGBA
	public Color currentColor; //current color, ignoring temporary effects like hovers

	// Use this for initialization
	void Start () {
		mRenderer = this.GetComponentInParent<Renderer>();
		currentColor = startColor;
		mRenderer.material.color = currentColor;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter() {
		colorHovered ();
	}

	void OnMouseExit() {
		colorHovered ();
	}

	void OnMouseDown() {
        Player activePlayer = GameState.players[GameState.activePlayerIndex];
        Debug.Log("ACTIVE PLAYER INDEX: " + GameState.activePlayerIndex);

        GameObject selectedMonster = activePlayer.selected;
        
        if (selectedMonster != null)
        {
            Debug.Log(selectedMonster.name);
            selectedMonster.GetComponent<movement>().moveTo(xpos, ypos);
        }
        else if (activePlayer.cardSelected && activePlayer.availableNodesForSummon != null)
        {
            //Debug.Log("search for node: " + xpos + ", " + ypos);
            if (Contains(activePlayer.availableNodesForSummon, new Pair<int, int>(xpos, ypos)))
            {
                List<Pair<int, int>> summonNodes = new List<Pair<int, int>>() { new Pair<int, int>(xpos, ypos) }; // ONLY for monsters that need one node to spawn
                activePlayer.summonMonster(activePlayer.selectedCard.GetComponent<Card>().cardName.text, summonNodes, GameState.turn);

                GameObject currentMinion = activePlayer.boardMinions[activePlayer.boardMinions.Count - 1];

                // HERE SET DATA OF MINION USING SETDATA FUNCTION
                Card selectedCard = activePlayer.selectedCard.GetComponent<Card>();

                currentMinion.GetComponent<monsterInfo>().setData(jsonparse.cardTemplates[selectedCard.id].card_name,
                                                                  jsonparse.cardTemplates[selectedCard.id].card_actionpoints,
                                                                  jsonparse.cardTemplates[selectedCard.id].card_manacost,
                                                                  jsonparse.cardTemplates[selectedCard.id].card_movement,
                                                                  1,
                                                                  activePlayer, GameState.turn,
                                                                  jsonparse.cardTemplates[selectedCard.id].card_keywords,
                                                                  activePlayer.selectedCard);

                currentMinion.GetComponent<monsterInfo>().setPosition(summonNodes);
                //currentMinion.GetComponent<monsterInfo>().parentPlayer = activePlayer;

                currentMinion.GetComponent<monsterInfo>().parentPlayer.updateClickedItem(null);
                //GameState.setSquares(currentMinion.GetComponent<monsterInfo>().coords, true);
                //Vector3 boardPos = GameState.getPositionRelativeToBoard(newPos);
                //gameObject.transform.position = boardPos;
                //GetComponentInParent<monsterInfo>().setPosition(newPos);
                GameState.setSquares(currentMinion.GetComponent<monsterInfo>().coords, false);
                currentMinion.GetComponent<monsterInfo>().onPostMove();
                monsterOnNode = currentMinion;

                //GameState.boardTable[xpos, ypos].GetComponent<nodeInfo>().switchActiveColor();

                //foreach (GameObject minion in activePlayer.boardMinions)
                //{
                //    if (minion != currentMinion)
                //    // minion.GetComponent<clickableV2>().toggleChange();
                //        GameState.boardTable[minion.GetComponent<monsterInfo>().coords[0].First, minion.GetComponent<monsterInfo>().coords[0].Second].GetComponent<nodeInfo>().switchActiveColor();
                //}


                foreach (Pair<int, int> pair in activePlayer.availableNodesForSummon)
                    GameState.boardTable[pair.First, pair.Second].GetComponent<nodeInfo>().makeInactive();

                activePlayer.availableNodesForSummon = null;
                activePlayer.cardSelected = false;
                //activePlayer.handCards.RemoveAt(activePlayer.selectedCardIndex);
                Debug.Log(activePlayer.handCards);
                cardEventHandler.onMinionSummon(jsonparse.cardTemplates[selectedCard.id].card_name);
            }
        }
        else
            return;

    }

    bool Contains(List<Pair<int,int>> list, Pair<int,int> p)
    {
        foreach (Pair<int,int> pair in list)
        {
            if (pair.First == p.First && pair.Second == p.Second)
                return true;
        }
        return false;
    }


    public void switchHoverColor() {
		if (mRenderer.material.color == hoverColor) {
			mRenderer.material.color = currentColor;
		} else {
			mRenderer.material.color = hoverColor;
		}
	}

	public void switchActiveColor() {
		if (currentColor != activeColor) {
			currentColor = activeColor;
		}
		else {
			currentColor = startColor;
		}
		mRenderer.material.color = currentColor;
	}

    public void makeActive()
    {
        currentColor = activeColor;
        mRenderer.material.color = activeColor;
    }

    public void makeInactive()
    {
        currentColor = startColor;
        mRenderer.material.color = startColor;
    }

    //colors/uncolors the hovered squares for the movement, if the player has a monster selected
    private void colorHovered() {
		GameObject selectedMonster = GameState.players [GameState.activePlayerIndex].selected;
        //Debug.Log("HELLO");
		if (selectedMonster == null && !GameState.players[GameState.activePlayerIndex].cardSelected) {
			return; //no selected monster for movement
		}
        //int monsterSize = (int)Mathf.Sqrt ((float)selectedMonster.GetComponent<monsterInfo> ().coords.Count);\
        //Debug.Log("xpos: " + xpos);
		if (xpos + 1/*monsterSize*/ >= GameState.dimensionX || ypos + 1/*monsterSize*/ >= GameState.dimensionY) {
			return; //out of table placement
		}
        //Debug.Log("monster size: " + 1/*monsterSize*/);
		int i;
		GameState.boardTable [xpos, ypos].GetComponent<nodeInfo> ().switchHoverColor ();
		//for (i = 1; i < 1/*monsterSize*/; i++) {
		//	GameState.boardTable [xpos+i, ypos].GetComponent<nodeInfo> ().switchHoverColor ();
		//	GameState.boardTable [xpos, ypos+i].GetComponent<nodeInfo> ().switchHoverColor ();
		//	GameState.boardTable [xpos+i, ypos+i].GetComponent<nodeInfo> ().switchHoverColor ();
		//}
	}
}
