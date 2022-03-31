using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selector : MonoBehaviour
{
    public CardScript card;
    public int totalNumber; //how many can be added to a deck

    public TextMeshProUGUI numberText;

    public bool requiresUnlock;

    public string unlockTitle;

}
