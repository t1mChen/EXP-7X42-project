using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMsgController : MonoBehaviour
{
    public float displayTime = 5f;
    public float delayTime = 2f;

    private float startTime;
    private bool messageShown = false;
    private GameObject childObject;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        childObject = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!messageShown && Time.time - startTime >= delayTime)
        {
            childObject.SetActive(true);
            messageShown = true;

            // Schedule deactiving the message after the displayTime.
            Invoke("DeActivate", displayTime);
        }
    }

    public void DeActivate()
    {
        childObject.SetActive(false);
    }
}
