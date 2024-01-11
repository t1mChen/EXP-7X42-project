using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletHole : MonoBehaviour
{
    // Start is called before the first frame update
    private float begintime = 0.0f;
    void Start()
    {
        begintime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // destroy bullet hole if longer than 10 seconds
        if (Time.time - begintime > 10.0f) { 
            Destroy(gameObject);
        }
    }
}
