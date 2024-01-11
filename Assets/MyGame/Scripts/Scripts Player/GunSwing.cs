using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSwing : MonoBehaviour
{
    public float angle; // the angle of swing
    public float maxAngle;
    public float smooth; // to make the swing smoother

    private Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    { 
        // current location with respect to parent location
        originalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float swingX = -Input.GetAxis("Mouse X") * angle;
        float swingY = -Input.GetAxis("Mouse Y") * angle;

        // limit the max swing you can have
        swingX = Mathf.Clamp(swingX, -maxAngle, maxAngle);
        swingY = Mathf.Clamp(swingY, -maxAngle, maxAngle);

        Vector3 finalPosition = new Vector3(swingX, swingY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + originalPosition, Time.deltaTime * smooth);


    }
}
