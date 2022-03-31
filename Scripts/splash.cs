using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class splash : MonoBehaviour
{
    private float enterTime;
    private bool loaded;

    // Start is called before the first frame update
    void Start()
    {
        enterTime = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > enterTime && !loaded)
        {
            loaded = true;
            SceneManager.LoadSceneAsync(1);
        }
    }
}
