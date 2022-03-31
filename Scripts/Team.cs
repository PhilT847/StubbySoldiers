using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int teamID;
    public Color teamColor;

    [HideInInspector]
    public float energy;

    private Slider energySlider;

    public Team enemy;

    // Start is called before the first frame update
    void Start()
    {
        if(teamID == 0 && !FindObjectOfType<GameController>().saveThisDeck)
        {
            teamColor = GameObject.FindGameObjectWithTag("SavedDeck").GetComponent<Deck>().color;
        }

        energySlider = GameObject.FindGameObjectWithTag("Energy").GetComponent<Slider>();//  FindObjectOfType<Slider>();

        energySlider.minValue = 0f;
        energySlider.maxValue = 10f;

        energy = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(energy < 10f)
        {
            energy += 0.5f * Time.deltaTime;

            if (energy > 10f)
            {
                energy = 10f;
            }

            energySlider.value = energy;
        }
   }
}
