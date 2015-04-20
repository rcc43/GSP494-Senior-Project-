using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


//drives the game
public class Script_GameController : MonoBehaviour {

    public bool demo = false;

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
    public int waveIncreaseFactorMin = 3;
    public int waveIncreaseFactorMax = 10;

    public float healthScalingFactor = .2f;
    public float resourceScalingFactor = .1f;

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
    public GUISkin failureSkin;

    GameObject selectedEnemy;

    public string mainMenuLevel;

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

        if (!demo)
        {
            sidebar = Instantiate(baseBar) as GameObject;
            RectTransform sideTrans = sidebar.GetComponent<RectTransform>();
            sideTrans.SetParent(UICanvas.transform);
            sideTrans.sizeDelta = new Vector2(sidebar_width, sidebar_height);
            sideTrans.anchoredPosition = new Vector2(Screen.width / 2 - (sidebar_width / 2) + 10, 0.0f);

            bottomBar = Instantiate(baseBar) as GameObject;
            RectTransform botTrans = bottomBar.GetComponent<RectTransform>();
            botTrans.SetParent(UICanvas.transform);
            botTrans.sizeDelta = new Vector2(bottom_width, bottom_height);
            botTrans.anchoredPosition = new Vector2(0.0f, -(Screen.height / 2) + (bottom_height / 2) - 10);

            healthBar = Instantiate(healthBar_prefab) as GameObject;
            RectTransform healthTrans = healthBar.GetComponent<RectTransform>();
            healthTrans.SetParent(UICanvas.transform);
            //healthTrans.sizeDelta = new Vector2(healthBar_width, healthBar_height);
            healthTrans.anchoredPosition = new Vector2(0 - healthBar_offset, -(Screen.height / 2) + (bottom_height / 2) - 10);
            Script_HealthBar healthBarData = healthBar.GetComponent<Script_HealthBar>();

            Script_Base baseData = Base.GetComponent<Script_Base>();
            baseData.healthBar = healthBar;
            baseData.healthProjection = healthBar.GetComponent<Script_HealthBar>();

            towerBuildButton = new GameObject[towerPrefabs.Length];

            towerBuildButton_Width = sidebar_width - sideButton_offset;
            BuildButtons_X = Screen.width / 2 - (sidebar_width / 2) + 10;

            UpdateButtons();
        }

        waveNum = 0;

        FormationLedger.Init();

        StartCoroutine(StartGame());
	}
	
	// Update is called once per frame
	void Update ()
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

        if (defeated)
        {
            defeatTimer -= Time.deltaTime;
            if (defeatTimer <= 0)
            {
                defeated = false;
                ResetLevel();
            }
        }

        if (!demo)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Pause();
            }
        }
	}

    void OnGUI()
    {
        if (!demo)
        {
            GUI.Label(new Rect(0.0f, 0.0f, 50.0f, 70.0f), "Next wave in: " + timeToWave.ToString("F0"));
            if (insufficentResources == true)
            {
                GUI.skin = debuffSkin;
            }
            else
            {
                GUI.skin = basicSkin;
            }
            GUI.Label(new Rect(650.0f, 540.0f, 80.0f, 70.0f), "Resources: " + Resources.ToString("F0"));

            if (defeated)
            {
                GUI.skin = failureSkin;
                GUI.Label(new Rect((Screen.width /2) - 100 , Screen.height/2, 200.0f, 70.0f), "Base Destroyed!");
            }
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


    public void ResetLevel()
    {
        DestroyEnemies();
        DestroyTowers();
        waveNum = 0;
        Base.GetComponent<Script_Base>().health = 100;
        Resources = 2000;
        StopAllCoroutines();
        StartCoroutine(StartGame());
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0.0f;
        PauseMenu.SetActive(true);
    }

    public void Unpause()
    {
        paused = false;
        Time.timeScale = 1.0f;
        PauseMenu.SetActive(false);
    }

    public void Quit()
    {
        ResetLevel();
        Application.LoadLevel(mainMenuLevel);

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

    void FormulateWave()
    {
        spawnQueue.Clear();
        int attackSize = startWaveSize + waveNum + Random.Range(waveIncreaseFactorMin, waveIncreaseFactorMax);
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
            float healthFactor = (Mathf.Pow( 1 + healthScalingFactor, waveNum)); // + 1.0f;
            Script_Enemy_Health memHealth = formation.members[i].GetComponent<Script_Enemy_Health>();
            memHealth.SetHealth(memHealth.maxHealth * healthFactor);
            memHealth.resourceYield = memHealth.resourceYield * ((waveNum + 1) * resourceScalingFactor);
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
        FormulateWave();
        yield return new WaitForSeconds(betweenWavePause);
        for (int i = 0; i < spawnQueue.Count; i++)
        {
            StartCoroutine(SpawnFormation(spawnQueue[i]));
            yield return new WaitForSeconds((spawnQueue[i].spawnList.Count * inFormationPause) + betweenFormationPause);
        }
        waveNum++;
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
