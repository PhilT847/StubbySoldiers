using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsButton : MonoBehaviour
{
    public Deck game;

    [HideInInspector]
    public bool isClickable;

    void Update()
    {
        if(PlayerPrefs.GetInt("AdFree") == 0)
        {
            if (PlayerPrefs.GetFloat("TempAd") < 1 && !isClickable)
            {
                isClickable = true;
                GetComponent<Button>().interactable = true;
            }
            else if (PlayerPrefs.GetFloat("TempAd") > 0)
            {
                isClickable = false;
                GetComponent<Button>().interactable = false;
            }
        }
        else //if you bought the ad-free version, you can no longer view free ads.
        {
            GetComponent<Button>().interactable = false;
        }

    }
}
