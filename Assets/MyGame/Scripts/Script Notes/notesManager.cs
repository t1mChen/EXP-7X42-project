using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using System;
using TMPro;

public class notesManager : MonoBehaviour
{

    private string note01 = "Experiment Report 344: \n\n" +
        "Topic: Self adaptive Evolution\n\n" +
        "Subject: sample 600 \n\n" +
        "Finding: No signs of evolution observed, even as we push the difficulty to its limits\n\n" +
        "Status: Failed \n\n" +
        "Result: Executed \n\n" +
        "Conclusion: Further experiments are warranted\n\n" +
        "Suggestion: expand the environment and conduct more sample tests\n\n" +
        "Perfomance Details: the s... (Deleted)\n\n";
    private string note02 = "7fuw032t()%#)@#H%#_(Memory Leak___" +
        "New Message: \n\n" +
        "Time: 12:24\n\n" +
        "Today, the first tested object has passed the test\n\n" +
        "It is a miracle. The subject is invaluable, we must not tell it anything\n\n";
    private string note03 = "\n\n\nSave yourself in the real world\n\n\n";
    private string note04 = "There is no exit to this place\n\n unless you escape your mind...\n\n";
    private string note05 = "Do not trap yourself, save yourself\n\n" +
                        "Leave here after the 5:00 countdown\n\n corporation here will not serve you well\n\n";
    private string note06 = "You cannot kill the boss,\n\n"+"It is programmed to kill you\n\n";
    private string note07 = "You are the real one being tested...\n\n";

    private string note08 = "Profile:7X42\n\n\n" +
        "Agility: 20\n\n" +
        "expected chance: 0.01%\n\n" +
        "difficulty: highest\n\n";
    private string note09 = "... in the previous round, 7X42 has performed with excellency\n\n"+
        "We must stop it from bringing the knowledge to the public...\n\n";
    private string note10 = "This factory is built by a program from our supercomputers\n";
    private string note11 = "Loss of our best guinea pig is regrettable... but 2:30 is the final limit, we must shut it down\n\n" +
                            "Release the INVINCIBLE";
    private string note12 = "We must not let it know the truth of experiment\n\n";
    private string note13 =
        "We need to expand our )(&#$(%(#&%, and secretly monitor this sample\n\n" +
        "and to program a even more hostile creature to &*R#((@#NRNJ\n\n" +
        "(This message is confidential)\n\n two T**(Y#(*#)@$)(%&*#\n";
    private string note14 ="\nThe boss will summon all minotaurs\n";
    private string note15 = "no time to explain... collect as much as you can! you need resources to save yourse...\n\n";

    List<string> notesList = new List<string>();

    [SerializeField] GameObject buffCanvas;
    [SerializeField] GameObject timerManager;
    [SerializeField] GameObject boss;
    GameObject player;
    GameObject crateManager;

    private bool displayOn = false;
    private float buffer = 0f;

    public enum Buff
    {
        [Description("Minotaurs are enhanced")]
        enemyBloodp,
        [Description("Minotaurs are weakened")]
        enemyBloodm,
        [Description("Boss is SPEEDING UP")]
        bossFaster,
        [Description("Boss is slowing down")]
        bossSlower,
        [Description("Boss will be released earlierb")]
        bossEarlier,
        [Description("Number of crates is deducted")]
        lessCrates,
        [Description("Your speed is boosted")]
        fasterPlayer,
        [Description("Your are slowed ...")]
        slowerPlayer,
        [Description("Super Gravity")]
        cannotJump,
        //[Description("Normal Gravity")]
        //canJump,
        [Description("Less Gravity")]
        jumpHigher,
        [Description("Lose Direction")]
        oppositeDirectionMovement,
        [Description("Invisible function founded")]
        invisiblePropAdded,
        [Description("Minotaurs broadening vision")]
        enemyGoodSight,
        [Description("Minotaurs shorten vision")]
        enemyBadSight

    }
    // Start is called before the first frame update
    void Start()
    {
        // inititiate all of the notes into list
        player = GameObject.Find("Player");
        crateManager = GameObject.Find("CrateManager");
       notesList.Add(note01);
       notesList.Add(note02);
       notesList.Add(note03);
       notesList.Add(note04);
       notesList.Add(note05);
       notesList.Add(note06);
       notesList.Add(note07);
       notesList.Add(note08);
       notesList.Add(note09);
       notesList.Add(note10);
       notesList.Add(note11);
       notesList.Add(note12);
       notesList.Add(note13);
       notesList.Add(note14);
       notesList.Add(note15);
       buffCanvas.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (displayOn == true) {
            buffCanvas.GetComponent<Canvas>().enabled = true;
            buffer += Time.deltaTime;
        }

        if (buffer >= 10f)
        {
            buffer = 0.0f;
            displayOn = false;
            buffCanvas.GetComponent<Canvas>().enabled = false;
        }
    }

