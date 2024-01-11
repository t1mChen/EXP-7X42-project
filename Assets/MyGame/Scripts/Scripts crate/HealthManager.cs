using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using TMPro;

// the script is used for showing player's health bar and hasn't been completed yet
// would be improved the next stage
[RequireComponent(typeof(MeshRenderer))]

public class HealthManager : MonoBehaviour
{

    [SerializeField] private int startingHealth;
    [SerializeField] UnityEvent onDeath;
    [SerializeField] UnityEvent onRecover;
    [SerializeField] GameObject playerBlood;
    private GameObject timeManager;

    private int CurrentHealth;
    private const int recoverHealth = 20;
    private Image hurtimage;
    private float hurtbuffer = 0.0f;
    private float HURTLIMIT = 0.7f;
    public GameObject healingEffect;
    private GameObject healingPs;

    [Header("UISetting")]
    public TextMeshProUGUI RemainingHealthUI;

    [SerializeField] private AudioSource hitAudio;

    // set player's health
    private void Start()
    {
        startingHealth = 400;
        UpdateHealthUI();
        timeManager = GameObject.Find("TimerManager");
        healingPs = Instantiate(healingEffect, transform).gameObject;
        healingPs.transform.localPosition = new Vector3(0, -2, 0);
        ParticleSystem[] particles = healingPs.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles)
        {
            p.Stop();
        }
            ResetHealthToStarting();
        hurtimage = GameObject.Find("hurtimage").GetComponent<Image>();
        hurtimage.enabled = false;
    }
    public void ApplyHealing()
    {
        StartCoroutine(recoverPeriodically());
        
    }

    private IEnumerator recoverPeriodically()
    {
        healingPs.SetActive(true);
        ParticleSystem[] particles = healingPs.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem p in particles){
            p.Play();
            Debug.Log("ISPLAYING");
        }
        
        
        Debug.Log(healingPs.activeSelf);
        int recoverTimes = 0;
        while (recoverTimes <= 10)
        {
            healingPs.SetActive(true);
            if (CurrentHealth >= startingHealth)
            {
                CurrentHealth = startingHealth;
            }
            yield return new WaitForSeconds(0.7f);
            ApplyRecover();
            recoverTimes += 1;
        }
        healingPs.SetActive(false);
    }
    public void ResetHealthToStarting()
    {
        CurrentHealth = this.startingHealth;
        UpdateHealthUI();
    }

    public void ApplyDamage(int damage)
    {
        if (!timeManager.GetComponent<TimerManagerScript>().getinTransitionStatus())
        {
            hitAudio.Play();
            CurrentHealth -= damage;
            hurtimage.enabled = true;
            hurtbuffer = 0.1f;
            UpdateHealthUI();
        }
    }


    public void ApplyRecover()
    {
        CurrentHealth += recoverHealth/2;
        UpdateHealthUI();
    }

    //the length of health bar shows player's health
    void Update()
    {
        UpdateHealthUI();
        float ratio = ((float)CurrentHealth) / startingHealth;
        playerBlood.GetComponent<RectTransform>().localScale = new Vector3(ratio, 1f, 1f);

        if (CurrentHealth <= 0)
        {
            onDeath.Invoke();
        }
        if (hurtbuffer >= 0.0f)
        {
            hurtbuffer += Time.deltaTime;
        }
        if (hurtbuffer > HURTLIMIT)
        {
            hurtbuffer = 0.0f;
            hurtimage.enabled = false;
        }
    }

    // Method for updating UI on health
    public void UpdateHealthUI()
    {
        RemainingHealthUI.text = CurrentHealth.ToString() + ("/") + startingHealth;
    }

}
