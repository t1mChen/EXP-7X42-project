using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
// used for "storing" and show props for players when they approaches and open
public class OpenCrate : MonoBehaviour
{
    // list of possible props that may appear in crate
    public WeightedRandomList<Transform> lootTable;

    [SerializeField] private Animator animator;
    [SerializeField] private float stepTime;
    public Transform itemHolder;
    private bool firstTimeOpen;
    private GameObject prop;
    private float timer = 0;
    private const float minDistanceOpenCrate = 6;
    private const float destroyCrateTimer = 3;
    private bool startDestroy = false;
    private Vector3 startPosition;
    public AudioClip openCrateSound;
    private bool onDisplay = false;
    void Start()
    {

        // get the component of animation
        animator = GetComponent<Animator>();
        // has the invisible effect to hide things inside the crate
        itemHolder.localScale = Vector3.zero;
        firstTimeOpen = true;
        itemHolder.gameObject.SetActive(false);
        foreach (Transform child in itemHolder)
        {
            child.gameObject.SetActive(false);
        }

        GameObject.Find("openCrate").GetComponent<Canvas>().enabled = false;
        GameObject.Find("closeCrate").GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        // open the crate when player approaches
        GameObject player = GameObject.Find("Player");
        if (Vector3.Distance(player.transform.position, transform.position) < minDistanceOpenCrate)
        {
            if (!onDisplay&&firstTimeOpen) { 
                onDisplay = true;
                GameObject.Find("openCrate").GetComponent<Canvas>().enabled = true;
            }
            if (!onDisplay && !firstTimeOpen)
            {
                onDisplay = true;
                GameObject.Find("closeCrate").GetComponent<Canvas>().enabled = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                if (firstTimeOpen)
                {
                    GameObject.Find("openCrate").GetComponent<Canvas>().enabled = false;
                    GameObject.Find("closeCrate").GetComponent<Canvas>().enabled = true;
                    firstTimeOpen = false;
                    animator.Play("OpenCrate");
                    animator.SetBool("IsOpen", true);
                    ShowItem();
                    GetComponent<AudioSource>().PlayOneShot(openCrateSound);
                }
                //if (firstTimeOpen)
                //{
                //    animator.Play("OpenCrate");
                //    firstTimeOpen = false;
                //}
                //isOpen = !isOpen;
                //if (isOpen)
                //{
                //    animator.SetBool("IsOpen", true);
                //    ShowItem();
                //}
                //// this part would not be executed in the game finally, but remain it for testing first
                //else
                //{
                //    animator.SetBool("IsOpen", false);
                //    HideItem();

                //}
            }
        }
        else {
            if (onDisplay && firstTimeOpen)
            {
                GameObject.Find("openCrate").GetComponent<Canvas>().enabled = false;
                onDisplay = false;
            }
            else if (onDisplay && !firstTimeOpen) {
                GameObject.Find("closeCrate").GetComponent<Canvas>().enabled = false;
                onDisplay = false;
            }

        }


        if (prop.IsDestroyed())
        {
            GameObject.Find("closeCrate").GetComponent<Canvas>().enabled = false;
            startDestroy = true;
            // Destroy Crate after 3 seconds the prop get removed
            if (startDestroy)
            {
                timer += Time.deltaTime;
                if (timer >= destroyCrateTimer)
                {
                    CrateManager crateManager = GameObject.Find("CrateManager").GetComponent<CrateManager>();
                    //crateManager.updateNumCrates();
                    crateManager.updateCrateStateOnDestroy(startPosition);
                    Destroy(gameObject);
                    // ready to regenerate another crate
                    //crateManager.isReadyChange();
                }
            }
        }

    }
    void ShowItem()
    {
        // pop out random props
        itemHolder.localScale = Vector3.one;
        itemHolder.gameObject.SetActive(true);
        Transform item = lootTable.GetRandom();
        foreach (Transform others in itemHolder)
        {
            if (others != item)
            {
                others.gameObject.SetActive(false);
            }
        }
        prop = item.gameObject;


        prop.SetActive(true);


    }
    void HideItem()
    {
        itemHolder.gameObject.SetActive(false);
        itemHolder.localScale = Vector3.zero;

    }
    public void setupStartPosition(Vector3 point)
    {
        this.startPosition = point;
    }

}