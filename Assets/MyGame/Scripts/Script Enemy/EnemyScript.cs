using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyScript : MonoBehaviour
{
    // can change the public to "private const" when the distance is confirmed 
    // public only for adjusut attributes in unity 
    private int DetectingDistance = 15;
    private const float AttackingDistance = 6.5f;

    // Speed     
    [SerializeField] float WalkingSpeed; // slow enemy = 1.1; fast enemy = 1.7
    private const float RunningSpeed = 6.0f;
    private const float StillSpeed = 0;
    private float speed;

    private const string PlayerObjectName = "Player";
    private const string BulletObjectTag = "Bullet";
    private const string EnemyObjectTag = "Enemy";

    //private const int BulletHarm = 100;
    private int FullHealth = 120;
    private int health;
    
    private const float UpdateAnimationInterval = 0.5f;

    private Animator animator;

    private GameObject player;
    private float displacementToPlayer;
    private float detectingDistanceToPlayer;
    private int detectingOffset = 5;
    private bool isgrounded;
    private bool discoverPlayer;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private GameObject blood;
    private GameObject bloodbar;

    private float physicsControl = 10f;
    private float rayCastDetection = 4f;
    private float bloodBarY = 0.2f;
    private float bloodBarZ = 0.000001f;
    private float attackTimer = 0f;
    private int minotaurDamage = 10;
    private bool attacked = false;
    private bool validAttack = false;
    private GameObject timeManager;
    private bool isBlind = false;
    private bool isHitByBullet = false;
    private float chaseTime = 0;
    private const float ChaseTimeAfterBullet = 5; 

    [SerializeField] private AudioSource hitAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] private AudioSource deathAudio;
    private bool deathPlayed = false;
    private int prevHealth;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        prevHealth = FullHealth;
        health = FullHealth;
        timeManager = GameObject.Find("TimerManager");
        agent = GetComponent<NavMeshAgent>();
        discoverPlayer = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        player = GameObject.Find(PlayerObjectName);

        blood = this.transform.Find("blood").gameObject;
        bloodbar = this.transform.Find("bloodbar").gameObject;

        speed = WalkingSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlind) return;
        // update the displacement to the player 
        displacementToPlayer = (player.transform.position - this.transform.position).magnitude;
        detectingDistanceToPlayer = (player.transform.position - (this.transform.position + detectingOffset * this.transform.forward)).magnitude;
        SearchingPlayer();


        EnemyMove();
        if (animator.GetBool("isDeath") == false) { UpdateHealthyAnimation(); }
        

        // make sure the enemy sticks to the ground when going down the stairs 
        isgrounded = Physics.Raycast(this.transform.position, Vector3.down, rayCastDetection);
        if (!isgrounded)
        {
            rb.AddForce(Vector3.down.normalized * speed * physicsControl, ForceMode.Force);
        }

        if (displacementToPlayer <= AttackingDistance)
        {
            validAttack = true;
        }
        else {
            validAttack = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            rotateToPlayer();
            attackTimer += Time.deltaTime;
            if (attackTimer >= 0.2f && !attacked)
            {
                attackAudio.Play();
                if (validAttack) {
                    player.GetComponent<HealthManager>().ApplyDamage(minotaurDamage);
                }                
                attacked = true;
            }
            if (attackTimer >= 1.13)
            {
                attacked = false;
                attackTimer = 0f;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            rotateToPlayer();
            attackTimer += Time.deltaTime;
            if (attackTimer >= 0.25f && !attacked)
            {
                attackAudio.Play();
                if (validAttack) {
                    player.GetComponent<HealthManager>().ApplyDamage(minotaurDamage);
                }
                attacked = true;
            }
            if (attackTimer >= 1.2)
            {
                attacked = false;
                attackTimer = 0f;
            }
        }
        else {
            attackTimer = 0f;
            attacked = false;
        }


        // update current blood
        updateHealthUI();
        
        if (health <= 0)
        {
            speed = 0;
            agent.speed = 0; 
            if (!deathPlayed)
            {
                deathAudio.Play();
                deathPlayed = true;
            }
            this.tag = EnemyCreator.DeadTag; 
        }
        if (prevHealth != health && health % 100 == 0) {
            hitAudio.Play();
        }
        prevHealth = health;
    }

    public void updateHealthUI()
    {
        float bloodpercentage = ((((float)health) / FullHealth));
        blood.GetComponent<TextMeshPro>().text = ((int)(bloodpercentage * 100f)).ToString() + "%";
        bloodbar.transform.localScale = new Vector3(bloodpercentage, bloodBarY, bloodBarZ);
    }
    public void enhanceEnemy()
    {
        FullHealth += 100;
        health *= 6 / 5;
        updateHealthUI();
    }
    public void weakerEnemy()
    {
        FullHealth -= 100;
        health *= 4 / 5;
        updateHealthUI();
    }
    private void rotateToPlayer() {
        // rotate y axis towards the player
        Vector3 directionToPlayer = player.transform.position - this.transform.position;

        // this line of code is generated with chatGPT, calculate degrees from direction
        float angleToPlayer = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;

        this.transform.rotation = Quaternion.Euler(new Vector3(0f,angleToPlayer,0f));
    }

    private void SearchingPlayer()
    {
        // If the distance is close enough, turn to player 
        if (detectingDistanceToPlayer < DetectingDistance && animator.GetBool("isDeath") == false)
        {
            discoverPlayer = true;
        }
        else 
        { 
            discoverPlayer = false; 
        }
    }

    private void EnemyMove()
    {
        /* If during the transition of boss coming out the cage, do not move */
        if (timeManager.GetComponent<TimerManagerScript>().getinTransitionStatus()) 
        {
            speed = 0; 
            agent.speed = speed; 
            return; 
        }

        /* If the player is not in its detecting distance, nor the enemy hit by bullet, 
         * move forward by the current speed
         */
        if (!discoverPlayer && !isHitByBullet)
        {
            if (agent.hasPath) {
                agent.ResetPath();
            }
            
            this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        /* Otherwise move towards the player */
        else
        {
            agent.speed = speed; 
            agent.SetDestination(player.transform.position);

            /* If hit by the bullet, chase the player for a period of time */
            if (isHitByBullet) 
            { 
                chaseTime += Time.deltaTime; 
                if (chaseTime >= ChaseTimeAfterBullet)
                {
                    /* Reset attributes */
                    isHitByBullet = false; 
                    chaseTime = 0;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        // if the enemy collide with wall, turn around
        if (collision.gameObject.name != PlayerObjectName 
            && collision.gameObject.tag != BulletObjectTag
            && collision.gameObject.tag != EnemyObjectTag 
            && discoverPlayer == false)
        {
            // randomly turn towards a direction
            int turnDegree = Random.Range(90, 270);
            StartCoroutine(rotatetowards(turnDegree));
        }
        else if (collision.gameObject.tag == BulletObjectTag)
        {
            isHitByBullet = true; 
        }

    }


    private IEnumerator rotatetowards(int turnDegree)
    {
        int current = 0;
        while (current<turnDegree)
        {
            if (current + 10 >= turnDegree) {
                this.transform.Rotate(Vector3.up, turnDegree - current);
                break;
            }
            current += 10;
            this.transform.Rotate(Vector3.up, 10);
        }
        yield return null;
    }



    // update health if gets hit
    public void getAttack() {
        int newHealth = health - BulletController.bulletHarm;
        if (newHealth < 0) { health = 0; } 
        else { health = newHealth; }
        UpdateUnhealthyAnimation();
    }

   private void UpdateHealthyAnimation()
    {      
        // If the player is very close, start attacking 
        if (displacementToPlayer < AttackingDistance)
        {
            DoAttack();            
            speed = StillSpeed;
            discoverPlayer = false; 
        }

        // If the player is close enough, run towards the player 
        else if (detectingDistanceToPlayer < DetectingDistance)
        {
            DoRun();            
            speed = RunningSpeed; 
        }
        
        // If the distance is not close enough 
        else
        {
            DoWalk();
            speed = WalkingSpeed; 
        }        

    }

    private void UpdateUnhealthyAnimation() 
    {
        if (health <= 0)
        {
            
            speed = StillSpeed;
            discoverPlayer = false; 
            DoDeath();
        }

        else if (75 < health && health < 85) // if health is 80
        {
            speed = StillSpeed;
            discoverPlayer = false; 
            DoHit1();            
        }

        else if (35 < health && health < 45) // if health is 40
        {
            speed = StillSpeed;
            discoverPlayer = false; 
            DoHit2();            
        }
    }

    public void broadenSight()
    {
        DetectingDistance += 6;
    }

    public void shortenSight()
    {
        DetectingDistance -= 3;
    }

    private void DoWalk()
    {
        animator.SetBool("isWalk", true);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", false);
        animator.SetBool("isHit2", false);
    }

    private void DoRun()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", true);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", false);
        animator.SetBool("isHit2", false);
    }

    private void DoAttack()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", true);
        animator.SetBool("isAttack2", true);
        animator.SetBool("isHit1", false);
        animator.SetBool("isHit2", false);

    }

    private void DoHit1()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", true);
        animator.SetBool("isHit2", false);
    }

    private void DoHit2()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", false);
        animator.SetBool("isHit2", true);
    }

    private void DoDeath()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", true);
        animator.SetBool("isHit2", true);
        animator.SetBool("isDeath", true);
    }

    public void BlindToPlayer()
    {
        StartCoroutine(inBlindingMode());
    }
    private IEnumerator inBlindingMode()
    {
        isBlind = true;
        discoverPlayer = false;
        int time = 0;
        while (time <= 15) {
            yield return new WaitForSeconds(1);
            time++;
        }
        isBlind = false;
    }

}
