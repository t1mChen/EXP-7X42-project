using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GunController : MonoBehaviour
{
    // For Fire
    [SerializeField] gunCollisionHandler gunCollision;
    [SerializeField] Transform bulletStartPoint;
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletStartSpeed;
    private float fireTimer;
    [SerializeField] float fireInterval;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Light pointLight;

    // For recoil
    [SerializeField] Camera playerCam;
    private CameraRecoil recoilScript;

    // For bullet holes
    public GameObject hole;
    public GameObject glasshole;

    // For bullet counts
    public int maxBullet = 30;   // number of bullets initially in gun
    private int bulletLeft = 100;   // number of bullets in backpack
    public int currBullet = 30;   // number of bullets remaining in gun
    [SerializeField] GameObject cube;
    private gunRotator rotator;
    
    [Header("UISetting")]
    public TextMeshProUGUI RemainingBulletUI;
    public TextMeshProUGUI BulletInGunUI;
    public Image CrosshairUI;

    private GameObject timeManager;
    private bool transit = false;
    private pauseUI pause;
    private bool ispaused = false;


    // Start is called before the first frame update
    void Start()
    {
        currBullet = maxBullet;
        UpdateBulletUI();
        rotator = cube.GetComponent<gunRotator>();
        // Get the recoil script
        recoilScript = transform.Find("CameraHolder/CameraRecoil").GetComponent<CameraRecoil>();
        
        timeManager = GameObject.Find("TimerManager");
        pause = GameObject.Find("pauseUIHolder").GetComponent<pauseUI>();
    }

    // Update is called once per frame
    void Update()
    {
        transit = timeManager.GetComponent<TimerManagerScript>().getinTransitionStatus();
        ispaused = pause.getPauseState();
        if (Input.GetKeyDown(KeyCode.R) && (currBullet < maxBullet) && (bulletLeft > 0))
        {
            reload();
        }
        if (!transit&&!ispaused)
        {
            openFire();
        }
    }

    // Set up logic for firing
    void openFire()
    {
        if (Input.GetMouseButton(0) && (rotator.GetIsRotating() > 0))
        {
            fire();
        } else 
        {
            pointLight.enabled = false;
        }

        if (fireTimer < fireInterval)
        {
            fireTimer += Time.deltaTime;
        }
    }

    public void reload()
    {
        rotator.rotateforward();
        GameObject.Find("reload").GetComponent<AudioSource>().Play();
        StartCoroutine(WaitForReload());
        
    }

    private IEnumerator WaitForReload()
    {
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        rotator.rotateUp();
        int bulletLoaded = maxBullet - currBullet;
        if (bulletLeft >= bulletLoaded) {
            bulletLeft -= bulletLoaded;
            currBullet += bulletLoaded;
        } else {
            currBullet += bulletLeft;
            bulletLeft = 0;
        }
        UpdateBulletUI();
    }

    public void OnBulletPicked()
    {
        bulletLeft += 10;
        UpdateBulletUI();
    }
    public void fire() 
    {
        // If not reached firing interval, or no bullets left in gun, do not fire
        if (fireTimer < fireInterval || currBullet <= 0)
        {
            return;
        }
        // Fire bullet
        recoilScript.Recoil();
        muzzleFlash.Play();
        pointLight.enabled = true;
        RaycastHit hit;

        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 100)) 
        {
            // Detected hitpoint
            Vector3 targetPoint = hit.point;

            // Instantiate bullet
            Vector3 shootDirection = (targetPoint - bulletStartPoint.position).normalized;
            GameObject newBullet = Instantiate(bullet);

            // Shoot the bullet    
            newBullet.transform.position = bulletStartPoint.position;
            newBullet.transform.rotation = Quaternion.LookRotation(shootDirection);
            newBullet.GetComponent<Rigidbody>().velocity = shootDirection * bulletStartSpeed;

            // New bullet fired, so decrement number of bullets in gun and reset time since last fire
            this.currBullet --;
            fireTimer = 0;

            // Produce bullet holes
            if (hit.collider.tag == "glass")
            {
                Instantiate(glasshole, targetPoint + (hit.normal * 0.001f), Quaternion.FromToRotation(Vector3.up, hit.normal),hit.collider.gameObject.transform);
            }else if (hit.collider.tag == "structure") {
                Instantiate(hole, targetPoint + (hit.normal * 0.001f), Quaternion.FromToRotation(Vector3.up, hit.normal), hit.collider.gameObject.transform);
            }

            // Update bullet UI
            UpdateBulletUI();
        }   
    }

    // Method for updating UI on bullet number
    public void UpdateBulletUI()
    {
        RemainingBulletUI.text = "/" + bulletLeft.ToString();
        BulletInGunUI.text = currBullet.ToString();
    }

    public void addFireInterval(float i) {
        fireInterval += i;
    }

    public void addClipLoad()
    {
        maxBullet = 60;
    }
}
