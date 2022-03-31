﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject cannonBall;
    public GameObject cannon;

    private float timeBeforeInstantiate;
    private float nextAttackTime;

    private bool aiming;
    private LayerMask findEnemies;

    private Transform target;
    private UnitScript[] targets;
    private List<UnitScript> possibleTargets;

    public Transform reticle;
    private Vector2 reticlePosition;

    private float targetSwitch;
    private float castleRange; //45-55... random

    private bool canShoot; //false if enemies are in melee range... attack instead!

    // Start is called before the first frame update
    void Start()
    {
        castleRange = 50f * Random.Range(0.95f, 1.1f);

        findEnemies = LayerMask.GetMask("Character");

        nextAttackTime = Time.time + 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetSwitch < Time.time)
        {
            List<UnitScript> potentialTargets = new List<UnitScript>();

            targetSwitch = Time.time + 4f;

            CastleScript[] findCastles = FindObjectsOfType<CastleScript>(); //if there's a castle nearby, just target that

            for (int i = 0; i < findCastles.Length; i++)
            {
                if (findCastles[i].team != GetComponent<UnitScript>().team && Mathf.Abs(findCastles[i].gameObject.transform.position.x - transform.position.x) < castleRange) //if there's a castle within 40, break and just target that
                {
                    reticle.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    reticlePosition = new Vector2(findCastles[i].gameObject.transform.position.x, findCastles[i].gameObject.transform.position.y - 25f);
                    GetComponent<UnitScript>().waitTime = Time.time + 6f;
                    goto targetEnd;
                }
            }

            targets = FindObjectsOfType<UnitScript>();

            for (int i = 0; i < targets.Length; i++) //if they're an enemy in the same lane, count them
            {
                if (Mathf.Abs(targets[i].gameObject.transform.position.y - transform.position.y) < 6f && Mathf.Abs(targets[i].gameObject.transform.position.x - transform.position.x) < castleRange && targets[i].team != GetComponent<UnitScript>().team)
                {
                    potentialTargets.Add(targets[i]);
                }
            }

            if (potentialTargets.Count > 0)
            {
                reticle.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                reticlePosition = new Vector2(potentialTargets[0].gameObject.transform.position.x, potentialTargets[0].gameObject.transform.position.y - 1.5f);
            }
            else
            {
                reticle.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                nextAttackTime = Time.time + 4f;
            }

        targetEnd: reticle.position = reticlePosition; //if wall found, skips here to skip target finding
        }

        reticle.position = reticlePosition;

        cannon.GetComponent<Animator>().SetBool("isWalking", GetComponent<Animator>().GetBool("isWalking"));

        if (Time.time > nextAttackTime && !GetComponent<UnitScript>().canAttack)
        {
            GetComponent<UnitScript>().waitTime = Time.time + 6.25f;

            aiming = true;

            cannon.GetComponent<Animator>().SetTrigger("Shoot");

            timeBeforeInstantiate = Time.time + 1.1f;

            nextAttackTime = Time.time + 6f;
        }

        if (Time.time > timeBeforeInstantiate && aiming)
        {
            aiming = false;

            FindObjectOfType<audioManagerScript>().Play("CannonBoom");

            shootBall();
        }
    }

    void shootBall()
    {
        float rangeVar = Random.Range(0.9f, 1.1f); //random range variant

        float distanceVar = 0.5f + Mathf.Abs(transform.position.x - reticle.position.x) / 60; //increase force based on distance from enemy... -30 to 30 is usual distance, so up to 60m away, meaning 0.5-1.5x force

        timeBeforeInstantiate = Time.time + 6f;

        var ball = Instantiate(cannonBall, new Vector2(transform.position.x - 2f * GetComponent<UnitScript>().flipMult, transform.position.y + 6f), Quaternion.identity);
        ball.GetComponent<projectileScript>().owner = GetComponent<UnitScript>();

        if (GetComponent<UnitScript>().flipMult < 0f)
        {
            ball.transform.Rotate(0f, -180f, 0f);
        }

        ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * GetComponent<UnitScript>().flipMult, 0.45f) * 800f * rangeVar * distanceVar);
        ball.GetComponent<Rigidbody2D>().AddTorque(-20f * GetComponent<UnitScript>().flipMult);

        if (GetComponent<UnitScript>().damageMod > 0) //add damageMod to rocks
        {
            ball.GetComponent<projectileScript>().minDamage += GetComponent<UnitScript>().damageMod;
            ball.GetComponent<projectileScript>().maxDamage += GetComponent<UnitScript>().damageMod;
        }

        ball.GetComponent<projectileScript>().affectedTeam = GetComponent<UnitScript>().enemyTeam;
    }
}
