using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject obstaclePrefab;  //Old
    public GameObject[] obstaclePrefabs;  //New for rng obstacles

    public Vector3 spawnPos = new(25, 0, 0);

    public float startDelay = 2;
    public float repeatRate = 2;

    private PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Instantiate(obstaclePrefab, new Vector3(25, 0, 0), obstaclePrefab.transform.rotation);

        InvokeRepeating(nameof(SpawnObstacle), startDelay, repeatRate);


        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void SpawnObstacle()
    {
        if (playerController.gameOver) 
        {
            return;
        }

        if (obstaclePrefabs.Length > 0) // check if array is empty
        {
            int RandomObstacle = Random.Range(0, obstaclePrefabs.Length);

            if (obstaclePrefabs[RandomObstacle]) // check index isn't empty
            {
                Instantiate(obstaclePrefabs[RandomObstacle], spawnPos, obstaclePrefabs[RandomObstacle].transform.rotation);
            }
            else 
            {
                Debug.Log("obstaclePrefabs Index " + RandomObstacle + " is empty");
            }
        }
        else 
        {
            Debug.Log("obstaclePrefabs is empty");
        }

        
        
    }
}
