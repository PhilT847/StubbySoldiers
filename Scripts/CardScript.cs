using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public string nameCode; //added to DeckData for loading
    public GameObject unit;
    public float cost;

    public Sprite image;

    public bool isSpell; //makes the area sprite appear to show where a spell will affect enemies/allies.

}
