/**
 * COMP 30019 Project2, Semester 2, 2023
 * This code takes inspiration from original code by Gilbert in the video published on Youtube on Sep.27th 2021
 * Original code found at "https://www.youtube.com/watch?v=geieixA4Mqc"
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    // Rotations
    private Vector3 currRotation;
    private Vector3 targetRotation;

    // Fire recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    // Setting
    [SerializeField] private float duration;
    [SerializeField] private float returnSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currRotation = Vector3.Slerp(currRotation, targetRotation, duration * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currRotation);
    }

    public void Recoil()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilX));
    }
}
