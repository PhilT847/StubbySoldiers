using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mageScript : MonoBehaviour
{
    public GameObject projectile;
    public int projectileForceBonus; //added range
    public string spellSound;

    private bool canCast; //if there's an enemy in range, can cast!

    public float spellCooldown;
    private float cooldownTimer;
    private bool spellReady;
    private float createSpellTimer;

    private float findTargetsTimer; //every 2s, reset to find new targets
    //private List<Transform> enemyTargets;

    public float range;
    public bool isAngled; //priest waves are not angled, as they heal allies. Some attacks are angled however.

    // Start is called before the first frame update
    void Start()
    {
        range *= Random.Range(0.95f, 1.05f); //randomize slightly so mage units don't clump
        cooldownTimer = Time.time + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > findTargetsTimer)
        {
            findTargets();
        }

        if (Time.time > cooldownTimer && Time.time > GetComponent<UnitScript>().nextAttackTime && canCast)
        {
            canCast = false;
            cooldownTimer = Time.time + spellCooldown;
            createSpellTimer = Time.time + 0.5f;
            spellReady = true;
            GetComponent<Animator>().SetTrigger("Cast");

            GetComponent<UnitScript>().waitTime = Time.time + spellCooldown + 0.2f; //wait until you can't cast anymore to move again
        }

        if (Time.time > createSpellTimer && spellReady)
        {
            spellReady = false;

            createSpell();
        }

    }

    void findTargets()
    {
        findTargetsTimer = Time.time + 0.25f;

        CastleScript[] findCastles = FindObjectsOfType<CastleScript>(); //if there's a castle nearby, just target that

        for (int i = 0; i < findCastles.Length; i++)
        {
            if (findCastles[i].team != GetComponent<UnitScript>().team && Mathf.Abs(findCastles[i].gameObject.transform.position.x - transform.position.x) < range) //if there's a castle within 40, break and just target that
            {
                GetComponent<UnitScript>().waitTime = Time.time + spellCooldown + 0.5f;
                canCast = true;
                goto targetEnd;
            }
        }

        UnitScript[] allUnits = FindObjectsOfType<UnitScript>(); //find all units, then pick the ones in range

        List<Transform> enemyTargets = new List<Transform>();

        for (int i = 0; i < allUnits.Length; i++)
        {
            if (Mathf.Abs(allUnits[i].gameObject.transform.position.y - transform.position.y) < 6f && allUnits[i].team != GetComponent<UnitScript>().team) //if in lane and enemy, pick
            {
                enemyTargets.Add(allUnits[i].gameObject.transform);
            }
        }

        if (enemyTargets.Count > 0)
        {
            for (int i = 0; i < enemyTargets.Count; i++)
            {
                if (Mathf.Abs(enemyTargets[i].position.x - transform.position.x) < range)
                {
                    canCast = true;
                    break;
                }
                else
                {
                    canCast = false;
                }
            }
        }
        else
        {
            canCast = false;
        }

        targetEnd: spellCooldown = spellCooldown;
    }

    void createSpell()
    {
        FindObjectOfType<audioManagerScript>().Play(spellSound);

        if(GetComponent<UnitScript>().stunnedTime < Time.time)
        {
            var spell = Instantiate(projectile, new Vector2(transform.position.x + 4f * GetComponent<UnitScript>().flipMult, transform.position.y + 5f), Quaternion.identity);
            spell.GetComponent<projectileScript>().owner = GetComponent<UnitScript>();

            if (GetComponent<UnitScript>().flipMult < 0f)
            {
                spell.transform.Rotate(0f, -180f, 0f);
            }

            if (GetComponent<UnitScript>().damageMod > 0) //add damageMod to spells
            {
                spell.GetComponent<projectileScript>().minDamage += GetComponent<UnitScript>().damageMod;
                spell.GetComponent<projectileScript>().maxDamage += GetComponent<UnitScript>().damageMod;
            }

            spell.GetComponent<projectileScript>().affectedTeam = GetComponent<UnitScript>().enemyTeam;

            if (isAngled)
            {
                spell.GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * GetComponent<UnitScript>().flipMult, 0.2f) * (650f + projectileForceBonus));
                spell.GetComponent<Rigidbody2D>().AddTorque(-45f * GetComponent<UnitScript>().flipMult);
            }
            else //reduce gravity and shoot forwards
            {
                spell.GetComponent<Rigidbody2D>().gravityScale = 0.25f;
                spell.GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * GetComponent<UnitScript>().flipMult, 0f) * (800f + projectileForceBonus));
            }
        }

        findTargets();
    }
}
