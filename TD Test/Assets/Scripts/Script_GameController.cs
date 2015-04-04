using UnityEngine;
using System.Collections.Generic;


//drives the game
public class Script_GameController : MonoBehaviour {

    public GameObject baseEnemy; //the prefab for the test enemy class.

    public GameObject ghostBaseTower; //the prefab for the test tower placement ghost.

    public GameObject SpawnRoad; //the road where the enemy will spawn.
    public Vector3 spawnMove; //this is the direction entities will move upon spawning.

    public bool building = false; //whether or not the cursor is currently placing a tower.

    public float wavePause = 3.0f; //time between waves.
    float waveTimer = 0;

    List<GameObject> towers = new List<GameObject>(); //a list of towers in the game.
    List<GameObject> enemies = new List<GameObject>(); //a list of enemies in the game.
    GameObject[] ground;

	// Use this for initialization
	void Start ()
    {
        ground = GameObject.FindGameObjectsWithTag("Ground");
        Script_Road spawnRoadTile = SpawnRoad.GetComponent<Script_Road>();
        if (spawnRoadTile != null)
        {
            spawnRoadTile.FindNext(spawnMove);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

        waveTimer -= Time.deltaTime;

        if (waveTimer <= 0)
        {
            SpawnBaseEnemy(); //spawns a test dummy.
            waveTimer = wavePause;
        }
      
        if (Input.GetKey("1")) //if 1 key is pressed and there isn't already a tower being built...
        {
            if (!building)
            {
                building = true; //sets building to true, creates placement ghost.
                Instantiate(ghostBaseTower);
            }
        }
        if (Input.GetKey("2"))
        {
            DestroyTowers();
        }
	}


    //returns a list of enemies.
    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    public List<GameObject> GetTowers()
    {
        return towers;
    }

    //creates a basic enemy.
    void SpawnBaseEnemy()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        if (SpawnRoad != null)
        {
            spawnPos = SpawnRoad.transform.position; //creates position hovering over origin.
            spawnPos.y += baseEnemy.GetComponent<Script_Enemy_Move>().aboveHeight;
        }
        Quaternion spawnRot = Quaternion.identity; //creates default facing.

        GameObject newEnemy; //creates a new enemy.
        newEnemy = Instantiate(baseEnemy, spawnPos, spawnRot) as GameObject;
        enemies.Add(newEnemy); //adds the enemy to the enemy list.
    }


    void DestroyTowers()
    {
        for (int i = 0; i < ground.Length; i++)
        {
            Script_Ground groundScript = ground[i].GetComponent<Script_Ground>();
            if (groundScript != null)
            {
                Destroy(groundScript.tower);
                groundScript.tower = null;
            }
        }
    }

}
