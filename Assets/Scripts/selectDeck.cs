using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class selectDeck : MonoBehaviour {

    private string path = "Assets/Resources/playerDecks.txt";

    List<int> deckById = new List<int>();
    List<List<string>> deckCodeList = new List<List<string>>();
    List<string> deckCodeNames = new List<string>();

    // Use this for initialization
    void Start () {
        GameObject mainui = GameObject.Find("Main UI");
        StreamReader reader = new StreamReader(path);
        
        int i = 0;
        while (reader.Peek() >= 0) {
            string deckCode = reader.ReadLine();
            deckCodeList.Add(new List<string>());
            deckCodeList[i].AddRange(deckCode.Split('☼')[1].Split(' '));
            deckCodeNames.Add(deckCode.Split('☼')[0]);
            foreach(string s in deckCode.Split('☼')[1].Split(' ')){
                Debug.Log(s);
            }
            int temp = i;
            GameObject deckButton = (GameObject)Instantiate(Resources.Load("deckButton"));
            deckButton.GetComponentInChildren<Text>().text = deckCodeNames[i];
            deckButton.transform.SetParent(mainui.transform);
            deckButton.transform.localPosition = new Vector3(-280 + 75*i, 140, 0);
            deckButton.AddComponent<Button>();
            deckButton.GetComponent<Button>().onClick.AddListener(() => { selectAndStart(temp); });

            deckButton.AddComponent<Text>();
            //deckButton.GetComponent<Text>().text = "Deck " + temp;

            i++;
        }

        reader.Close();
	}

    public void selectAndStart(int i)
    {
        for(int j=0; j<deckCodeList[i].Count - 1;j++)
        {
            Debug.Log(deckCodeList[i][j]);
            deckById.Add(int.Parse(deckCodeList[i][j]));
        }
        GameState.deck = deckById;
        SceneManager.LoadScene("mainscene");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
