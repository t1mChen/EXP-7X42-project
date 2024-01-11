
/**
 * COMP 30019 Milestone3, Semester 2, 2023
 * This code takes inspiration from original code by Dave/GameDevelopment in the video published on Youtube on Feb.8th 2022
 * Original code found at "https://www.youtube.com/watch?v=f473C43s8nE"
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{   
    // Define sensitivity on horizontal and vertical axis
    private float sens = 30;
    
    [SerializeField] private Transform orientation;

    private float xRotation;
    private float yRotation;
    
    private void Start()
    {
        // Get sensitivity from settings
        sens = PassVariables.getSens();
        // Make sure cursor is locked in middle of screen 
        Cursor.lockState = CursorLockMode.Locked;
        // Cursor invisible
        Cursor.visible = false;
    }

    private void Update()
    {
        // Get mouse movement update
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;

        // add or subtract the change from rotations
        xRotation = xRotation - mouseY;
        yRotation = yRotation + mouseX;

        // Cannot look up or down over 90 degrees, add restriction
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.localRotation = Quaternion.Euler(0, yRotation, 0);

    }
    
}
