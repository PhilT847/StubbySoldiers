using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellScript : MonoBehaviour
{
    [HideInInspector]
    public Team team;

    public int pulses; //how many times it hits.

    public float hitRate; //time between pulses. Waits until first time to do its effect (ex, 0.5s means it appears after 0.5s)

    public int damage; //or healing for heal

    public float spellWidth;

    public string enterSound;
    public string pulseSound;

    public float spellTime; //allows for animations past the final pulse
    private float killTime;

    public bool addsDecay;
    public bool ignoresDefense;
    public bool healsAllies;
    public bool grantsInvulnerable;
    public bool slows;

    private float nextHitTime;
    private SpriteRenderer ring;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<audioManagerScript>().Play(enterSound);

        transform.localScale = new Vector2(0.25f, 1f);
        nextHitTime = Time.time + hitRate;
        ring = GetComponentInChildren<SpriteRenderer>();
        killTime = Time.time + spellTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < 1f) //grow in
        {
            transform.localScale = new Vector2(transform.localScale.x + 3f * Time.deltaTime, 1f);

            if(transform.localScale.x > 1f)
            {
                transform.localScale = new Vector2(1f, 1f);
            }
        }

        if(pulses > 0)
        {
            if(Time.time > nextHitTime)
            {
                nextHitTime = Time.time + hitRate;

                castSpell();
            }
        }
        else if(Time.time > killTime)
        {
            Destroy(gameObject);
        }
    }

    void castSpell()
    {
        FindObjectOfType<audioManagerScript>().Play(pulseSound);
        pulses--;

        Collider2D[] spellTargets = Physics2D.OverlapBoxAll(transform.position, new Vector2(spellWidth, 4f), 0f);

        for(int i = 0; i < spellTargets.Length; i++)
        {
            if (spellTargets[i].CompareTag("Character"))
            {
                if (!healsAllies && spellTargets[i].gameObject.GetComponentInParent<UnitScript>().team != team)
                {
                    spellTargets[i].GetComponentInParent<UnitScript>().takeDamage(damage, 0f, ignoresDefense);

                    if (slows && spellTargets[i].GetComponentInParent<UnitScript>().slowedTime < Time.time + 0.5f)
                    {
                        spellTargets[i].GetComponentInParent<UnitScript>().slowedTime = Time.time + 0.5f;
                    }
                    if (addsDecay && spellTargets[i].GetComponentInParent<UnitScript>().decayTime < Time.time + 0.5f)
                    {
                        spellTargets[i].GetComponentInParent<UnitScript>().decayTime = Time.time + 0.5f;
                    }
                }

                else if (healsAllies && spellTargets[i].gameObject.GetComponentInParent<UnitScript>().team == team)
                {
                    spellTargets[i].GetComponentInParent<UnitScript>().heal(damage, damage>0);

                    if (grantsInvulnerable)
                    {
                        spellTargets[i].GetComponentInParent<UnitScript>().invulnerabilityTimer = Time.time + 0.25f;
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, new Vector2(spellWidth, 4f));
    }
}
