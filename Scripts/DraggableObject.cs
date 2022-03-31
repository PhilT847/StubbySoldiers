using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    public bool isCard; //cards shrink when dragged

    private Transform previewPart;
    private bool movable;
    private BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            //previewPart = GetComponent<CardPosition>().previewSprite;

            Touch follow = Input.GetTouch(0);
            Vector2 followPos = Camera.main.ScreenToWorldPoint(follow.position);

            if (follow.phase == TouchPhase.Began)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(followPos);

                if (touchedCollider == col)
                {
                    movable = true;
                }
            }

            if (follow.phase == TouchPhase.Moved && movable)
            {
                //previewPart.gameObject.GetComponent<SpriteRenderer>().enabled = true;

                transform.position = new Vector2(followPos.x, followPos.y);

                /*if(followPos.y > -15f)
                {
                    if (followPos.y > 0f)
                    {
                        previewPart.position = new Vector2(followPos.x, 6f);
                    }
                    else
                    {
                        previewPart.position = new Vector2(followPos.x, -10f);
                    }
                }*/

                if (isCard && transform.localScale.x > 0.4f)
                {
                    transform.localScale = new Vector2(transform.localScale.x - 2.5f * Time.deltaTime, transform.localScale.y - 2.5f * Time.deltaTime);
                    if (transform.localScale.x < 0.4f)
                    {
                        transform.localScale = new Vector2(0.4f, 0.4f);
                    }
                    //transform.localScale = new Vector2(0.5f, 0.5f);
                }
            }

            if (follow.phase == TouchPhase.Ended)
            {
                Collider2D touchedCollider = Physics2D.OverlapPoint(followPos);

                if (isCard && touchedCollider == col)
                {
                    GetComponent<CardPosition>().LetGo();
                }

                movable = false;
            }
        }

        if (isCard && !movable)
        {
            if (transform.position.x != GetComponent<CardPosition>().originalPosition.x || transform.position.y != GetComponent<CardPosition>().originalPosition.y)
            {
                transform.position = Vector2.MoveTowards(transform.position, GetComponent<CardPosition>().originalPosition, 200f * Time.deltaTime);
            }

            if (transform.localScale.x < 1f)
            {
                transform.localScale = new Vector2(transform.localScale.x + 5f * Time.deltaTime, transform.localScale.y + 5f * Time.deltaTime);

                if(transform.localScale.x > 1f)
                {
                    transform.localScale = new Vector2(1f, 1f);
                }
            }
        }
    }
}
