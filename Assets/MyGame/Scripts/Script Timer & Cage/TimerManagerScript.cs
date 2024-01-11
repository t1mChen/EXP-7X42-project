using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerManagerScript : MonoBehaviour
{
    
    // Attributes related to boss timer 
    private float bossCountDown = 150f; // for actual game, set to 150f
    private GameObject bossTimerPrefab;
    private const string BossTimerName = "bossTimer";
    private static string bossCountDownStr; 

    // Attributes related to game timer (5 min)
    private float gameCountDown = 300f;
    [SerializeField] GameObject gameTimerPrefab;
    private static string gameCountDownStr;
    private bool isAlarmPlaying = false;

    [SerializeField] GameObject cam;
    [SerializeField] GameObject camHolder;
    [SerializeField] UnityEvent onWin;
    private bool inTransition = false;

    private void Awake()
    {
        this.transform.position = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        bossTimerPrefab = GameObject.Find(BossTimerName);
        GameTimerCreator();
    }

    //Update is called once per frame
    void Update()
    {
        // Update game timer 
        gameCountDown -= Time.deltaTime;
        if (gameCountDown >= 0)
        {
            gameCountDownStr = TimeToString(gameCountDown);
        }

        // Update boss timer 
        bossCountDown -= Time.deltaTime;
        if (bossCountDown < 0)
        {

            if (GameObject.Find("Boss").GetComponent<Animator>().GetBool("outTheCage") == false)
            {
                // Update on the boss status (i.e. animation) when the boss is out the cage 
                GameObject.Find("Boss").GetComponent<BossScript>().OutTheCage();
                // Initialize 2nd round enemy 
                GameObject.Find("EnemyCreator").GetComponent<EnemyCreator>().InitiateEnemy();
            }

            GameObject cage = GameObject.Find("Cage");
            if (cage != null)
            {
                cage.transform.Translate(new Vector3(0,1,0));
                if (cage.transform.position.y > 150)
                {
                    Destroy(cage);
                }
            }

        }
        else
        {
            bossCountDownStr = TimeToString(bossCountDown);
            bossTimerPrefab.GetComponent<TextMeshPro>().text = bossCountDownStr;
        }

        if (!isAlarmPlaying && bossCountDown < 0)
        {
            isAlarmPlaying = true;
            GameObject.Find("alarmaudio").GetComponent<AudioSource>().Play();
        }

        if (bossCountDown < 1f && bossCountDown > -4f)
        {
            inTransition = true;
            cam.SetActive(true);
            camHolder.SetActive(true);
            cam.transform.Translate(Vector3.back * Time.deltaTime * 3f);
        }
        else if (bossCountDown < -4f)
        {
            cam.SetActive(false);
            camHolder.SetActive(false);
            inTransition = false;
        }

        if (gameCountDown <= 0.0f)
        {
            onWin.Invoke();
        }
    }

    public bool getinTransitionStatus() {
        return inTransition;
    }

    public bool bossEarlier() {
        if (bossCountDown > 20f) {
            bossCountDown -= 10f;
            return true;
        }
        return false;
    }

    void GameTimerCreator()
    {
        // Initialize a list of positions that will for the game count downs

        Instantiate(gameTimerPrefab, new Vector3(40,-2,181),
            Quaternion.Euler(new Vector3(0f, 0f, 0f)), this.transform);

        Instantiate(gameTimerPrefab, new Vector3(88.5f, 0.7f, 73f),
            Quaternion.Euler(new Vector3(0f, 0f, 0f)), this.transform);

        Instantiate(gameTimerPrefab, new Vector3(22f, 5.65f, -50f),
            Quaternion.Euler(new Vector3(-90f, 90f, 0f)), this.transform);

        Instantiate(gameTimerPrefab, new Vector3(-44f, 0.5f, 43.45f),
            Quaternion.Euler(new Vector3(0f, 180f, 0f)), this.transform);

        Instantiate(gameTimerPrefab, new Vector3(-1.05f, 7f, 140f),
            Quaternion.Euler(new Vector3(0f, -90f, 0f)), this.transform);

        Instantiate(gameTimerPrefab, new Vector3(52.4f, 17f, 150f),
            Quaternion.Euler(new Vector3(0f, 90f, 0f)), this.transform);

    }

    public string TimeToString(float numSeconds)
    {
        int numMinutes = (int)numSeconds / 60;
        int remainSeconds = (int)numSeconds % 60;
        return string.Format("{0:D2} : {1:D2}", numMinutes, remainSeconds);
    }

    public static string GetGameCountDownStr()
    {
        return gameCountDownStr; 
    }

}
