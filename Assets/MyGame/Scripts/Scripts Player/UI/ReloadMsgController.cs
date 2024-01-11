using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReloadMsgController : MonoBehaviour
{
    public TextMeshProUGUI bulletInGun; 
    public GameObject ReloadMsg;
    public float displayTime = 5f;
    private bool messageShown = false;

    // Update is called once per frame
    void Update()
    {
        int numBullet = int.Parse(bulletInGun.text);
        // Check if the bullet count is zero and the message hasn't been shown yet.
        if (!messageShown && numBullet <= 10)
        {
            // Activate the message.
            ReloadMsg.SetActive(true);
            messageShown = true;

            // Schedule deactivating the message after the displayTime.
            Invoke("DeActivate", displayTime);
        }
    }

    public void DeActivate()
    {
        ReloadMsg.SetActive(false);
    }
}
