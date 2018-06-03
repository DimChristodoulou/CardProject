using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonparse : MonoBehaviour {

    string path;
    string jsonString;
    public static Card[] cards;
    public static int[] cardids;

    // Use this for initialization
    void Start () {
        path = Application.streamingAssetsPath + "/cards.json";
        jsonString = File.ReadAllText(path);
        cards = JsonHelper.getJsonArray<Card>(jsonString);
        cardids = new int[cards.Length];
        for (int i = 0; i < cards.Length; i++)
            cardids[i] = i;
    }

}

public class JsonHelper
{
    public static Card[] getJsonArray<Card>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<Card> wrapper = JsonUtility.FromJson<Wrapper<Card>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<Card>
    {
        public Card[] array;
    }
}

