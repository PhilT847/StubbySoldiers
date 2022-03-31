using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    public bool saveThisDeck;

    public Deck playerDeck;
    public Button exitBuilderButton;
    public string sceneMusic;

    public AudioMixer mixer;

    public CardScript[] codedCards;

    public float adFreeTime; //30 minutes (1800s) of ad-free play

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Data path: " + Application.persistentDataPath);

        checkForKeys(); //if keys do not exist, create empty (false) keys

        FindObjectOfType<audioManagerScript>().Play(sceneMusic);
        playerDeck = GameObject.FindGameObjectWithTag("SavedDeck").GetComponent<Deck>();

        setVolume(playerDeck.volumeLevel);

        if (saveThisDeck)
        {
            LoadPlayer();

            if (playerDeck.gameObject.scene.buildIndex != -1) //if not dontDestroyOnLoad
            {
                DontDestroyOnLoad(playerDeck.gameObject);
            }
            //DontDestroyOnLoad(playerDeck.gameObject);
        }
        else //in non-menu games, set color
        {
            Team[] teams = FindObjectsOfType<Team>();
            for(int i = 0; i < teams.Length; i++)
            {
                if(teams[i].teamID == 0)
                {
                    teams[i].teamColor = playerDeck.color;
                }
            }
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //REMOVE LATER
        {
            PlayerPrefs.SetInt("Gold", 500);
            PlayerPrefs.SetInt("AdFree", 0);
            PlayerPrefs.SetInt("BoughtGold", 0);
            PlayerPrefs.SetInt("UnlockedRedBlue", 0);
            PlayerPrefs.SetInt("UnlockedGreenYellowPurple", 0);
            PlayerPrefs.SetInt("UnlockedDecay", 0);
            PlayerPrefs.SetInt("UnlockedTempest", 0);
            PlayerPrefs.SetInt("UnlockedPaladin", 0);
            PlayerPrefs.SetInt("UnlockedDwarfKing", 0);
            PlayerPrefs.SetInt("UnlockedEldritch", 0);
            PlayerPrefs.SetInt("UnlockedCannon", 0);

            playerDeck.gold = 500;

            playerDeck.colorID = "W";
            playerDeck.Cards[0] = convertLetterToCard("P");
            playerDeck.Cards[1] = convertLetterToCard("H");
            playerDeck.Cards[2] = convertLetterToCard("I");
            playerDeck.Cards[3] = convertLetterToCard("U");
            playerDeck.Cards[4] = convertLetterToCard("A");
            playerDeck.Cards[5] = convertLetterToCard("O");
            playerDeck.Cards[6] = convertLetterToCard("K");
            playerDeck.Cards[7] = convertLetterToCard("D");
            playerDeck.Cards[8] = convertLetterToCard("R");
            playerDeck.Cards[9] = convertLetterToCard("Y");
            playerDeck.Cards[10] = convertLetterToCard("W");
            playerDeck.Cards[11] = convertLetterToCard("G");

            setDeck(); //sets the new, resetted deck

            GameObject.FindGameObjectWithTag("GoldCounter").GetComponent<TextMeshProUGUI>().SetText("{0}", playerDeck.gold); //set text

            GameObject[] locks = GameObject.FindGameObjectsWithTag("Lock");

            foreach(GameObject g in locks)
            {
                g.SetActive(true);
            }
        }

        if(PlayerPrefs.GetFloat("TempAd") > 0)
        {
            PlayerPrefs.SetFloat("TempAd", PlayerPrefs.GetFloat("TempAd") - Time.deltaTime);
        }
    }

    public void checkForKeys()
    {
        if (!PlayerPrefs.HasKey("Gold")) //this means there are no keys yet. Also set a preset Deck for a first-time player. Code WPHIUAOKDRYWG
        {
            PlayerPrefs.SetInt("Gold", 0);

            playerDeck.colorID = "W";
            playerDeck.Cards[0] = convertLetterToCard("P");
            playerDeck.Cards[1] = convertLetterToCard("H");
            playerDeck.Cards[2] = convertLetterToCard("I");
            playerDeck.Cards[3] = convertLetterToCard("U");
            playerDeck.Cards[4] = convertLetterToCard("A");
            playerDeck.Cards[5] = convertLetterToCard("O");
            playerDeck.Cards[6] = convertLetterToCard("K");
            playerDeck.Cards[7] = convertLetterToCard("D");
            playerDeck.Cards[8] = convertLetterToCard("R");
            playerDeck.Cards[9] = convertLetterToCard("Y");
            playerDeck.Cards[10] = convertLetterToCard("W");
            playerDeck.Cards[11] = convertLetterToCard("G");
        }
        if (!PlayerPrefs.HasKey("AdFree"))
        {
            PlayerPrefs.SetInt("AdFree", 0);
        }
        if (!PlayerPrefs.HasKey("BoughtGold"))
        {
            PlayerPrefs.SetInt("BoughtGold", 0);
        }
        if (!PlayerPrefs.HasKey("UnlockedRedBlue"))
        {
            PlayerPrefs.SetInt("UnlockedRedBlue", 0);
        }
        if (!PlayerPrefs.HasKey("UnlockedGreenYellowPurple"))
        {
            PlayerPrefs.SetInt("UnlockedGreenYellowPurple", 0);
        }
        if (!PlayerPrefs.HasKey("UnlockedDecay"))
        {
            PlayerPrefs.SetInt("UnlockedDecay", 0);
        }
        if (!PlayerPrefs.HasKey("UnlockedTempest"))
        {
            PlayerPrefs.SetInt("UnlockedTempest", 0);
        }
        if (!PlayerPrefs.HasKey("UnlockedPaladin"))
        {
            PlayerPrefs.SetInt("UnlockedPaladin", 0);
        }
        if (!PlayerPrefs.HasKey("UnlockedDwarfKing"))
        {
            PlayerPrefs.SetInt("UnlockedDwarfKing", 0);
        }
        if (!PlayerPrefs.HasKey("UnlockedEldritch"))
        {
            PlayerPrefs.SetInt("UnlockedEldritch", 0);
        }
        if (!PlayerPrefs.HasKey("UnlockedCannon"))
        {
            PlayerPrefs.SetInt("UnlockedCannon", 0);
        }
    }

    public void setDeck() //sets images to current cards
    {
        for(int i = 0; i < playerDeck.currentDeckButtons.Length; i++)
        {
            if (i < playerDeck.Cards.Count) //if a card, set image. If not, blank.
            {
                playerDeck.currentDeckButtons[i].gameObject.GetComponentInChildren<Button>().enabled = true;

                Sprite s = playerDeck.Cards[i].image;
                playerDeck.currentDeckButtons[i].gameObject.GetComponentInChildren<Image>().sprite = s;
            }
            else
            {
                playerDeck.currentDeckButtons[i].gameObject.GetComponentInChildren<Button>().enabled = false;

                Sprite s = playerDeck.currentDeckButtons[i].gameObject.GetComponent<currentDeckCard>().emptySprite;
                playerDeck.currentDeckButtons[i].gameObject.GetComponentInChildren<Image>().sprite = s;

            }
        }

        if(playerDeck.Cards.Count > 12)
        {
            playerDeck.Cards.RemoveRange(12, playerDeck.Cards.Count - 12);
        }

        if (playerDeck.Cards.Count < 12)
        {
            exitBuilderButton.interactable = false;
        }
        else
        {
            exitBuilderButton.interactable = true;
        }
    }

    public void changeQuality(int qual)
    {
        if (qual == 1)
        {
            QualitySettings.SetQualityLevel(0);
        }
        else
        {
            QualitySettings.SetQualityLevel(1);
        }
    }

    public void setVolume(float level)
    {
        GameObject.FindGameObjectWithTag("SavedDeck").GetComponent<Deck>().volumeLevel = level;
        mixer.SetFloat("MasterVolume", level);
    }
    
    public void SavePlayer()
    {
        SaveSystem.SaveDeck(playerDeck);
    }

    public void LoadPlayer()
    {
        DeckData data = SaveSystem.LoadDeck();

        if (data != null) //make sure there's a saved deck
        {
            Debug.Log("Generating Card Data... " + data.deckCode);

            for (int i = playerDeck.Cards.Count - 1; i > -1; i--) //remove cards for reset
            {
                playerDeck.Cards.Remove(playerDeck.Cards[i]);
            }

            playerDeck.colorID = data.deckCode.Substring(0,1);

            for (int i = 1; i < 13; i++) //turns string code of DeckData into card deck... index 0 is color
            {
                playerDeck.Cards.Add(convertLetterToCard(data.deckCode.Substring(i,1))); //get letter and convert it to card
            }

            //add gold based on code
            /*
            playerDeck.gold = 0;

            
            playerDeck.gold += convertLetterToInteger(data.deckCode.Substring(13, 1)) * 1000;
            playerDeck.gold += convertLetterToInteger(data.deckCode.Substring(14, 1)) * 100;
            playerDeck.gold += convertLetterToInteger(data.deckCode.Substring(15, 1)) * 10;
            playerDeck.gold += convertLetterToInteger(data.deckCode.Substring(16, 1));
            */

            playerDeck.gold = PlayerPrefs.GetInt("Gold");

            if(GameObject.FindGameObjectsWithTag("GoldCounter").Length > 0)
            {
                GameObject.FindGameObjectWithTag("GoldCounter").GetComponent<TextMeshProUGUI>().SetText("{0}", playerDeck.gold); //set text
            }
            
            setDeck();
        }

    }

    public int convertLetterToInteger(string w)
    {
        if (w == "z")
        {
            return 0;
        }
        else if (w == "a")
        {
            return 1;
        }
        else if (w == "b")
        {
            return 2;
        }
        else if (w == "c")
        {
            return 3;
        }
        else if (w == "d")
        {
            return 4;
        }
        else if (w == "e")
        {
            return 5;
        }
        else if (w == "f")
        {
            return 6;
        }
        else if (w == "g")
        {
            return 7;
        }
        else if (w == "h")
        {
            return 8;
        }
        else if (w == "i")
        {
            return 9;
        }

        return 0;
    }
    public CardScript convertLetterToCard(string w)
    {
        if (w == "P")
        {
            return codedCards[0];
        }
        else if (w == "U")
        {
            return codedCards[1];
        }
        else if (w == "K")
        {
            return codedCards[2];
        }
        else if (w == "L")
        {
            return codedCards[3];
        }
        else if (w == "E")
        {
            return codedCards[4];
        }
        else if (w == "A")
        {
            return codedCards[5];
        }
        else if (w == "R")
        {
            return codedCards[6];
        }
        else if (w == "G")
        {
            return codedCards[7];
        }
        else if (w == "W")
        {
            return codedCards[8];
        }
        else if (w == "D")
        {
            return codedCards[9];
        }
        else if (w == "F")
        {
            return codedCards[10];
        }
        else if (w == "N")
        {
            return codedCards[11];
        }
        else if (w == "O")
        {
            return codedCards[12];
        }
        else if (w == "C")
        {
            return codedCards[13];
        }
        else if (w == "I")
        {
            return codedCards[14];
        }
        else if (w == "H")
        {
            return codedCards[15];
        }
        else if (w == "Y")
        {
            return codedCards[16];
        }
        else
        {
            return codedCards[17];
        }
        
    }
}
