using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public bool hasInternalDeck;

    public Team team;

    public int gold;

    public Color color;
    public string colorID; //color ID for saving the deck

    public List<CardScript> Cards;

    private CardScript nextCardInDeck; //the next card to be added.

    public CardPosition[] cardPlaces; //where the cards sit.

    public currentDeckCard[] currentDeckButtons;

    public float volumeLevel;

    //Cards sit at the bottom of the screen, with a maximum of 4.

    // Start is called before the first frame update
    void Start()
    {
        if (hasInternalDeck)
        {
            copyDeck();
        }

        setColor(colorID);
        createFirstFive(); //places the first five cards.
    }

    void copyDeck() //adds saved cards into deck
    {
        Deck d = GameObject.FindGameObjectWithTag("SavedDeck").GetComponent<Deck>();

        for(int i = 0; i < 12; i++) // i < d.Cards.Count
        {
            Cards.Add(d.Cards[i]);
        }
    }

    void createFirstFive()
    {
        for(int i = 0; i < cardPlaces.Length; i++)
        {
            int randomCard = Random.Range(0, Cards.Count);

            cardPlaces[i].currentCard = Cards[randomCard];

            cardPlaces[i].SetCard();

            Cards.Remove(Cards[randomCard]); //remove the card added to the deck.
        }

        nextCardInDeck = Cards[Random.Range(0, Cards.Count)]; // set the first replacement. is the +1 correct?
    }

    public void replaceCard(int position) //once the card is placed, replace the card with another from the list randomly.
    {
        Cards.Add(cardPlaces[position].currentCard); //add the used card back into the deck for shuffling.

        cardPlaces[position].currentCard = nextCardInDeck; //the next card in the deck is added to the position

        cardPlaces[position].SetCard();

        Cards.Remove(nextCardInDeck); //remove the card as it's placed into position

        nextCardInDeck = Cards[Random.Range(0, Cards.Count)]; // set the next replacement. is the +1 correct?
    }

    public void clearDeck()
    {
        for(int i = Cards.Count - 1; i > -1; i--)
        {
            Cards.Remove(Cards[i]);
        }

        FindObjectOfType<GameController>().setDeck(); //resets deck images accordingly

        Selector[] selectors = FindObjectsOfType<Selector>(); //reset selectors to 3 of each card

        for (int c = 0; c < selectors.Length; c++)
        {
            selectors[c].totalNumber = 3;
            selectors[c].numberText.SetText("3");
        }
    }

    public void addCard(Selector s) //add card from
    {
        if (!s.requiresUnlock || (s.requiresUnlock && PlayerPrefs.GetInt(s.unlockTitle) == 1)) //if it doesn't need an unlock or does and is unlocked, add!
        {
            if (Cards.Count < 12 && s.totalNumber > 0)
            {
                FindObjectOfType<audioManagerScript>().Play("Pop");

                s.totalNumber--;
                s.numberText.SetText("{0}", s.totalNumber);
                Cards.Add(s.card);
                FindObjectOfType<GameController>().setDeck(); //resets deck images accordingly
            }
        }
    }

    public void removeCard(int i)
    {
        if (FindObjectOfType<GameController>().playerDeck.Cards.Count > i)
        {
            Selector[] selectors = FindObjectsOfType<Selector>();

            for (int c = 0; c < selectors.Length; c++)
            {
                if (selectors[c].card == FindObjectOfType<GameController>().playerDeck.Cards[i] && selectors[c].totalNumber < 3)
                {
                    selectors[c].totalNumber++;
                    selectors[c].numberText.SetText("{0}", selectors[c].totalNumber);
                }
            }

            Cards.Remove(FindObjectOfType<GameController>().playerDeck.Cards[i]);

            FindObjectOfType<GameController>().setDeck(); //resets deck images accordingly
        }
    }

    public void setColor(string c)
    {
        colorID = c;

        if (c == "W")
        {
            color = Color.white;
        }
        else if (c == "R") //red
        {
            color = new Color(1f, 170f / 255f, 170f / 255f);
        }
        else if (c == "B") //blue
        {
            color = new Color(165f/255f, 191f/255f, 1f);
        }
        else if (c == "Y") //yellow
        {
            color = new Color(1f, 1f, 182f/255f);
        }
        else if (c == "G") //green
        {
            color = new Color(170f/255f, 1f, 170f / 255f);
        }
        else //purple
        {
            color = new Color(1f, 195f/255f, 1f);
        }
    }
}
