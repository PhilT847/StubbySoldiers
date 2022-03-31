using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class EnemyAI : MonoBehaviour
{
    public Team team;

    public Transform topLaneTargeter;
    public Transform bottomLaneTargeter;
    public Transform allyTargeter;

    public GameObject[] weakSpells;
    public GameObject[] strongSpells;
    public GameObject[] heal;
    public GameObject[] weakUnits;
    public GameObject[] strongUnits;

    private float energy; //added to by specific spells and units... strong is longer than weak

    private float allThingsTimer;

    private float weakSpellTimer;
    private float strongSpellTimer;
    private float supportTimer;
    private float weakUnitTimer;
    private float strongUnitTimer;

    private List<Transform> topUnits;
    private List<Transform> botUnits;

    private List<Transform> allyUnits;

    private bool scanBottomLane;

    private float nextScanTime;

    private float topThreat;
    private float bottomThreat; //0 with no enemies on screen, 1 with many

    // Start is called before the first frame update
    void Start()
    {
        energy = 1f;

        topUnits = new List<Transform>();
        botUnits = new List<Transform>();
        allyUnits = new List<Transform>();

        allThingsTimer = Time.time + 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Advertisement.isShowing)
        {
            if (energy < 10f)
            {
                energy += 0.5f * Time.deltaTime;
            }

            if (Time.time > nextScanTime)
            {
                nextScanTime = Time.time + 1.5f;
                searchLane(scanBottomLane);

                scanBottomLane = !scanBottomLane;
            }

            if (Time.time > allThingsTimer) //if either lane is threatened, act!
            {
                act();
            }

            if (topUnits.Count > 0 && topUnits[0] != null)
            {
                topLaneTargeter.position = topUnits[0].position;
            }
            if (botUnits.Count > 0 && botUnits[0] != null)
            {
                bottomLaneTargeter.position = botUnits[0].position;
            }
            if (allyUnits.Count > 0 && allyUnits[0] != null)
            {
                allyTargeter.position = allyUnits[0].position;
            }
        }

        void act()
        {
            if (energy > 1f && topThreat + bottomThreat < 1.75f)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    topThreat = bottomThreat;
                }
                allThingsTimer = Time.time + 6f;
                energy -= 1f;
                summonUnit(false);
            }
            else if (energy > 2f)
            {
                allThingsTimer = Time.time + 8f;

                if (energy > 2f && (topThreat > 3f || bottomThreat > 3f) && Time.time > strongSpellTimer)
                {
                    strongSpellTimer = Time.time + 12f;
                    energy -= 3f;
                    castSpell(true);
                }

                if (checkHealTargets() == true && energy > 2f && topThreat + bottomThreat > 1f && Time.time > supportTimer) //if there's threat in frontline, heal
                {
                    supportTimer = Time.time + 5f;
                    energy -= 2f;
                    healAlly();
                }

                if (energy > 4f && Time.time > strongUnitTimer)
                {
                    strongUnitTimer = Time.time + 12f;
                    energy -= 4f;
                    summonUnit(true);
                }

                if (topThreat + bottomThreat < 2.2f && energy > 2f)
                {
                    weakUnitTimer = Time.time + Random.Range(3f, 5f);
                    energy -= 2f;
                    summonUnit(false);
                }

                if ((topThreat > 2f || bottomThreat > 2f) && energy > 2f)
                {
                    if (Time.time > strongSpellTimer)
                    {
                        strongSpellTimer = Time.time + 16f;
                        weakSpellTimer = Time.time + 2f;
                        energy -= 3f;
                        castSpell(true);
                    }
                    else if (Time.time > weakSpellTimer)
                    {
                        weakSpellTimer = Time.time + 8f;
                        energy -= 2f;
                        castSpell(false);
                    }
                }

                allThingsTimer += 2f; //add some more time for AI to react
            }
        }
    }

    bool checkHealTargets()
    {
        Collider2D[] unitsToCheck = Physics2D.OverlapBoxAll(transform.position, new Vector2(6f, 2f), 0, LayerMask.GetMask("Character"));

        for (int i = 0; i < unitsToCheck.Length; i++)
        {
            if (unitsToCheck[i].gameObject.CompareTag("Character") && unitsToCheck[i].GetComponentInParent<UnitScript>().team == team) //if on enemy team, deal damage/slow
            {
                return true;
            }
        }

        return false;
    }

    void searchLane(bool isBottom)
    {
        if (isBottom)
        {
            botUnits = new List<Transform>();
            bottomThreat = 0f;
        }
        else
        {
            topUnits = new List<Transform>();
            topThreat = 0.05f; //top lane is preferred slightly, allowing for top > bottom calls to work
        }

        allySearch();

        determineThreat(isBottom);
    }

    void summonUnit(bool isStrong)
    {
        int flipUnit = Random.Range(0, 5); //20% chance to spawn in other lane

        int rando = Random.Range(0, strongUnits.Length);

        if (topThreat > bottomThreat)
        {
            if (flipUnit != 4)
            {
                if (isStrong)
                {
                    var unit = Instantiate(strongUnits[rando], new Vector2(38f, 7.5f), Quaternion.identity);
                    unit.GetComponent<UnitScript>().team = team;
                }
                else
                {
                    var unit = Instantiate(weakUnits[rando], new Vector2(38f, 7.5f), Quaternion.identity);
                    unit.GetComponent<UnitScript>().team = team;
                }
            }
            else
            {
                if (isStrong)
                {
                    var unit = Instantiate(strongUnits[rando], new Vector2(38f, -10.5f), Quaternion.identity);
                    unit.GetComponent<UnitScript>().team = team;
                }
                else
                {
                    var unit = Instantiate(weakUnits[rando], new Vector2(38f, -10.5f), Quaternion.identity);
                    unit.GetComponent<UnitScript>().team = team;
                }
            }

        }
        else
        {
            if (flipUnit != 4)
            {
                if (isStrong)
                {
                    var unit = Instantiate(strongUnits[rando], new Vector2(38f, -10.5f), Quaternion.identity);
                    unit.GetComponent<UnitScript>().team = team;
                }
                else
                {
                    var unit = Instantiate(weakUnits[rando], new Vector2(38f, -10.5f), Quaternion.identity);
                    unit.GetComponent<UnitScript>().team = team;
                }
            }
            else
            {
                if (isStrong)
                {
                    var unit = Instantiate(strongUnits[rando], new Vector2(38f, 7.5f), Quaternion.identity);
                    unit.GetComponent<UnitScript>().team = team;
                }
                else
                {
                    var unit = Instantiate(weakUnits[rando], new Vector2(38f, 7.5f), Quaternion.identity);
                    unit.GetComponent<UnitScript>().team = team;
                }
            }
        }
    }

    void castSpell(bool isStrong)
    {
        int rando = Random.Range(0, strongSpells.Length);

        if(topThreat > bottomThreat)
        {
            if (isStrong)
            {
                var spell = Instantiate(strongSpells[rando], new Vector2(topLaneTargeter.position.x, 7.5f), Quaternion.identity);
                spell.GetComponent<spellScript>().team = team;
            }
            else
            {
                var spell = Instantiate(weakSpells[rando], new Vector2(topLaneTargeter.position.x, 7.5f), Quaternion.identity);
                spell.GetComponent<spellScript>().team = team;
            }
        }
        else
        {
            if (isStrong)
            {
                var spell = Instantiate(strongSpells[rando], new Vector2(bottomLaneTargeter.position.x, -10.5f), Quaternion.identity);
                spell.GetComponent<spellScript>().team = team;
            }
            else
            {
                var spell = Instantiate(weakSpells[rando], new Vector2(bottomLaneTargeter.position.x, -10.5f), Quaternion.identity);
                spell.GetComponent<spellScript>().team = team;
            }
        }
    }

    void healAlly()
    {
        if(allyTargeter.position.y > 0f)
        {
            var support = Instantiate(heal[Random.Range(0, 4)], new Vector2(allyTargeter.position.x, 7.5f), Quaternion.identity);
            support.GetComponent<spellScript>().team = team;
        }
        else
        {
            var support = Instantiate(heal[Random.Range(0, 4)], new Vector2(allyTargeter.position.x, -10.5f), Quaternion.identity);
            support.GetComponent<spellScript>().team = team;
        }
        
    }

    void allySearch() //finds allies on the frontline
    {
        UnitScript[] findAllies = FindObjectsOfType<UnitScript>();

        for (int i = 0; i < findAllies.Length; i++)
        {
            if (findAllies[i].team == team && findAllies[i].gameObject.transform.position.x < 0f)
            {
                allyUnits.Add(findAllies[i].gameObject.transform);
            }
        }
    }

    void determineThreat(bool isBottom)
    {
        UnitScript[] findEnemies = FindObjectsOfType<UnitScript>();
        float boundaries = 5.5f;

        if (isBottom)
        {
            boundaries = -12f;
        }

        for(int i = 0; i < findEnemies.Length; i++)
        {
            if (findEnemies[i].team != team && Mathf.Abs(findEnemies[i].gameObject.transform.position.y - boundaries) < 2f)
            {
                addUnitThreat(findEnemies[i], isBottom);
            }
        }

    }

    void addUnitThreat(UnitScript unit, bool isBottom)
    {
        if (isBottom)
        {
            botUnits.Add(unit.gameObject.transform);
            bottomThreat += 0.1f * unit.renderImportance; //add threat based on importance
        }
        else
        {
            topUnits.Add(unit.gameObject.transform);
            topThreat += 0.1f * unit.renderImportance;
        }
    }

}
