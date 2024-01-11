using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.AI.Navigation;
using UnityEngine.AI;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;

public class CrateManager : MonoBehaviour
{
    public struct Pair
    {
        public Vector3 position;
        public bool isGenerated;
    }
    [SerializeField] GameObject prefab;
    private bool isReady;
    private int numCrates;
    List<Vector3> randomPoints;
    List<bool> isGenerated;
    private float timer;
    GameObject[] crateSpawnPoints;
    private int numCratesThreshold;
    void Start()
    {

        numCratesThreshold = 15;
        randomPoints = new List<Vector3>();
        isGenerated = new List<bool>();
        collect();
        timer = 0;
        numCrates = 0;

        // initialize 30 crates

        StartCoroutine(generateCratePeriodically());
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 30)
        {

            timer = 0;
            if (numCrates < numCratesThreshold)
            {
                StartCoroutine(generateCratePeriodically());
            }
        }

    }


    private IEnumerator generateCratePeriodically()
    {
        yield return new WaitForSeconds(0.7f);
        while (numCrates < numCratesThreshold) {
            generateCrate();
        }
        
    }

    private void generateCrate()
    {
        int randomNum = Random.Range(0, randomPoints.Count - 1);
        if (!isGenerated[randomNum])
        {
            Vector3 pickupGeneratePoint = randomPoints[randomNum];

            isGenerated[randomNum] = true;

            numCrates++;

            OpenCrate crate = Instantiate(prefab, pickupGeneratePoint, Quaternion.identity, this.transform).GetComponent<OpenCrate>();
            crate.setupStartPosition(pickupGeneratePoint);
        }
    }


    public void updateNumCrates()
    {
        numCrates--;
    }

    

    // used to collect all the location of floors that are able to land crates
    public void collect()
    {
        crateSpawnPoints = GameObject.FindGameObjectsWithTag("CrateSpawnPoint");
        for (int i = 0; i < crateSpawnPoints.Length; i++)
        {
            Vector3 spawnPoint = crateSpawnPoints[i].transform.position;
            randomPoints.Add(spawnPoint);
            isGenerated.Add(false);
        }



    }
    
    public void updateCrateStateOnDestroy(Vector3 position)
    {
        updateNumCrates();
        int index = randomPoints.FindIndex(item=>item == position);
        isGenerated[index] = false;
    }

    public void lessCrates()
    {
        numCratesThreshold -= 3;
    }

}
