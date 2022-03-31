using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public Team team;
    public int preferredTarget;
    public GameObject[] units;

    private float spawnCooldown; //1s between spawns

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void chooseTarget(int num)
    {
        preferredTarget = num;
    }

    public void create() //creates a unit. 
    {
        if(Time.time > spawnCooldown) //if cooldown is good and you have the money, spawn.
        {
            spawnCooldown = Time.time + 0.1f;

            var unit = Instantiate(units[preferredTarget], transform.position, Quaternion.identity);
            unit.GetComponent<UnitScript>().team = team;
        }
    }
}
