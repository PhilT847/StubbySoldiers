using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public string playerCode;
    /*
    public PlayerData() //creates the code that is then turned into binary data... example ABRGDTPKE
    {
        playerCode = "";
        GameController game = GameObject.FindObjectOfType<GameController>();

        if (game.adFree)
        {
            playerCode += "A";
        }
        else
        {
            playerCode += "O";
        }

        if(game.boughtGold)
        {
            playerCode += "B";
        }
        else
        {
            playerCode += "O";
        }

        if (game.hasRedBlue)
        {
            playerCode += "R";
        }
        else
        {
            playerCode += "O";
        }

        if (game.hasGreenYellowPurple)
        {
            playerCode += "G";
        }
        else
        {
            playerCode += "O";
        }

        if (game.unlockedDecay)
        {
            playerCode += "D";
        }
        else
        {
            playerCode += "O";
        }

        if (game.unlockedTempest)
        {
            playerCode += "T";
        }
        else
        {
            playerCode += "O";
        }

        if (game.unlockedPaladin)
        {
            playerCode += "P";
        }
        else
        {
            playerCode += "O";
        }

        if (game.unlockedDwarfKing)
        {
            playerCode += "K";
        }
        else
        {
            playerCode += "O";
        }

        if (game.unlockedEldritch)
        {
            playerCode += "E";
        }
        else
        {
            playerCode += "O";
        }

        Debug.Log("Player Code: " + playerCode);
    }
    */
}
