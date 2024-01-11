using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletController : MonoBehaviour
{
    private string EnemyObjectTag = "Enemy";
    private string PlayerObjectTag = "Player";
    private float initTime;
    private const int lifeTime = 10;
    public static int bulletHarm = 20;
    [SerializeField] GameObject explosion;
    // When colliding with any rigidbody, destroy bullet


    private void Start()
    {
        initTime = Time.time;
        audioManager manager = GameObject.Find("AudioManager").GetComponent<audioManager>();
        manager.gunFire();
    }
    private void Update()
    {
        // destroy bullet if fly over 10 seconds
        if (Time.time - initTime > lifeTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // attack enemy and make explosion effect when colliding
        if (collision.gameObject.tag == EnemyObjectTag) {
            collision.gameObject.GetComponent<EnemyScript>().getAttack();
            Instantiate(explosion, collision.ClosestPoint(transform.position), Quaternion.identity);

        }

        if (collision.gameObject.tag!=PlayerObjectTag)
        {
            if (collision.gameObject.tag == "structure") {
                AudioSource hitSound = GameObject.Find("bulletHit")
                    .GetComponent<AudioSource>();
                if (hitSound.isPlaying) {
                    hitSound.Stop();
                }
                hitSound.Play();

                Rigidbody rb = collision.GetComponent<Rigidbody>();
                Vector3 forceDir = (this.transform.position-
                    collision.gameObject.transform.position);
                // random variation
                forceDir += new Vector3(0.1f, 0.1f, 0.1f);
                forceDir = forceDir.normalized;
                if (rb != null)
                {
                    rb.AddForce(forceDir * 10f, ForceMode.Impulse);
                }
            }
            // Destroy the bullet when it collides with a Collider object
            if (gameObject != null) {
                Destroy(gameObject);
            }
            
        }
    }




    public static void increaseHarm()
    {
        bulletHarm += 2;
        
    }
}

