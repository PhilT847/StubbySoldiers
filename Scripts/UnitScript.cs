using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    public string name;
    [HideInInspector]
    public Team team;
    [HideInInspector]
    public Team enemyTeam;
    [HideInInspector]
    public float flipMult;

    public SpriteRenderer rankPiece;
    public int maxHP;
    [HideInInspector]
    public int currentHP;
    public int minDamage;
    public int maxDamage;
    [HideInInspector]
    public int damageMod; //affected by buffs
    public float attackSpeed;
    public float attackStunTime; //time that melee stuns... usually 0
    public int defense;
    public float speed;
    private float currentSpeed;

    [HideInInspector]
    public float invulnerabilityTimer;

    public int renderImportance; //how "important" this unit is.

    public BoxCollider2D weapon;

    [HideInInspector]
    public bool canAttack;

    private bool hitFirstTarget;

    public SpriteRenderer stunnedPiece;
    private float stunFlipTime;

    public SpriteRenderer buffDefPiece;
    public SpriteRenderer buffAtkPiece;

    [HideInInspector]
    public float nextAttackTime;

    public bool isCaster; //removes melee attack
    public bool ignoresDefense; //used for magical damage
    public bool leechAttack; //regen 2 HP when attacking
    public bool regenHealth; //regen 1 HP every second
    public bool largeWeapon; //changes sound
    private float regenHealthTimer;

    [HideInInspector]
    public float stunnedTime;
    [HideInInspector]
    public float slowedTime;
    [HideInInspector]
    public float buffDefTime;
    [HideInInspector]
    public float buffAtkTime;
    [HideInInspector]
    public float waitTime; //wait after attacking to walk
    [HideInInspector]
    public float decayTime; //time where they can't be healed

    private bool hasReachedCastle; //if they've reached the castle, X-position is constant.
    private float xReachPosition; //the actual x when reaching the castle

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        currentSpeed = speed;
        enemyTeam = team.enemy;

        if (team.teamID > 0) //number 2 or 3... enemy side
        {
            flipMult = -1f;

            speed *= flipMult;

            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            flipMult = 1f;
        }

        //change melee range slightly to vary army lines
        GetComponentInChildren<CapsuleCollider2D>().size = new Vector2(GetComponentInChildren<CapsuleCollider2D>().size.x * Random.Range(0.95f,1.05f), GetComponentInChildren<CapsuleCollider2D>().size.y);

        SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        int unitRando = Random.Range(0, 1000);

        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder += 1000 * renderImportance + unitRando; //somewhat random, but also based on importance
        }

        //transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(-2f, 2f)); //sort order

        assignColors();
        setHealthColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasReachedCastle)
        {
            transform.position = new Vector2(xReachPosition, transform.position.y);
        }

        if(Time.time > regenHealthTimer && regenHealth) //if regen active, regen 1 HP every 2 seconds.
        {
            heal(1, false);

            regenHealthTimer = Time.time + 1f;
        }

        if(stunnedTime > Time.time)
        {
            stunnedPiece.enabled = true;

            if (stunFlipTime < Time.time)
            {
                stunnedPiece.gameObject.transform.Rotate(0f, 180f, 0f);
                stunFlipTime = Time.time + 0.2f;
            }

            if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("bodyIdle"))
            {
                GetComponent<Animator>().Play("bodyIdle");
            }

            nextAttackTime = Time.time + 0.25f; //prevents attacking directly out of stun
        }
        else
        {
            stunnedPiece.enabled = false;
        }

        if(slowedTime > Time.time)
        {
            currentSpeed = speed * 0.5f;
        }
        else
        {
            currentSpeed = speed;
        }

        if (buffDefTime > Time.time)
        {
            buffDefPiece.enabled = true;
        }
        else
        {
            buffDefPiece.enabled = false;
        }

        if (buffAtkTime > Time.time)
        {
            buffAtkPiece.enabled = true;

            damageMod = 1;
        }
        else
        {
            buffAtkPiece.enabled = false;

            damageMod = 0;
        }

        if (Time.time > nextAttackTime && Time.time > stunnedTime && canAttack && !isCaster)
        {
            melee();
        }

        if (Time.time > stunnedTime && Time.time > waitTime)
        {
            GetComponent<Animator>().SetBool("isWalking", true);

            transform.Translate((currentSpeed / 10) * Time.deltaTime, 0, 0, 0);
        }
        else
        {
            GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    public void melee()
    {
        hitFirstTarget = false;

        GetComponent<Animator>().SetTrigger("Melee");
        FindObjectOfType<audioManagerScript>().Play("SwingSmall");

        canAttack = false; //allows reset to check if still sensing

        waitTime = Time.time + attackSpeed + 0.2f;

        nextAttackTime = Time.time + attackSpeed;
    }

    public void heal(int heal, bool healNoise)
    {
        if(currentHP < maxHP && Time.time > decayTime) //if decayed, no heal.
        {
            if (healNoise)
            {
                FindObjectOfType<audioManagerScript>().Play("HealAlly");
            }
            currentHP += heal;
        }

        if(currentHP > maxHP)
        {
            currentHP = maxHP;
        }

        setHealthColor();
    }

    public void takeDamage(int damage, float addStunTime, bool defIgnored)
    {
        if (Time.time > invulnerabilityTimer)
        {
            if (buffDefTime > Time.time && damage > 0) //if defense is buffed, reduce incoming damage including magic
            {
                damage -= 1;
            }

            if (!defIgnored) //physical
            {
                if (damage - defense > 0) //damage has to outdo defense to deal any damage
                {
                    currentHP -= (damage - defense);
                }
            }
            else //magical
            {
                currentHP -= damage;
            }

            if (addStunTime > 0f && (stunnedTime - Time.time) < addStunTime) //if stun is the longest, apply stun
            {
                stunnedTime = Time.time + addStunTime;
            }

            setHealthColor();

            if (currentHP < 1)
            {
                FindObjectOfType<audioManagerScript>().Play("DeathCharacter");
                Destroy(gameObject);
            }
        }
    }

    public void setHealthColor()
    {
        float current = currentHP;
        float maximum = maxHP;

        float ratio = (current / maximum) * 150f;

        //float ratio = ((float)currentHP / maxHP) * 100f; //100 to 0 //((float)currentHP / maxHP) * 200; // 200 to 0

        rankPiece.color = new Color32((byte)(255 - ratio), (byte)(105 + ratio), 120, 220);
    }

    void assignColors()
    {
        SpriteRenderer[] bodyParts = GetComponentsInChildren<SpriteRenderer>();

        for (int i = 0; i < bodyParts.Length; i++)
        {
            if (bodyParts[i].gameObject.CompareTag("Body"))
            {
                bodyParts[i].color = team.teamColor;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character") && Time.time > nextAttackTime && collision.gameObject.GetComponent<UnitScript>().currentHP > 0 && team != collision.gameObject.GetComponent<UnitScript>().team)
        {
            canAttack = true;
        }

        if (collision.gameObject.CompareTag("Fortress") && Time.time > nextAttackTime && team != collision.gameObject.GetComponentInParent<CastleScript>().team)
        {
            canAttack = true; //if you hit a fortress, stop walking forever.

            if (!hasReachedCastle)
            {
                hasReachedCastle = true;
                xReachPosition = transform.position.x;
            }
        }

        /*if (collision.gameObject.CompareTag("Character") && collision.gameObject.GetComponent<BoxCollider2D>().IsTouching(weapon))
        {
            collision.gameObject.GetComponent<UnitScript>().takeDamage(Random.Range(minDamage, maxDamage + 1), attackStunTime, ignoresDefense);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hitFirstTarget)
        {
            if (collision.gameObject.CompareTag("Fortress") && collision.gameObject.GetComponent<BoxCollider2D>().IsTouching(weapon) && team != collision.gameObject.GetComponentInParent<CastleScript>().team)
            {
                if (!largeWeapon)
                {
                    FindObjectOfType<audioManagerScript>().Play("SwordSmall");
                }
                else
                {
                    FindObjectOfType<audioManagerScript>().Play("SwordLarge");
                }
                hitFirstTarget = true; //disable weapon after hitting one enemy
                collision.gameObject.GetComponentInParent<CastleScript>().takeCastleDamage(Random.Range(minDamage, maxDamage + 1) + damageMod);
            }

            if (collision.gameObject.CompareTag("Character") && collision.gameObject.GetComponent<UnitScript>().invulnerabilityTimer < Time.time && collision.gameObject.GetComponent<BoxCollider2D>().IsTouching(weapon) && team != collision.gameObject.GetComponent<UnitScript>().team)
            {
                hitFirstTarget = true;
                FindObjectOfType<audioManagerScript>().Play("SwordLarge");
                collision.gameObject.GetComponent<UnitScript>().takeDamage(Random.Range(minDamage, maxDamage + 1) + damageMod, attackStunTime, ignoresDefense);

                //stop collision so that units only hit one target
                if (leechAttack)
                {
                    GetComponent<UnitScript>().heal(5, false);
                }
            }
        }
    }
    
}
