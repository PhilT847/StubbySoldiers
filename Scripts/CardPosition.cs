using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CardPosition : MonoBehaviour
{
    public int order;

    private ParticleSystem spawnParticles;

    public Transform previewSprite;

    [HideInInspector]
    public Image cardImage;
    [HideInInspector]
    public GameObject cardSpawn; //what it makes- unit, spell...
    [HideInInspector]
    public CardScript currentCard;

    [HideInInspector]
    public bool isSpell;

    [HideInInspector]
    public Vector2 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        cardImage = GetComponent<Image>();
        originalPosition = transform.position;

        spawnParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void SetCard() //sets the spawned object and image
    {
        Sprite s = currentCard.GetComponent<SpriteRenderer>().sprite;
        GetComponent<Image>().sprite = s;

        cardSpawn = currentCard.unit;

        isSpell = currentCard.isSpell;

        if (isSpell)
        {

        }
        else
        {

        }
    }

    public void LetGo() //spawns the unit if it's in a spawning area, or returns the card to its position if not.
    {
        if (GetComponentInParent<Deck>().team.energy > currentCard.cost && transform.position.y > -23f && ((!isSpell && transform.position.x < -12f) || (isSpell /*&& ((transform.position.y > 0f && transform.position.y < 15f) || (transform.position.y > -20f && transform.position.y < -5f))*/))) //if not enough points or not placed correctly, go back.
        {
            FindObjectOfType<audioManagerScript>().Play("Pop");

            GetComponentInParent<Deck>().team.energy -= currentCard.cost; //spend energy

            if (!isSpell)
            {

                if (transform.position.y > 0f) //top lane
                {
                    var unit = Instantiate(cardSpawn, new Vector3(transform.position.x, 5.5f, 0f), Quaternion.identity);
                    spawnParticles.gameObject.transform.position = unit.transform.position;
                    unit.GetComponent<UnitScript>().team = GetComponentInParent<Deck>().team;
                }
                
                else //bot lane
                {
                    var unit = Instantiate(cardSpawn, new Vector3(transform.position.x, -12f, 0f), Quaternion.identity);
                    spawnParticles.gameObject.transform.position = unit.transform.position;
                    unit.GetComponent<UnitScript>().team = GetComponentInParent<Deck>().team;
                }

                spawnParticles.Play();
            }
            else
            {
                if (transform.position.y > 0f) //top lane
                {
                    var unit = Instantiate(cardSpawn, new Vector3(transform.position.x, 7.5f, 0f), Quaternion.identity);
                    unit.GetComponent<spellScript>().team = GetComponentInParent<Deck>().team;
                }

                else //bot lane
                {
                    var unit = Instantiate(cardSpawn, new Vector3(transform.position.x, -10.5f, 0f), Quaternion.identity);
                    unit.GetComponent<spellScript>().team = GetComponentInParent<Deck>().team;
                }
            }
            

            GetComponentInParent<Deck>().replaceCard(order); //replace the card

            transform.localScale = new Vector2(1f, 1f);
            transform.position = originalPosition;
        }
        /*
        if (!isSpell)
        {
            if (GetComponentInParent<Deck>().team.energy > currentCard.cost && (GetComponent<BoxCollider2D>().IsTouchingLayers(12) || GetComponent<BoxCollider2D>().IsTouchingLayers(13))) //if not enough points or not placed correctly, go back.
            {
                FindObjectOfType<audioManagerScript>().Play("Pop");

                GetComponent<ParticleSystem>().Play();

                GetComponentInParent<Deck>().team.energy -= currentCard.cost; //spend energy

                var unit = Instantiate(cardSpawn, transform.position, Quaternion.identity);
                unit.GetComponent<UnitScript>().team = GetComponentInParent<Deck>().team;

                GetComponentInParent<Deck>().replaceCard(order); //replace the card

                transform.localScale = new Vector2(1f, 1f);
                transform.position = originalPosition;
            }
        }
        else
        {
            if (GetComponentInParent<Deck>().team.energy > currentCard.cost) //check for y position) //if not enough points or not placed correctly, go back.
            {
                //FindObjectOfType<audioManagerScript>().Play("Pop");

                GetComponent<ParticleSystem>().Play();

                GetComponentInParent<Deck>().team.energy -= currentCard.cost; //spend energy

                var unit = Instantiate(cardSpawn, transform.position, Quaternion.identity);

                unit.GetComponent<spellScript>().team = GetComponentInParent<Deck>().team;

                GetComponentInParent<Deck>().replaceCard(order); //replace the card

                transform.localScale = new Vector2(1f, 1f);
                transform.position = originalPosition;
            }
        }
        */


        //always return to form... see Draggable for MoveToward when not being moved
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //sparkle if over spawn region
        if (GetComponent<BoxCollider2D>().IsTouchingLayers(12) || GetComponent<BoxCollider2D>().IsTouchingLayers(13))
        {
        }
        else
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
    }
}
