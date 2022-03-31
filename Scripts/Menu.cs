using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void enterScene(int scene)
    {
        //stop music
        if(FindObjectsOfType<MusicLooper>().Length > 0)
        {
            FindObjectOfType<audioManagerScript>().Stop(FindObjectOfType<MusicLooper>().musicLoop);
        }

        if (!FindObjectOfType<GameController>().saveThisDeck)
        {
            Deck[] decks = FindObjectsOfType<Deck>();

            for(int i = 0; i < decks.Length; i++)
            {
                Destroy(decks[i].gameObject);
            }
        }
        SceneManager.LoadSceneAsync(scene);
    }
}
