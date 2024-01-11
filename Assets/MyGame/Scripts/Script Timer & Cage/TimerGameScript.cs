using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        
        this.GetComponent<TextMeshPro>().text = TimerManagerScript.GetGameCountDownStr(); 
    }
}
