//-------------------------------------------------------------------------------------
// This program will spawn a random array of enemy fish gameobjects randomly on the Y 
// axis evey 10 seconds with a spawnTimer variable. The spawnTimer variable will be 
// decreased every 8 seconds by .2 seconds untill a random fish is spawned every
// second.
//-------------------------------------------------------------------------------------

using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    private Vector2 spawnPosition; // Vector to store the spawn position
    private float spawnMaxY = 7.5f, spawnMinY = -3.5f; // variable to store the max and minimum y position

    public GameObject[] fish; // public array of fish game objects

    private float spawnTimer; // 
    private float reduceSpawnTimeTimer; // timer variable to reduce the spawn timer
    private float reduceSpawnTime; // amount to reduce thespawn timer
    private float reduceSpawnTimeMax; // maximum amount we want to reduce the spawn timer

    private int spawnRandomFish; // random value for the fish array

 
    void Awake()
    {
        // Getting this transforms position and storing it in the spawnPosition
        spawnPosition = transform.position;
        // initializing the time and timer variables
        spawnTimer = 10f;
        reduceSpawnTimeTimer = 8f;
        reduceSpawnTime = 0f;
        reduceSpawnTimeMax = spawnTimer - 1.2f;
    }


    void Update()
    {
        // If the reduceSpawnTimeTimer variable is less than 0 and the reduceSpawnTime is less 
        // the max amount of time we want to reduce the spawnTimer variable
        reduceSpawnTimeTimer -= Time.deltaTime;
        if (reduceSpawnTimeTimer < 0 && reduceSpawnTime < reduceSpawnTimeMax)
        {
            // reset the reduceSpawnTimeTimer variable and increase the amount of time we want
            // subtract from the span timer.
            reduceSpawnTimeTimer = 8f;
            reduceSpawnTime += .2f;
        }
       
        // if the spawntimer is less than 0
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            // Instantiate a random fish between a random range on the y axis           
            spawnRandomFish = Random.Range(1, 20);
            spawnPosition.y = Random.Range(spawnMaxY, spawnMinY);
            Instantiate(fish[spawnRandomFish], spawnPosition, transform.transform.rotation);
            // reset a reduce the spawnTimer
            spawnTimer = 10f - reduceSpawnTime;
        }
        //if (Player_Movement.hasWon == true)
        //{
        //    Destroy(gameObject);
        //}

    }
}
