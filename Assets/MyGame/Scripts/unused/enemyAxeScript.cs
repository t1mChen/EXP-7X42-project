// Unused collision detection for testing interaction when enemy attacking player

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAxeScript : MonoBehaviour
{
    [SerializeField] GameObject parent;
    [SerializeField] GameObject player;
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<BoxCollider>().center = this.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        bool isAttack1 = parent.gameObject.GetComponent<Animator>().GetBool("isAttack1");
        bool isAttack2 = parent.gameObject.GetComponent<Animator>().GetBool("isAttack2");
        if (other.name == "PlayerObj" && (isAttack1 || isAttack2)) {
            GameObject.Find("PlayerObj").GetComponent<HealthManager>().ApplyDamage(damage);
        }
    }
}
