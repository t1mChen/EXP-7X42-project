using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunRotator : MonoBehaviour
{
    // controller of parent object managing rotation of gun
    private Quaternion rotationOrigin;
    private float TOTAL_ANGLE = 90f;
    [SerializeField] int stepsize;
    private int isRotating = 1;

    // Start is called before the first frame update
    void Start()
    {
        rotationOrigin = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void rotateUp() {
        isRotating *= -1;
        StartCoroutine(upwardRotation(stepsize));
        
    }
    public void rotateforward()
    {
        isRotating *= -1;
        StartCoroutine(upwardRotation(-stepsize));
    }

    private IEnumerator upwardRotation(int up) {

        // rotate the gun up periodically for smoother presentation
        float track = 0f;
        while (true)
        {
            track += (TOTAL_ANGLE / stepsize);
            transform.Rotate(Vector3.left, (TOTAL_ANGLE / up));

            if (track >= TOTAL_ANGLE)
            {
                break; // Rotation is complete
            }

            yield return null;
        }
    }

    public int GetIsRotating()
    {
        return isRotating;
    }
}
