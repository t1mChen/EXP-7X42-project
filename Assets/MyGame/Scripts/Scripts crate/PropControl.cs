using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
// contains the behaviour of self-rotating and moving towards player
// (use F to pickup prop, which is a hidden game logic not told to player)
public class PropControl : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float stepTime;
    private bool isRotating = true;
    private bool isMoving = false;
    private bool initial = true;
    private float rotatePeriod = 0.001f;
    private Vector3 rotatingSpeed = new Vector3(0, -1, 0);
    void Start()
    {
        // find the player
        player = GameObject.Find("Player");
        Vector3 angularVelocity = GetComponent<Rigidbody>().angularVelocity;

        // Compare the magnitude of angular velocity to the threshold
        StartCoroutine(RotatePeriodically());
    }


    void Update()
    {
        // make continuous rotating and moving effects of props
        if (gameObject.activeSelf)
        {
            if (initial)
            {
                // show prop in a position obvious
                gameObject.transform.localPosition = new Vector3(0, 1f, 0);
                initial = false;
            }



            if (isMoving)
            {
                // prop move towards the player
                transform.Translate((player.GetComponent<Collider>().bounds.center
                    - GetComponent<Collider>().bounds.center) * Time.deltaTime);
            }



        }
    }

    private IEnumerator RotatePeriodically()
    {
        while (!Input.GetKeyDown(KeyCode.F) && isRotating)
        {
            yield return new WaitForSeconds(rotatePeriod);
            transform.Rotate(rotatingSpeed);
        }
        isRotating = false;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        isMoving = true;
    }


}
