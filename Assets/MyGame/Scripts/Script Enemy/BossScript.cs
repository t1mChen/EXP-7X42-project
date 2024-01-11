using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossScript : MonoBehaviour
{
    
    // Change in position of the boss 
    private float WalkingSpeed = 1.5f;
    private const float StillSpeed = 0f;
    public float speed = StillSpeed;
    private const float InstantShiftRange = 40f;

    // The harm that boss can cause on the player 
    private const int HarmByPunch = 10;
    private const float HarmRangePunching = 4.1f;
    private const float HarmRangeJumping = 11.0f;
    private const float HarmRangeSwiping = 20.0f;
    private const float HarmRangeJumpAttacking = 6.0f;

    private Animator animator;

    private GameObject player; 
    [SerializeField] float displacementToPlayer;
    private NavMeshAgent agent;

    [SerializeField] GameObject blade;
    private bool beginSwipe = false;
    private float swipeStart = 0f;
    private bool swiped = false;
    private float endDelay = 0.0f;

    [SerializeField] GameObject jumpAttackObject;
    private bool beginJump = false;
    private float jumpStart = 0f;
    private bool jumpAttacked = false;

    private GameObject timerManager;
    private bool readyAttack = false;
    private float attackTimer = 0f;
    private bool attacked = false;

    [SerializeField] private AudioSource roarAudio;
    [SerializeField] private AudioSource jumpAudio;
    [SerializeField] private AudioSource jumpAttAudio;
    [SerializeField] private AudioSource swipeAudio;
    [SerializeField] private AudioSource punchAudio;

    private bool roared = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the attributes         
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        timerManager = GameObject.Find("TimerManager");
    }


    // Update is called once per frame
    void Update()
    {   
        // Only update after the boss is out the cage 
        if (animator.GetBool("outTheCage"))
        {
            // Update the distance between boss and the player 
            displacementToPlayer = (player.transform.position - this.transform.position).magnitude;

            // Conduct suitable animation 
            UpdateBossAnimation();
            UpdateBossDamage();
            UpdateBossSpeed();
            BossMove();
        }

        // when mutant performs a swipe, generate a blade that fly into direction of player
        bossSwipe();

        // when mutant performs a jump, generate a distance attack within circle of boss
        bossJump();

    }

    private void bossJump() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Jumping"))
        {
            beginJump = true;
        }
        else if(jumpStart > 3.168f){ 
            jumpStart = 0f;
            jumpAttacked = false;
            beginJump = false;
        }
        if (beginJump) {
            jumpStart += Time.deltaTime;
            if (jumpStart > 1.5f && !jumpAttacked) {
                jumpAudio.Play();
                jumpAttacked = true;
                GameObject jumpAtt = Instantiate(jumpAttackObject);
                jumpAtt.transform.position = this.transform.position;
            }
        }

    }

    private void bossSwipe() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Swiping"))
        {
            beginSwipe = true;
        }
        else
        {
            endDelay += Time.deltaTime;
            swiped = false;
            swipeStart = 0f;
        }
        // if the time is matched with swipe and it is a valid attack
        if (endDelay > 0.2f && beginSwipe && swipeStart >= 1 && !swiped)
        {
            swipeAudio.Play();
            // attack the blade towards the player
            Vector3 target = player.transform.position;
            GameObject bladeObject = Instantiate(blade);
            bladeObject.transform.position = transform.position;
            target = target - this.transform.position;
            target = target.normalized;
            bladeObject.GetComponent<bladeScript>().setDir(target);
            bladeObject.transform.rotation = Quaternion.LookRotation(target, Vector3.up);

            swipeStart = 0f;
            beginSwipe = false;
            endDelay = 0f;
            swiped = true;
        }
        if (beginSwipe)
        {
            swipeStart += Time.deltaTime;
        }
    }


    private void BossMove()
    {
        // Walking forward with the current speed         
        agent.SetDestination(player.transform.position);

        if (timerManager.GetComponent<TimerManagerScript>().getinTransitionStatus()) {
            agent.speed = 0;
        }
        else if (displacementToPlayer > InstantShiftRange) { agent.speed = 100; } 
        else { agent.speed = speed; }
    }


    private void UpdateBossAnimation()
    {
        if (displacementToPlayer > HarmRangeSwiping)
        {
            ToWalking(); 
        }
        else if (displacementToPlayer > HarmRangeJumping)
        {
            // randomly swip or walking
            int animationIndex = Random.Range(0, 100);
            if (animationIndex < 50) { ToSwiping(); } // check if the boss is on the same layer as the player
            else { ToWalking(); }
            
        }
        else if (displacementToPlayer > HarmRangeJumpAttacking + 0.5f)
        {
            // randomly swip, jump, walk 
            int animationIndex = Random.Range(0, 100);
            if ( animationIndex < 30 ) 
            { 
                ToJumping(); 
            }
            else if (animationIndex < 50) { ToSwiping(); }
            else { ToWalking(); }
        }
        else
        {
            ToPunch();
        }
    }


    private void UpdateBossSpeed()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Walking")) { speed = WalkingSpeed; }
        else { speed = StillSpeed; }
    }


    private void UpdateBossDamage()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Mutant Punch"))
        {
            rotateToPlayer();
            readyAttack = true;
            if (attackTimer >= 0.4f && !attacked)
            {
                punchAudio.Play();
                if (displacementToPlayer<HarmRangePunching)
                {
                    player.GetComponent<HealthManager>().ApplyDamage(HarmByPunch);
                }
                attacked = true;
            }
            if (attackTimer >= 1.07f)
            {
                attacked = false;
                attackTimer = 0f;
            }
        }
        else
        {
            readyAttack = false;
        }
        if (readyAttack)
        {
            attackTimer += Time.deltaTime;
        }
        else
        {
            attackTimer = 0f;
            attacked = false;
        }
    }

    private void rotateToPlayer()
    {
        // rotate y axis towards the player
        Vector3 directionToPlayer = player.transform.position - this.transform.position;

        // this line of code is generated with chatGPT, calculate degrees from direction
        float angleToPlayer = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;

        this.transform.rotation = Quaternion.Euler(new Vector3(0f, angleToPlayer, 0f));
    }

    // Setting the animator conditions 
    public void OutTheCage()
    {
        animator.SetBool("outTheCage", true);
        if (!roared) {
            roared = true;
            roarAudio.PlayDelayed(1.0f);
        }
    }

    // Walking is a looping animation 
    private void ToWalking()
    {
        animator.SetBool("toWalking", true);
        animator.SetBool("toPunch", false);
        animator.SetBool("toSwiping", false);
        animator.SetBool("toJumping", false);
        animator.SetBool("toJumpAttack", false);
    }

    // Punch is a looping animation 
    private void ToPunch()
    {
        animator.SetBool("toWalking", false);
        animator.SetBool("toPunch", true);
        animator.SetBool("toSwiping", false);
        animator.SetBool("toJumping", false);
        animator.SetBool("toJumpAttack", false);
    }

    private void ToSwiping()
    {
        animator.SetBool("toWalking", true);
        animator.SetBool("toPunch", false);
        animator.SetBool("toSwiping", true);
        animator.SetBool("toJumping", false);
        animator.SetBool("toJumpAttack", false);
    }

    private void ToJumping()
    {
        animator.SetBool("toWalking", true);
        animator.SetBool("toPunch", false);
        animator.SetBool("toSwiping", false);
        animator.SetBool("toJumping", true);
        animator.SetBool("toJumpAttack", false);
    }

    public bool speedUp() {
        WalkingSpeed += 1f;
        return true;
    }

    public bool slowDown() {
        if (WalkingSpeed > 0.5f) {
            WalkingSpeed -= 0.5f;
            return true;
        }
        return false;
    }



}
