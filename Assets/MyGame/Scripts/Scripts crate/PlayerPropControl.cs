using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using TMPro;

public class PlayerPropControl : MonoBehaviour
{
    public AudioClip pickupPropSound;
    // the type of things that can be pickup
    [SerializeField] private string tagToPickup;
    [SerializeField] private ParticleSystem deathEffect;
    public WeightedRandomList<Transform> effectTable;
    [SerializeField] GameObject myBag;
    private bool openBag = false;
    private GameObject lastCollided = null;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OpenMyBag();
    }

    

    // haven't been used in current stage
    public void Kill()
    {
        var explosion = Instantiate(this.deathEffect);
        explosion.transform.position = transform.position;
    }
    private IEnumerator DestroyObjectAfterDelay(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        Transform fireEffect = effectTable.GetRandom();
        GameObject fire = Instantiate(fireEffect, transform).gameObject;
        fire.transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collided = other.gameObject;
        // the player get the prop and add to the collections with an fire effect
        
        if (collided.tag == tagToPickup)
        {
            if (lastCollided == null)
            {
                dealWithProp(collided);
            }
            else if (lastCollided != collided)
                dealWithProp(collided);
            GetComponent<AudioSource>().PlayOneShot(pickupPropSound);
        }
        

    }
    void dealWithProp(GameObject prop)
    {
        if (prop.name.Equals("Pistolbullet"))
        {
            GameObject.Find("Player").GetComponent<GunController>().OnBulletPicked();
        }
        else
        {
            ItemOnWorld iow = prop.GetComponent<ItemOnWorld>();
            iow.AddNewItem();
            
        }
        lastCollided = prop;
        StartCoroutine(DestroyObjectAfterDelay(prop));
    }
    void OpenMyBag()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            openBag = !openBag;
            myBag.SetActive(openBag);
        }

    }
}
