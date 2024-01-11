using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rifleone : MonoBehaviour
{

    private bool valid = false;
    [SerializeField] GameObject rifle;
    [SerializeField] GameObject originalrifle;
    [SerializeField] GameObject lab;
    [SerializeField] int id;
    [SerializeField] GameObject player;
    private GameObject canvas;
    private float time = 0.0f;
    private bool timeStart = false;
    private bool changed = false;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("changeGun");
    }

    // Update is called once per frame
    void Update()
    {
        if (valid && Input.GetKeyDown(KeyCode.Q)&&!changed) {
            canvas.GetComponent<Canvas>().enabled = false;
            timeStart = true;
            lab.transform.Translate(Vector3.down);

            // give addons based on existing information about each rifle
            if (id == 1)
            {
                player.GetComponent<PlayerController>().gunChangeSpeedUp(1);
                player.GetComponent<GunController>().addFireInterval(0.05f);
            }
            else if (id == 2)
            {
                player.GetComponent<PlayerController>().gunChangeSpeedUp(-1);
                player.GetComponent<GunController>().addClipLoad();
            }
            else
            {
                player.GetComponent<PlayerController>().gunChangeSpeedUp(-1);
                player.GetComponent<GunController>().addFireInterval(-0.05f);
            }
            player.GetComponent<GunController>().reload();
            changed = true;
        }
        if (timeStart) {
            time += Time.deltaTime;
        }
        if (time > 0.5f) {
            originalrifle.SetActive(false);
            rifle.SetActive(true);           
        }

        if (time >= 1f)
        {
            Destroy(lab);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // display notifications
        if (other.gameObject.tag == "Player") {
            valid = true;
            if (!changed) {
                canvas.GetComponent<Canvas>().enabled = true;
            }
            
        }

    }

    private void OnTriggerExit(Collider other)
    {   // undisplay notifications
        if (other.gameObject.tag == "Player")
        {
            valid = false;
            if (!changed)
            {
                canvas.GetComponent<Canvas>().enabled = false;
            }
        }
    }


}
