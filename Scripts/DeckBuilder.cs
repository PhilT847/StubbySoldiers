using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckBuilder : MonoBehaviour
{
    public Deck internalDeck; //refers to playerDeck in GameController

    public GameObject[] cards;
    public string[] descriptions;

    public TextMeshProUGUI infoText;

    public void generateInfo(int i)
    {
        infoText.SetText(descriptions[i]);
    }

    public void comparePlayerDeck() //removes extra cards so players can't add too many
    {
        int removedCount = 0;

        Deck playerDeck = FindObjectOfType<Deck>(); //find player's deck and compare to selectors

        Selector[] selectors = FindObjectsOfType<Selector>();

        for(int r = 0; r < selectors.Length; r++) //resets cards to 3 before reducing
        {
            selectors[r].totalNumber = 3;
        }

        for(int i = 0; i < playerDeck.Cards.Count; i++)
        {
            for(int j = 0; j < selectors.Length; j++)
            {
                if (playerDeck.Cards[i] == selectors[j].card)
                {
                    removedCount++;
                    selectors[j].totalNumber -= 1; //reduces total count of card if extras exist
                    selectors[j].numberText.SetText("{0}", selectors[j].totalNumber);
                }
            }
        }

        Debug.Log("Removed " + removedCount + " cards");

    }

}
