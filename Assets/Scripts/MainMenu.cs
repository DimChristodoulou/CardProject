using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Use this for initialization
    public void Play()
    {
        SceneManager.LoadScene("mainscene");
    }

    public void Deckbuilder()
    {
        SceneManager.LoadScene("deckbuilder");
    }
}
