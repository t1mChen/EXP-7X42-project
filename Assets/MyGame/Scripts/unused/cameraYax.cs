
// Unused camera control script for testing purpose
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraYax : MonoBehaviour
{
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * speed;
        transform.Rotate(Vector3.left * mouseY);
    }
}
