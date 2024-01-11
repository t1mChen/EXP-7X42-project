
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.AI.Navigation;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;

public class AIEnemyScript : MonoBehaviour
{
    // can change the public to "private const" when the distance is confirmed 
    // public only for adjusut attributes in unity 
    public int DetectingDistance = 100;
    public int AttackingDistance = 20;
    public int WalkingSpeed = 10;
    public int RunningSpeed = 30;
    private const int StillSpeed = 0;

    private const string PlayerObjectName = "Player";
    private const string BulletObjectTag = "Bullet";
    private const int BulletHarm = 10;
    private int health = 1000;
    private int FULL_HEALTH = 1000;
    private int speed = 10;

    private const float UpdateAnimationInterval = 1.0f;
    private const float SelfTurnInterval = 5.0f;

    private Animator animator;
    private GameObject player;
    private Transform playerTransform;
    private float displacementToPlayer;
    private bool isgrounded;


    Rigidbody rb;

    [SerializeField] GameObject blood;
    [SerializeField] GameObject bloodbar;

    private float physicsControl = 10f;
    private float rayCastDetection = 4f;
    private float bloodBarY = 0.2f;
    private float bloodBarZ = 0.000001f;
    private bool discoverPlayer;
    private NavMeshAgent agent;


    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find(PlayerObjectName);
        agent = GetComponent<NavMeshAgent>();
        discoverPlayer = false;
        playerTransform = player.transform;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();

        InvokeRepeating("UpdateHealthyAnimation", 0.01f, UpdateAnimationInterval);
        InvokeRepeating("EnemySelfTurn", SelfTurnInterval, SelfTurnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        // update current blood
        float bloodpercentage = ((((float)health) / FULL_HEALTH));
        blood.GetComponent<TextMeshPro>().text = ((int)(bloodpercentage * 100f)).ToString() + "%";
        bloodbar.transform.localScale = new Vector3(bloodpercentage, bloodBarY, bloodBarZ);

        // update the displacement to the player 
        displacementToPlayer = (player.transform.position - this.transform.position).magnitude;
        if (!discoverPlayer)
            SearchingPlayer();

        EnemyMove();

        // make sure the enemy sticks to the ground when going down the stairs 
        isgrounded = Physics.Raycast(this.transform.position, Vector3.down, rayCastDetection);
        if (!isgrounded)
        {
            rb.AddForce(Vector3.down.normalized * speed * physicsControl, ForceMode.Force);
        }

    }

    private void SearchingPlayer()
    {
        // If the distance is close enough, turn to player 
        if (displacementToPlayer < DetectingDistance)
        {
            discoverPlayer = true;
            //// rotate y axia towards the player
            //Vector3 directionToPlayer = player.transform.position - this.transform.position;

            //// this line of code is generated with chatGPT, calculate degrees from direction
            //float angleToPlayer = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;

            //this.transform.rotation = Quaternion.Euler(new Vector3(0f, angleToPlayer, 0f));
        }
        else
        { discoverPlayer = false; }
    }

    private void EnemyMove()
    {
        // Walking forward with the current speed 
        if (!discoverPlayer)
            this.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        else
        { agent.SetDestination(playerTransform.position); }


    }

    private void EnemySelfTurn()
    {
        // if a certain area around the current position is empty 
        // randomly turn to the other direction
    }

    // automatically called?
    private void OnTriggerEnter(Collider collision)
    {

        // if the enemy collide with wall, turn around
        if (collision.gameObject.name != PlayerObjectName && collision.gameObject.tag != "Floor")
        {
            // randomly turn towards a direction
            //float[] turns = [90, 180, 270];
            //int turnIndex = Random.Range(0, turns.Length);
            int turnDegree = Random.Range(90, 180);

            /*
            Vector3 angle = this.transform.localEulerAngles;
            angle.y += turnDegree;
            this.transform.localEulerAngles = angle;*/

            this.transform.Rotate(Vector3.up, turnDegree);
        }

    }

    // update health if gets hit
    public void getAttack()
    {
        health -= BulletHarm;
        print(health);
        UpdateUnhealthyAnimation();

    }

    private void UpdateHealthyAnimation()
    {
        // If the player is very close, start attacking 
        if (displacementToPlayer < AttackingDistance)
        {
            DoAttack();
            speed = StillSpeed;
            player.GetComponent<HealthManager>().ApplyDamage(10);
        }

        // If the player is close enough, run towards the player 
        else if (displacementToPlayer < DetectingDistance)
        {
            DoRun();
            speed = RunningSpeed;
            agent.speed = RunningSpeed;
        }

        // If the distance is not close enough 
        else
        {
            DoWalk();
            speed = WalkingSpeed;
            agent.speed = WalkingSpeed;
        }

    }

    private void UpdateUnhealthyAnimation()
    {
        if (health <= 0)
        {
            speed = StillSpeed;
            DoDeath();
            Destroy(this);
        }

        else if (health % 500 == 0)
        {
            speed = StillSpeed;
            DoHit1();
        }

        else if (health % 100 == 0)
        {
            speed = StillSpeed;
            DoHit2();
        }
    }

    private void DoWalk()
    {
        animator.SetBool("isWalk", true);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", false);
        animator.SetBool("isHit2", false);
        animator.SetBool("isDeath", false);

    }

    private void DoRun()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", true);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", false);
        animator.SetBool("isHit2", false);
        animator.SetBool("isDeath", false);
    }

    private void DoAttack()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", true);
        animator.SetBool("isAttack2", true);
        animator.SetBool("isHit1", false);
        animator.SetBool("isHit2", false);
        animator.SetBool("isDeath", false);

    }

    private void DoHit1()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", true);
        animator.SetBool("isHit2", false);
        animator.SetBool("isDeath", false);
    }

    private void DoHit2()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isAttack1", false);
        animator.SetBool("isAttack2", false);
        animator.SetBool("isHit1", false);
        animator.SetBool("isHit2", true);
        animator.SetBool("isDeath", false);
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

}
