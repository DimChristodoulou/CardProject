using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonparse : MonoBehaviour {

    string path;
    string jsonString;
    public static CardTemplate[] cardTemplates;
    public static int[] cardids;

    // Use this for initialization
    void Start () {
        path = Application.streamingAssetsPath + "/cards.json";
        jsonString = File.ReadAllText(path);
        cardTemplates = JsonHelper.getJsonArray<CardTemplate>(jsonString);
        cardids = new int[cardTemplates.Length];
        for (int i = 0; i < cardTemplates.Length; i++)
            cardids[i] = i+1;
    }

}

public class JsonHelper
{
    public static CardTemplate[] getJsonArray<CardTemplate>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<CardTemplate> wrapper = JsonUtility.FromJson<Wrapper<CardTemplate>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<CardTemplate>
    {
        public CardTemplate[] array;
    }
}

