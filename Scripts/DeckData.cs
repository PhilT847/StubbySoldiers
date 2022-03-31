using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckData
{
    public string deckCode;

    public DeckData(Deck playerDeck)
    {
        /*if (playerDeck.Cards.Count > 0)
        {
            deckCode = "";

            deckCode += "1";//GameObject.FindGameObjectWithTag("SavedDeck").GetComponent<Deck>().colorID.ToString();

            for (int i = 0; i < playerDeck.Cards.Count; i++)
            {
                deckCode += playerDeck.Cards[i].nameCode; //adds each letter to the string
            }

            Debug.Log(deckCode);
        }*/

        deckCode = "";

        deckCode += GameObject.FindGameObjectWithTag("SavedDeck").GetComponent<Deck>().colorID;

        for (int i = 0; i < 12; i++) // i < playerDeck.Cards.Count
        {
            deckCode += playerDeck.Cards[i].nameCode; //adds each letter to the string
        }

        //string moneyString = GameObject.FindGameObjectWithTag("SavedDeck").GetComponent<Deck>().gold.ToString(); //gold count

        /*
        string moneyString = string.Format("{0}", GameObject.FindGameObjectWithTag("SavedDeck").GetComponent<Deck>().gold);
        
        for(int i = 0; i < moneyString.Length; i++)
        {
            string sub = moneyString.Substring(i, 1);

            if(sub == "0")
            {
                deckCode += "o";
            }
            else if(sub == "1")
            {
                deckCode += "a";
            }
            else if (sub == "2")
            {
                deckCode += "b";
            }
            else if (sub == "3")
            {
                deckCode += "c";
            }
            else if (sub == "4")
            {
                deckCode += "d";
            }
            else if (sub == "5")
            {
                deckCode += "e";
            }
            else if (sub == "6")
            {
                deckCode += "f";
            }
            else if (sub == "7")
            {
                deckCode += "g";
            }
            else if (sub == "8")
            {
                deckCode += "h";
            }
            else if (sub == "9")
            {
                deckCode += "i";
            }

        }
        */

        Debug.Log(deckCode);
    }

}
