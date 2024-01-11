using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] GameObject enemyFastPrefeb;
    [SerializeField] GameObject enemySlowPrefeb;

    public const string DeadTag = "Dead";

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = Vector3.zero;
        InitiateEnemy(); 
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(GameObject.FindGameObjectWithTag(DeadTag), 3.5f);
    }

    public void InitiateEnemy()
    {
        // Set the position and rotation of the enemy 
        for (int i = 0; i < 2; i++) { RandomlySpawnAt(1f, 18f, -4.5f, 124f, 178f); } // boss right
        for (int i = 0;i < 2; i++) { RandomlySpawnAt(28f, 43f, -4.5f, 124f, 178f); } // boss left
        RandomlySpawnAt(44f, 50f, 4.5f, 125f, 140f); // boss 2nd floor 
        RandomlySpawnAt(1.2f, 14f, 4.5f, 123f, 130f); // boss 2nd floor 
        RandomlySpawnAt(1.2f, 12f, 14f, 125f, 130f); // boss 3rd floor 
        RandomlySpawnAt(28f, 49f, 14f, 171f, 178f); // boss 3rd floor 

        for (int i = 0; i < 2; i++) { RandomlySpawnAt(38f, 48f, 7f, 89f, 91.5f); } // fuel room

        for (int i = 0; i < 2; i++) { RandomlySpawnAt(16f, 29f, 1.25f, 31f, 40f); } // bridge room 

        RandomlySpawnAt(61f, 69f, -4.5f, 20.2f, 23.7f); // boss left corner room

        for (int i = 0; i < 2; i++) { RandomlySpawnAt(-35f, -31f, 1.25f, 48f, 68f); } // boss right small room

    }

    private void RandomlySpawnAt(float minX, float maxX, float y, float minZ, float maxZ)
    {
        // Randomly choose fast or slow walking enemy 
        GameObject prefab; 
        int randomNum = Random.Range(0, 100);
        if (randomNum < 50) { prefab = enemySlowPrefeb; }
        else { prefab = enemyFastPrefeb; }

        // Instantiate the enemy based on given position range 
        Instantiate(prefab, new Vector3(Random.Range(minX, maxX), y, Random.Range(minZ, maxZ)),
            Quaternion.Euler(new Vector3(0f, Random.Range(0, 365), 0f)), this.transform); 
    }
}
