using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class introEvent : MonoBehaviour
{
    public UnityEvent onEnter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // press enter to continue or next instruction
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)){
            onEnter.Invoke();
        }
    }
}
