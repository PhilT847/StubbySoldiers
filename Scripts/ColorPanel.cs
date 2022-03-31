using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPanel : MonoBehaviour
{
    public Button[] buttons;

    public void setButton()
    {
        string s = FindObjectOfType<Deck>().colorID;

        for(int m = 0; m < buttons.Length; m++) //set all interactable except selected color false
        {
            buttons[m].interactable = true;
        }

        if (s == "W")
        {
            buttons[0].interactable = false;
        }
        else if (s == "R")
        {
            buttons[1].interactable = false;
        }
        else if (s == "B")
        {
            buttons[2].interactable = false;
        }
        else if (s == "Y")
        {
            buttons[3].interactable = false;
        }
        else if (s == "G")
        {
            buttons[4].interactable = false;
        }
        else //purple
        {
            buttons[5].interactable = false;
        }

    }
}
