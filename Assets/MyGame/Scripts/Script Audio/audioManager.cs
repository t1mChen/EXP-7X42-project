using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    [SerializeField] private AudioSource ad;
    // Start is called before the first frame update
    void Start()
    {
        ad = GetComponent<AudioSource>();
        AudioListener.volume = PassVariables.getVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gunFire()
    {
        ad.Play();
    }
}
