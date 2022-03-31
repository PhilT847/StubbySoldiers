using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsScreen : MonoBehaviour
{
    public GameObject finishPanel;
    public GameObject shroudPanel;
    public TextMeshProUGUI finishText;
    public TextMeshProUGUI goldText;

    private bool gaveGold; //so gold is only awarded once

    public void createResult(bool youWon)
    {
        finishPanel.SetActive(true);
        shroudPanel.SetActive(true);

        if (!gaveGold)
        {
            gaveGold = true;

            if (youWon)
            {
                finishText.SetText("You won!");
                goldText.SetText("+ 100");
                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 100);
                PlayerPrefs.Save();
            }
            else
            {
                finishText.SetText("Enemy won!");
                goldText.SetText("+ 25");
                PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + 25);
                PlayerPrefs.Save();
            }
        }
    }
}
