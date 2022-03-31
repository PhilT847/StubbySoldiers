using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    public int minDamage;
    public int maxDamage;
    public string impactNoise;
    public float stunTime;
    public bool ignoresDef;
    public bool slowsEnemies;
    public bool healsAllies;

    private LayerMask findEnemies;

    [HideInInspector]
    public Team affectedTeam;
    [HideInInspector]
    public UnitScript owner;

    public bool explodes;

    public float explosionRadius;

    private bool hasAttacked;

    private float originalY; //allows for high projectiles that don't hit the ground of the other lane

    // Start is called before the first frame update
    void Start()
    {
        findEnemies = LayerMask.GetMask("Character");
        Destroy(gameObject, 5f);
        originalY = transform.position.y;
    }

    void OnTriggerEnter2D(Collider2D collision) //only collides if its y position isn't too much higher than original
    {
        if (Mathf.Abs(transform.position.y - originalY) < 10f || collision.gameObject.CompareTag("Fortress") || collision.gameObject.CompareTag("Ground"))
        {
            if (healsAllies && collision.gameObject.CompareTag("Character") && collision.gameObject.GetComponent<UnitScript>().team != affectedTeam) //if not on affected team (ally), then heal
            {
                if (collision.gameObject.GetComponent<UnitScript>().currentHP < collision.gameObject.GetComponent<UnitScript>().maxHP)
                {
                    collision.gameObject.GetComponent<UnitScript>().heal(minDamage, true); //heal and play heal noise
                }
            }

            if (collision.gameObject.CompareTag("Character") && !hasAttacked && collision.gameObject.GetComponent<UnitScript>().team == affectedTeam)
            {
                FindObjectOfType<audioManagerScript>().Play(impactNoise);

                hasAttacked = true;

                GetComponent<ParticleSystem>().Play(false);

                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;

                Destroy(gameObject, 2f);

                if (!explodes)
                {
                    collision.gameObject.GetComponent<UnitScript>().takeDamage(Random.Range(minDamage, maxDamage + 1), stunTime, ignoresDef);

                    if (slowsEnemies)
                    {
                        collision.gameObject.GetComponent<UnitScript>().slowedTime = Time.time + 3f;
                    }
                }
                else
                {
                    Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(transform.position, new Vector2(explosionRadius, explosionRadius), 0, findEnemies);

                    for (int i = 0; i < enemiesToHit.Length; i++)
                    {
                        if (enemiesToHit[i].gameObject.CompareTag("Character") && enemiesToHit[i].gameObject.GetComponentInParent<UnitScript>().team == affectedTeam) //if on enemy team, deal damage/slow
                        {
                            enemiesToHit[i].gameObject.GetComponentInParent<UnitScript>().takeDamage(Random.Range(minDamage, maxDamage + 1), stunTime, ignoresDef);

                            if (slowsEnemies)
                            {
                                enemiesToHit[i].GetComponentInParent<UnitScript>().slowedTime = Time.time + 3f;
                            }
                        }
                    }
                }

            }

            else if (collision.gameObject.CompareTag("Fortress") && collision.gameObject.GetComponentInParent<CastleScript>().team == affectedTeam) //if not character... proj's don't collide
            {
                FindObjectOfType<audioManagerScript>().Play(impactNoise);

                hasAttacked = true;

                collision.gameObject.GetComponentInParent<CastleScript>().takeCastleDamage(Random.Range(minDamage, maxDamage + 1));

                GetComponent<ParticleSystem>().Play(false);

                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;

                Destroy(gameObject, 2f);
            }

            else if (collision.gameObject.CompareTag("Ground")) //if not character... proj's don't collide
            {
                FindObjectOfType<audioManagerScript>().Play(impactNoise);

                hasAttacked = true;

                if (explodes)
                {
                    Collider2D[] enemiesToHit = Physics2D.OverlapBoxAll(transform.position, new Vector2(explosionRadius, explosionRadius), 0, findEnemies);

                    for (int i = 0; i < enemiesToHit.Length; i++)
                    {
                        if (enemiesToHit[i].gameObject.CompareTag("Character") && enemiesToHit[i].GetComponentInParent<UnitScript>().team == affectedTeam) //if on enemy team, deal damage/slow
                        {
                            enemiesToHit[i].GetComponentInParent<UnitScript>().takeDamage(Random.Range(minDamage, maxDamage + 1), stunTime, ignoresDef);

                            if (slowsEnemies)
                            {
                                enemiesToHit[i].GetComponentInParent<UnitScript>().slowedTime = Time.time + 3f;
                            }
                        }
                    }
                }

                GetComponent<ParticleSystem>().Play(false);

                SpriteRenderer[] sprites = GetComponents<SpriteRenderer>();

                for (int i = 0; i < sprites.Length; i++)
                {
                    sprites[i].enabled = false;
                }

                //GetComponents<SpriteRenderer>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;

                Destroy(gameObject, 2f);
            }
        }
        
    }

}
