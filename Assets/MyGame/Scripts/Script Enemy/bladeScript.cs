using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bladeScript : MonoBehaviour
{
    private int speed = 15;
    private float initTime;
    private float LIMIT = 10.0f;
    private Vector3 dir;
    [SerializeField] int damage;
    // Start is called before the first frame update
    void Start()
    {
        initTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // update location
        this.transform.position = this.transform.position + dir * speed * Time.deltaTime;

        if (Time.time - initTime > LIMIT) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // apply damage of blade if collide with player
        if (other.gameObject.name == "Player") {
            GameObject.Find("Player").
                GetComponent<HealthManager>().ApplyDamage(damage);
        }
    }
    public void setDir(Vector3 d) {
        dir = d;
    }
}
