using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CastleScript : MonoBehaviour
{
    public Team team;
    private int castleHealth;

    public bool testRoom;

    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        castleHealth = 250;
        healthBar.maxValue = castleHealth;

        healthBar.value = castleHealth;
    }

    public void takeCastleDamage(int dmg) //take damage, make chips fly off, sound, etc
    {
        castleHealth -= dmg;

        if(castleHealth < 1)
        {
            castleHealth = 0;

            if (!testRoom)
            {
                if (team.teamID == 0) //enemy wins
                {
                    FindObjectOfType<ResultsScreen>().createResult(false);
                }
                else
                {
                    FindObjectOfType<ResultsScreen>().createResult(true);
                    FindObjectOfType<ResultsScreen>().gameObject.GetComponentInChildren<ParticleSystem>().Play();
                }
            }
            else
            {
                castleHealth = 250;
            }
        }

        healthBar.value = castleHealth;

        //float ratio = ((float)castleHealth / 100) * 205; // 255 to 0

        //healthPiece.color = new Color32((byte)(50 + (255 - ratio)), (byte)(50 + ratio), 50, 255);
    }
}
