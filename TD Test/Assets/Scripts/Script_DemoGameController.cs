using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Script_DemoGameController : MonoBehaviour
{
    public GameObject[] enemyRoster;

    public GameObject[] towerGhostPrefabs; //the prefab for the test tower placement ghost.
    public GameObject[] towerPrefabs;

    public GameObject SpawnRoad; //the road where the enemy will spawn.
    public GameObject[] flyerSpawns; //locations that flying units will spawn.
    public Vector3 spawnMove; //this is the direction entities will move upon spawning.

    public GameObject PauseMenu;

    public GameObject Base; //the player's base.

    public bool building = false; //whether or not the cursor is currently placing a tower.
    public bool paused = false;
    public float Resources;

    bool insufficentResources;
    public float resourcesFlashTime;
    float resourcesFlashCounter = 0;

    int waveNum;
    public int startWaveSize;

    float timeToWave = 0.0f;

    public float startGamePause = 20.0f;
    public float inFormationPause = .5f;
    public float betweenFormationPause = 1.0f;
    public float betweenWavePause = 10.0f;

    public float defeatTimer = 0.0f;
    public bool defeated = false;

    public List<FormationBlueprint> spawnQueue = new List<FormationBlueprint>();

    List<GameObject> towers = new List<GameObject>(); //a list of towers in the game.
    List<GameObject> enemies = new List<GameObject>(); //a list of enemies in the game.
    GameObject[] ground;

    public GUISkin basicSkin;
    public GUISkin buffedSkin;
    public GUISkin debuffSkin;

    GameObject selectedEnemy;

    public string mainMenuLevel;


    // Use this for initialization
    void Start()
    {

        ground = GameObject.FindGameObjectsWithTag("Ground");
        Script_Road spawnRoadTile = SpawnRoad.GetComponent<Script_Road>();
        if (spawnRoadTile != null)
        {
            spawnRoadTile.FindNext(spawnMove);
        }

        waveNum = 0;

        FormationLedger.Init();

        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        timeToWave -= Time.deltaTime;
        if (resourcesFlashCounter >= 0)
        {
            resourcesFlashCounter -= Time.deltaTime;
        }
        else
        {
            insufficentResources = false;
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
    GameObject SpawnBaseEnemy()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        if (SpawnRoad != null)
        {
            spawnPos = SpawnRoad.transform.position; //creates position hovering over origin.
            spawnPos.y += enemyRoster[0].GetComponent<Script_Enemy_Move>().aboveHeight;
        }

        Quaternion spawnRot = Quaternion.identity; //creates default facing.

        GameObject newEnemy; //creates a new enemy.
        newEnemy = Instantiate(enemyRoster[0], spawnPos, spawnRot) as GameObject;
        enemies.Add(newEnemy); //adds the enemy to the enemy list.
        return newEnemy;
    }

    GameObject SpawnHercules()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        if (SpawnRoad != null)
        {
            spawnPos = SpawnRoad.transform.position; //creates position hovering over origin.
            spawnPos.y += enemyRoster[2].GetComponent<Script_Enemy_Move>().aboveHeight;
        }

        Quaternion spawnRot = Quaternion.identity; //creates default facing.

        GameObject newEnemy; //creates a new enemy.
        newEnemy = Instantiate(enemyRoster[2], spawnPos, spawnRot) as GameObject;
        enemies.Add(newEnemy); //adds the enemy to the enemy list.
        return newEnemy;
    }

    GameObject SpawnChiron()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        if (SpawnRoad != null)
        {
            spawnPos = SpawnRoad.transform.position; //creates position hovering over origin.
            spawnPos.y += enemyRoster[3].GetComponent<Script_Enemy_Move>().aboveHeight;
        }

        Quaternion spawnRot = Quaternion.identity; //creates default facing.

        GameObject newEnemy; //creates a new enemy.
        newEnemy = Instantiate(enemyRoster[3], spawnPos, spawnRot) as GameObject;
        enemies.Add(newEnemy); //adds the enemy to the enemy list.
        return newEnemy;
    }

    GameObject SpawnIcarus()
    {
        Vector3 spawnPos = new Vector3(0, 0, 0);
        if (flyerSpawns != null)
        {
            int spawnSpot = Random.Range(0, flyerSpawns.Length);
            spawnPos = flyerSpawns[spawnSpot].transform.position; //creates position hovering over origin.
            spawnPos.y += enemyRoster[1].GetComponent<Script_Enemy_Move>().aboveHeight;
        }
        Quaternion spawnRot = Quaternion.identity; //creates default facing.

        GameObject newEnemy; //creates a new enemy.
        newEnemy = Instantiate(enemyRoster[1], spawnPos, spawnRot) as GameObject;
        enemies.Add(newEnemy); //adds the enemy to the enemy list.
        return newEnemy;
    }


    void DestroyEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i]);
        }
        enemies.Clear();
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
        towers.Clear();
    }

    public void BuildTower(GameObject type)
    {
        if (!building)
        {
            building = true; //sets building to true, creates placement ghost.
            Instantiate(type);
        }
    }


    void FormulateWave()
    {
        spawnQueue.Clear();
        int attackSize = startWaveSize + waveNum + Random.Range(3, 10);
        for (int i = 0; i < attackSize; i++)
        {
            spawnQueue.Add(FormationLedger.formationBlueprints[Random.Range(0, FormationLedger.formationBlueprints.Count)]);
        }
    }

    IEnumerator SpawnFormation(FormationBlueprint blueprint)
    {
        Formation formation = new Formation();
        formation.Init(blueprint.spawnList.Count);

        for (int i = 0; i < blueprint.spawnList.Count; i++)
        {
            switch (blueprint.spawnList[i])
            {
                case enemyType.standard:
                    {
                        formation.members[i] = SpawnBaseEnemy();
                        break;
                    }
                case enemyType.tank:
                    {
                        formation.members[i] = SpawnHercules();
                        break;
                    }
                case enemyType.healer:
                    {
                        formation.members[i] = SpawnChiron();
                        break;
                    }
                case enemyType.flyer:
                    {
                        formation.members[i] = SpawnIcarus();
                        break;
                    }
            }
            //scales up health/healing with level.
            float healthFactor = waveNum * .1f + 1.0f;
            Script_Enemy_Health memHealth = formation.members[i].GetComponent<Script_Enemy_Health>();
            memHealth.SetHealth(memHealth.maxHealth * healthFactor);
            memHealth.resourceYield *= healthFactor;
            Script_AreaEffect area = formation.members[i].GetComponent<Script_AreaEffect>();
            if (area != null)
            {
                area.buff.magnitude *= healthFactor;
            }
            Script_Enemy_Move memMove = formation.members[i].GetComponent<Script_Enemy_Move>();
            memMove.formation = formation;
            yield return new WaitForSeconds(inFormationPause);
        }
        yield return new WaitForSeconds(betweenFormationPause);
    }

    IEnumerator SpawnWave()
    {
        waveNum++;
        FormulateWave();
        yield return new WaitForSeconds(betweenWavePause);
        for (int i = 0; i < spawnQueue.Count; i++)
        {
            StartCoroutine(SpawnFormation(spawnQueue[i]));
            yield return new WaitForSeconds((spawnQueue[i].spawnList.Count * inFormationPause) + betweenFormationPause);
        }
    }

    IEnumerator StartGame()
    {
        timeToWave = startGamePause;
        yield return new WaitForSeconds(startGamePause);
        while (true)
        {
            Debug.Log("Starting Wave " + (waveNum + 1).ToString());
            StartCoroutine(SpawnWave());
            float waveLength = CalculateWaveLength();
            timeToWave = waveLength;
            yield return new WaitForSeconds(waveLength);
        }
    }

    float CalculateWaveLength()
    {
        float length = 0;
        for (int i = 0; i < spawnQueue.Count; i++)
        {
            length += (spawnQueue[i].spawnList.Count * inFormationPause) + betweenFormationPause * 1.5f;
        }
        length += betweenWavePause;
        return length;
    }

    public bool EnoughResources(float cost)
    {
        if (Resources - cost >= 0)
        {
            Resources -= cost;
            return true;
        }
        else
        {
            insufficentResources = true;
            resourcesFlashCounter = resourcesFlashTime;
            return false;
        }
    }
}
