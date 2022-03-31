using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class Monetization : MonoBehaviour
{
    string iOS_ID = "3843328";
    bool testMode = true; //SET FALSE LATER

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("TempAd")) //only set if there's no key
        {
            PlayerPrefs.SetFloat("TempAd", 0);
        }

        Advertisement.Initialize(iOS_ID, testMode);
    }

    public void DisplayInterstitialAd()
    {
        if (PlayerPrefs.GetInt("AdFree") == 0 && PlayerPrefs.GetFloat("TempAd") < 1) //if AdFree is false and there's no active timer, show ad.
        {
            Advertisement.Show();
        }
    }

    public void DisplayPromotionalAd()
    {
        PlayerPrefs.SetFloat("TempAd", 1800);
        Advertisement.Show();
    }

    public void RemoveAds()
    {
        PlayerPrefs.SetInt("AdFree", 1);
        PlayerPrefs.Save();
    }

    public void BuyGold()
    {
        FindObjectOfType<Deck>().gold += 1000;
        FindObjectOfType<GameController>().SavePlayer(); //saves gold
        FindObjectOfType<GameController>().LoadPlayer(); //loads to update gold
        PlayerPrefs.SetInt("BoughtGold", 1);
        PlayerPrefs.Save();
    }
    
}
