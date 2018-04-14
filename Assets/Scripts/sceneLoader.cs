using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{

    // Use this for initialization
    public void Play()
    {
        SceneManager.LoadScene("selectDeck");
    }

    public void Deckbuilder()
    {
        SceneManager.LoadScene("deckbuilder");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("mainmenu");
    }
}