    public string getNote(int id) {
        return notesList[id-1];
    }

    public Buff randomBuff() {
        int i = UnityEngine.Random.Range(0, Enum.GetValues(typeof(Buff)).Length);
        Buff b = (Buff)Enum.GetValues(typeof(Buff)).GetValue(i);
        return b;
    }

    public void displayBuff(Buff b, bool valid) {
        // these three lines of codes are generated by ChatGPT to save and get descriptions for a enum
        System.Reflection.FieldInfo fieldInfo =
            b.GetType().GetField(b.ToString());
        DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);
        string text = attributes[0].Description;
        if (valid == false) {
            text = "Nothing Happened";
        }
        buffCanvas.transform.Find("buff").GetComponent<TextMeshProUGUI>().text = text;
        //buffCanvas.GetComponentInChildren<TextMeshProUGUI>().text = text;
        displayOn = true;
        
        return;
    }

    public void executeBuff() {
        // randomly generate a buff and carry out relevant actions
        Buff b = randomBuff();
        if (b == Buff.bossEarlier)
        {
            if (timerManager.GetComponent<TimerManagerScript>().bossEarlier())
            {
                displayBuff(b, true);
                return;
            }
        }
        else if (b == Buff.bossFaster)
        {
            if (boss.GetComponent<BossScript>().speedUp())
            {
                displayBuff(b, true);
                return;
            }
        }
        else if (b == Buff.bossSlower)
        {
            if (boss.GetComponent<BossScript>().slowDown())
            {
                displayBuff(b, true);
                return;
            }
        } 
        else if (b == Buff.oppositeDirectionMovement)
        {
            player.GetComponent<PlayerController>().SetReverseWalkingDirection();
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.lessCrates)
        {
            crateManager.GetComponent<CrateManager>().lessCrates();
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.invisiblePropAdded)
        {
            EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in enemies)
            {
                enemy.GetComponent<EnemyScript>().BlindToPlayer();
            }
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.slowerPlayer)
        {
            player.GetComponent<PlayerController>().WalkSlower();
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.fasterPlayer)
        {
            player.GetComponent<PlayerController>().WalkFaster();
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.jumpHigher)
        {
            player.GetComponent<PlayerController>().ApplyLessGravity();
            displayBuff(b, true);
            return;
        }

        else if (b == Buff.cannotJump) {
            player.GetComponent<PlayerController>().ApplySuperGravity();
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.enemyBadSight)
        {
            EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in enemies)
            {
                enemy.GetComponent<EnemyScript>().shortenSight();
            }
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.enemyGoodSight)
        {
            EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in enemies)
            {
                enemy.GetComponent<EnemyScript>().broadenSight();
            }
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.enemyBloodp)
        {
            EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in enemies)
            {
                enemy.GetComponent<EnemyScript>().enhanceEnemy();
            }
            displayBuff(b, true);
            return;
        }
        else if (b == Buff.enemyBloodp)
        {
            EnemyScript[] enemies = FindObjectsOfType<EnemyScript>();
            foreach (EnemyScript enemy in enemies)
            {
                enemy.GetComponent<EnemyScript>().weakerEnemy();
            }
            displayBuff(b, true);
            return;
        }
        displayBuff(b, false);
    }


}
