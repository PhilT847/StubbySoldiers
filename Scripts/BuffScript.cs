using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffScript : MonoBehaviour
{
    public bool addsDefense;
    public bool addsAttack;

    private float buffApplicationTime; //time to check for nearby allies

    // Start is called before the first frame update
    void Start()
    {
        buffApplicationTime = Time.time + 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(buffApplicationTime < Time.time)
        {
            buffApplicationTime = Time.time + 0.5f;

            Collider2D[] allies = Physics2D.OverlapBoxAll(transform.position, new Vector2(25f, 2f), 0f);

            for (int i = 0; i < allies.Length; i++)
            {
                if (allies[i].gameObject.CompareTag("Character") && allies[i].gameObject.GetComponentInParent<UnitScript>().team == GetComponent<UnitScript>().team)
                {
                    if (addsDefense)
                    {
                        allies[i].gameObject.GetComponent<UnitScript>().buffDefTime = Time.time + 0.75f;
                    }
                    if (addsAttack)
                    {
                        allies[i].gameObject.GetComponent<UnitScript>().buffAtkTime = Time.time + 0.75f;
                    }
                    
                }
            }

            //List<UnitScript>
        }
    }
}
