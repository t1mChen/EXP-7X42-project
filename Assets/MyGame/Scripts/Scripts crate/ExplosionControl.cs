using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionControl : MonoBehaviour
{
    private float timer;
    private const float explosionTime = 0.5f;
    void Start()
    {
        timer = 0;   
    }

    // explosion effect remains half seconds
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= explosionTime)
        {
            Destroy(gameObject);
        }
    }
}
