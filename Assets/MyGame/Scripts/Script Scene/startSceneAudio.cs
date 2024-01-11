using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startSceneAudio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // get volume set in settings
        AudioListener.volume = PassVariables.getVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
