using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rangeAttack : MonoBehaviour
{
    private float beginTime;
    [SerializeField] private Material material;
    private bool attacked = false;
    [SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        beginTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time-beginTime; // Get the current time
        material.SetFloat("_instantiateTime", time);

        if (time > 6.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if collide apply attack
        if (other.gameObject.tag == "Player") {
            if (!attacked) {
                GameObject.Find("Player").GetComponent<HealthManager>().ApplyDamage(damage);
                attacked = true;
            }
        }
    }
}
