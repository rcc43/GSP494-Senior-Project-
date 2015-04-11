using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


//drives the game
public class Script_GameController : MonoBehaviour {

    public GameObject[] enemyRoster;

    public GameObject[] towerGhostPrefabs; //the prefab for the test tower placement ghost.
    public GameObject[] towerPrefabs;

    public GameObject SpawnRoad; //the road where the enemy will spawn.
    public GameObject[] flyerSpawns; //locations that flying units will spawn.
    public Vector3 spawnMove; //this is the direction entities will move upon spawning.

    public GameObject Base; //the player's base.

    public bool building = false; //whether or not the cursor is currently placing a tower.

    public float[] wavePause = {1, 3, 5}; //time between waves.
    float[] waveTimer = {0, 0, 0};

    List<GameObject> towers = new List<GameObject>(); //a list of towers in the game.
    List<GameObject> enemies = new List<GameObject>(); //a list of enemies in the game.
    GameObject[] ground;

    public GUISkin basicSkin;
    public GUISkin buffedSkin;

    GameObject selectedEnemy;

    //UI Elements;

    public Canvas UICanvas;
    public GameObject baseBar;
    GameObject sidebar;
    GameObject bottomBar;
    public GameObject healthBar_prefab;
    GameObject healthBar;

    int BuildButtons_X;
    public int BuildButtons_StartY;
    public int BuildButtons_Space;

    int towerBuildButton_Width;
    public int towerBuildButton_Height;

    public int sidebar_width;
    public int sidebar_height;

    public int bottom_width;
    public int bottom_height;

    public int sideButton_offset;

    public int healthBar_width;
    public int healthBar_height;
    public int healthBar_offset;

    public GameObject towerBuildButton_prefab;
    GameObject[] towerBuildButton;

	// Use this for initialization
	void Start ()
    {
        ground = GameObject.FindGameObjectsWithTag("Ground");
        Script_Road spawnRoadTile = SpawnRoad.GetComponent<Script_Road>();
        if (spawnRoadTile != null)
        {
            spawnRoadTile.FindNext(spawnMove);
        }

        sidebar = Instantiate(baseBar) as GameObject;
        RectTransform sideTrans = sidebar.GetComponent<RectTransform>();
        sideTrans.SetParent(UICanvas.transform);
        sideTrans.sizeDelta = new Vector2(sidebar_width, sidebar_height);
        sideTrans.anchoredPosition = new Vector2(Screen.width / 2 - (sidebar_width / 2) + 10 , 0.0f);

        bottomBar = Instantiate(baseBar) as GameObject;
        RectTransform botTrans = bottomBar.GetComponent<RectTransform>();
        botTrans.SetParent(UICanvas.transform);
        botTrans.sizeDelta = new Vector2(bottom_width, bottom_height);
        botTrans.anchoredPosition = new Vector2(0.0f, -(Screen.height / 2) + (bottom_height / 2) - 10);

        healthBar = Instantiate(healthBar_prefab) as GameObject;
        RectTransform healthTrans = healthBar.GetComponent<RectTransform>();
        healthTrans.SetParent(UICanvas.transform);
        healthTrans.sizeDelta = new Vector2(healthBar_width, healthBar_height);
        healthTrans.anchoredPosition = new Vector2(0 - healthBar_offset, -(Screen.height / 2) + (bottom_height / 2) - 10);

        Script_Base baseData = Base.GetComponent<Script_Base>();
        baseData.healthBar = healthBar;

        towerBuildButton = new GameObject[towerPrefabs.Length];

        towerBuildButton_Width = sidebar_width - sideButton_offset;
        BuildButtons_X = Screen.width / 2 - (sidebar_width / 2) + 10;

        waveTimer = new float[3];
        waveTimer[0] = 1;
        waveTimer[0] = 2;
        waveTimer[0] = 4;

        UpdateButtons();
	}
	
	// Update is called once per frame
	void Update ()
    {
        waveTimer[0] -= Time.deltaTime;
        waveTimer[1] -= Time.deltaTime;
        waveTimer[2] -= Time.deltaTime;

        if (waveTimer[0] <= 0)
        {
            SpawnBaseEnemy(); //spawns a test dummy.
            SpawnIcarus();
            waveTimer[0] = wavePause[0];
        }

        if (waveTimer[1] <= 0)
        {
            SpawnChiron();
            waveTimer[1] = wavePause[1];
        }

        if (waveTimer[2] <= 0)
        {
            SpawnHercules();
            waveTimer[2] = wavePause[2];
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
            spawnPos.y += enemyRoster[0].GetComponent<Script_Enemy_Move>().aboveHeight;
        }
        
        Quaternion spawnRot = Quaternion.identity; //creates default facing.

        GameObject newEnemy; //creates a new enemy.
        newEnemy = Instantiate(enemyRoster[0], spawnPos, spawnRot) as GameObject;
        enemies.Add(newEnemy); //adds the enemy to the enemy list.
    }

    void SpawnHercules()
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
    }

    void SpawnChiron()
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
    }

    void SpawnIcarus()
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
    }


    public void ResetLevel()
    {
        DestroyEnemies();
        DestroyTowers();
    }

    void DestroyEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i]);
        }
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

    public void BuildTower(GameObject type)
    {
        if (!building)
        {
                building = true; //sets building to true, creates placement ghost.
                Instantiate(type);
        }
    }


    public void UpdateButtons()
    {
        for (int i = 0; i < towerPrefabs.Length; i++ )
        {
            if (towerBuildButton[i] == null)
            {
                RectTransform buttonTrans;
                towerBuildButton[i] = Instantiate(towerBuildButton_prefab) as GameObject;
                towerBuildButton[i].transform.SetParent(UICanvas.transform, false);
                buttonTrans = towerBuildButton[i].GetComponent<RectTransform>();
                if (buttonTrans != null)
                {
                    buttonTrans.sizeDelta = new Vector2(towerBuildButton_Width, towerBuildButton_Height);
                    buttonTrans.anchoredPosition = new Vector2(BuildButtons_X, BuildButtons_StartY - (i * (towerBuildButton_Height + BuildButtons_Space)));
                }
                Script_TowerBuildButton buttonScript = towerBuildButton[i].GetComponent<Script_TowerBuildButton>();
                if (buttonScript != null)
                {
                    buttonScript.UpdateData(towerPrefabs[i].GetComponent<Script_Tower>());
                    buttonScript.representedTower_Prefab = towerPrefabs[i];
                }
                Button button = towerBuildButton[i].GetComponent<Button>();
                if (button != null)
                {
                    GameObject buildPrefab = towerGhostPrefabs[i];
                    button.onClick.AddListener(() => { BuildTower(buildPrefab); });
                }
            }
        }
    }
}
