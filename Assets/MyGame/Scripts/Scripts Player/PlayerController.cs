/**
 * COMP 30019 Milestone3, Semester 2, 2023
 * This code takes inspiration from original code by Dave/GameDevelopment in the video published on Youtube on Feb.8th 2022
 * Original code found at "https://www.youtube.com/watch?v=f473C43s8nE"
 *
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;



public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform orientation;

    [SerializeField] private float groundDrag;

    private GameObject timeManager;

    float horizontalInput;
    float verticalInput;
    Vector3 direction;
    Rigidbody rb;

    public Transform groundCheckPoint;
    private bool isGrounded;
    
    private float physicsControl = 10f;
    private float rayCastDetection = 3f;

    private float gravityControl = 15f;

    private bool playFootstep;
    private const float propSpeeding = 3;
    private float jumpdowndelay = 0.0f;
    public GameObject runIcon;
    public int movingDirection = 1;
    public float jumping = 3000;
    private float jumpingLowerHeight1 = 300;
    private float jumpingLowerHeight2 = 2500;
    private bool transit;
    public LayerMask jumpLowLayer1;
    public LayerMask jumpLowLayer2;

    // Start is called before the first frame update
    private void Start()
    {
        runIcon.SetActive(false);
        timeManager = GameObject.Find("TimerManager");
        // Get the rigidbody of player and freeze its rotation
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        transit = timeManager.GetComponent<TimerManagerScript>().getinTransitionStatus();
    }

    // Get inputs from keyboard
    private void getInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }

    // Update is called once per frame
    private void Update()
    {
        transit = timeManager.GetComponent<TimerManagerScript>().getinTransitionStatus();

        getInput();   // Get input from keyboard once per update
        // Check if "slipperyness" caused speed to be too fast
        speedControl();

        // Don't want the player move to be slippery, so add drag
        rb.drag = groundDrag;

        if (!transit) {
            movePlayer();
        }
        

        if (!isGrounded && Physics.Raycast(this.transform.position, Vector3.down, rayCastDetection)&&jumpdowndelay==0.0f) {
            GameObject.Find("jumpDown").GetComponent<AudioSource>().Play();
            jumpdowndelay += Time.deltaTime;
        }

        if (jumpdowndelay > 0.0f) {
            jumpdowndelay += Time.deltaTime;
        }
        if (jumpdowndelay >= 0.3f) { 
            jumpdowndelay = 0.0f; 
        }

        isGrounded = Physics.Raycast(this.transform.position, Vector3.down, rayCastDetection);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !transit)
        {
            StartCoroutine(jumpup());
        }

        if (!isGrounded && !transit) {
            rb.AddForce(Vector3.down.normalized * speed * gravityControl, ForceMode.Force);
        }

        if (verticalInput == 0.0f && horizontalInput == 0.0f) { 
            playFootstep = false;
        }

        if (verticalInput != 0.0f || horizontalInput != 0.0f)
        {
            playFootstep = true;
        }
        AudioSource footstep = GameObject.Find("footsteps").GetComponent<AudioSource>();
        if (playFootstep && isGrounded)
        {

            if (!footstep.isPlaying && !transit)
            {
                footstep.Play();
            }
            else {
                footstep.UnPause();
            }
        }
        else {
            footstep.Pause();
        }

    }
    public void speedUp()
    {
        StartCoroutine(speedUpForSeconds());
    }
    public void gunChangeSpeedUp(int sp) {
        speed += sp;
    }
    private IEnumerator speedUpForSeconds()
    {

        
        //GameObject run = Instantiate(runIcon, runIcon.transform).gameObject;
        int speedUpTime = 0;
        speed += propSpeeding;
        while (speedUpTime < 20)
        {
            yield return new WaitForSeconds(1);
            speedUpTime += 1;
            runIcon.SetActive(true);
        }
        speed -= propSpeeding;
        runIcon.SetActive(false);
        //Destroy(run);
    }
    private IEnumerator jumpup() {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        while (true)
        {
            //bool casted = Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer);
            //int hitLayer = hit.collider.gameObject.layer;

            //// You can also get the name of the layer if needed
            //string hitLayerName = LayerMask.LayerToName(hitLayer);

            //// Log the layer information
            //Debug.Log("Ray hit object on layer: " + hitLayerName);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, jumpLowLayer1))
            {
                rb.AddForce(Vector3.up.normalized * jumpingLowerHeight1, ForceMode.Force);
                transform.Translate(0f, 0.02f, 0f);
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, jumpLowLayer2))
            {
                rb.AddForce(Vector3.up.normalized * jumpingLowerHeight2, ForceMode.Force);
                transform.Translate(0f, 0.0f, 0f);
            }
            else
            {
                
                rb.AddForce(Vector3.up.normalized * jumping, ForceMode.Force);
                transform.Translate(0f, 0.1f, 0f);
            }
            
            break;
            
        }
        yield return null;
    }

    // Now move the player
    private void movePlayer()
    {
        // Calculate new movement direction as a vector
        direction = (orientation.forward * verticalInput + orientation.right * horizontalInput) * movingDirection;
        
        // Involving physics, make it move super fast and slippery
        rb.AddForce(direction.normalized * speed * physicsControl, ForceMode.Force);
    }

    // To avoid physics accelerating player too fast, control speed
    private void speedControl() 
    {
        Vector3 currentVelocity = new Vector3 (rb.velocity.x, 0f, rb.velocity.z);
        
        // Check if current velocity exceeds set speed
        if (currentVelocity.magnitude > speed)
        {
            Vector3 limit = currentVelocity.normalized * speed;
            rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
        }
    }
    
    public void SetReverseWalkingDirection()
    {
        StartCoroutine(ReverseDirection());
    }
    private IEnumerator ReverseDirection()
    {
        int time = 0;
        movingDirection = -movingDirection;
        while (time <= 20)
        {
            yield return new WaitForSeconds(1);
            
            time++;
        }
        movingDirection = -movingDirection;
    }

    public void WalkFaster()
    {
        speed += 3;
    }
    public void WalkSlower()
    {
        speed -= 1.5f;
    }

    public void ApplySuperGravity()
    {
        StartCoroutine(JumpLower());
    }
    private IEnumerator JumpLower()
    {
        jumping = 500;
        while (jumping < 1800)
        {
            yield return new WaitForSeconds(2);
            jumping += 100;
        }
    }
    public void ApplyLessGravity()
    {
        StartCoroutine(JumpHigher());
    }
    private IEnumerator JumpHigher()
    {
        jumping = 6000;
        while (jumping > 4000)
        {
            yield return new WaitForSeconds(2);
            jumping -= 200;
        }
    }
    
}

    