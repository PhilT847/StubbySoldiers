using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerScript : MonoBehaviour
{
    public Team team;
    private float nextArrowTime;
    private bool canShoot;

    public Transform bow;
    public GameObject arrow;

    private float findTargetsTimer;
    private UnitScript target;

    private Vector3 originalArrowPosition;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        findTargetsTimer = Time.time + 1f;
        originalArrowPosition = arrow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //check if can shoot

        if (Time.time > findTargetsTimer)
        {
            findTargetsTimer = Time.time + 0.25f;

            UnitScript[] allUnits = FindObjectsOfType<UnitScript>(); //find all units, then pick the ones in range

            List<Transform> enemyTargets = new List<Transform>();

            for (int i = 0; i < allUnits.Length; i++)
            {
                if (transform.position.y > allUnits[i].gameObject.transform.position.y && Mathf.Abs(allUnits[i].gameObject.transform.position.y - transform.position.y) < 15f && Mathf.Abs(allUnits[i].gameObject.transform.position.x - transform.position.x) < 35f && allUnits[i].team != team) //if in lane and enemy, pick
                {
                    enemyTargets.Add(allUnits[i].gameObject.transform);
                }
            }

            if (enemyTargets.Count > 0)
            {
                target = enemyTargets[0].GetComponent<UnitScript>();

                FindClosestTarget(enemyTargets);

                canShoot = true;

            }
            else
            {
                target = null;

                canShoot = false;
            }
        }

        if(target != null)
        {
            Quaternion rotation = Quaternion.LookRotation(target.gameObject.transform.position - bow.position, transform.TransformDirection(Vector3.up));

            bow.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        }

        //bow.LookAt(target.transform);

        if (Time.time > nextArrowTime && canShoot && target != null)
        {
            if (arrow.transform.position == originalArrowPosition) //at the start, play the shooting noise
            {
                FindObjectOfType<audioManagerScript>().Play("ArrowShoot");
            }

            arrow.transform.position = Vector3.MoveTowards(arrow.transform.position, target.transform.position, 50f * Time.deltaTime);
        }
        else
        {
            arrow.transform.position = originalArrowPosition;
        }
    }

    private void FindClosestTarget(List<Transform> targets)
    {
        Transform closest = targets[0];

        for(int i = 0; i < targets.Count-1; i++)
        {
            if (Mathf.Abs(targets[i].position.x - transform.position.x) > Mathf.Abs(targets[i+1].position.x - transform.position.x))
            {
                closest = targets[i + 1];
            }
        }

        target = targets[targets.IndexOf(closest)].GetComponent<UnitScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canShoot && collision.gameObject.CompareTag("Character") && collision.gameObject.GetComponent<UnitScript>().team != team)
        {
            canShoot = false;

            FindObjectOfType<audioManagerScript>().Play("ImpactArrow");

            arrow.GetComponent<ParticleSystem>().Play();

            collision.gameObject.GetComponent<UnitScript>().takeDamage(8, 0f, false);

            arrow.transform.position = originalArrowPosition;

            nextArrowTime = Time.time + 1f;
        }
    }

}
