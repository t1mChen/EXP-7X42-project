using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
    private float timer = 0;
    private const float fireLifeTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        // the fire effect lasts two seconds to represent the player gets the prop
        if (timer >= fireLifeTime)
        {
            Destroy(gameObject);
        }
    }

}
