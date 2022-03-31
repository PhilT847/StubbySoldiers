using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walkerScript : MonoBehaviour
{
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool("isWalking", true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.position = new Vector3(Random.Range(-12f,-30f), Random.Range(-4.6f, 2f), 0f);
    }
}
