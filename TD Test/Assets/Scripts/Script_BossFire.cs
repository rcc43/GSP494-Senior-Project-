using UnityEngine;
using System.Collections;

public class Script_BossFire : MonoBehaviour
{
    Script_GameController controller;
    public GameObject missilePrefab;

    public float fireRate;
    float fireTimer;

	// Use this for initialization
	void Start ()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>();
        fireTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        fireTimer -=Time.deltaTime;
        if (fireTimer <= 0)
        {
            Fire();
            fireTimer = fireRate;
        }
	
	}

    public void Fire()
    {
        Quaternion spawnRot = Quaternion.identity;
        GameObject newEnemy;
        newEnemy = Instantiate(missilePrefab, gameObject.transform.position + new Vector3(0.0f, -.2f, 0.0f), spawnRot) as GameObject;
        Script_Enemy_Move launchMove = newEnemy.GetComponent<Script_Enemy_Move>();
        if (launchMove != null)
        {
            launchMove.aerialDisplacementTime = .4f;
            launchMove.aerialDisplacementSpeed = 4;
            launchMove.aerialDisplacementDir = new Vector3(0, 0, -1.0f);
        }
        controller.enemies.Add(newEnemy);
        controller.waveSize += 1;

        newEnemy = Instantiate(missilePrefab, gameObject.transform.position + new Vector3(0.0f, -.2f, 0.0f), spawnRot) as GameObject;
        launchMove = newEnemy.GetComponent<Script_Enemy_Move>();
        if (launchMove != null)
        {
            launchMove.aerialDisplacementTime = .4f;
            launchMove.aerialDisplacementSpeed = 4;
            launchMove.aerialDisplacementDir = new Vector3(-1.0f, 0, -1.0f);
        }
        controller.enemies.Add(newEnemy);
        controller.waveSize += 1;


        newEnemy = Instantiate(missilePrefab, gameObject.transform.position + new Vector3(0.0f, -.2f, 0.0f), spawnRot) as GameObject;
        launchMove = newEnemy.GetComponent<Script_Enemy_Move>();
        if (launchMove != null)
        {
            launchMove.aerialDisplacementTime = .4f;
            launchMove.aerialDisplacementSpeed = 4;
            launchMove.aerialDisplacementDir = new Vector3(1.0f, 0, -1.0f);
        }
        controller.enemies.Add(newEnemy);
        controller.waveSize += 1;


        newEnemy = Instantiate(missilePrefab, gameObject.transform.position + new Vector3(0.0f, -.2f, 0.0f), spawnRot) as GameObject;
       launchMove = newEnemy.GetComponent<Script_Enemy_Move>();
        if (launchMove != null)
        {
            launchMove.aerialDisplacementTime = .4f;
            launchMove.aerialDisplacementSpeed = 4;
            launchMove.aerialDisplacementDir = new Vector3(1.0f, 0, 0);
        }
        controller.enemies.Add(newEnemy);
        controller.waveSize += 1;

        newEnemy = Instantiate(missilePrefab, gameObject.transform.position + new Vector3(0.0f, -.2f, 0.0f), spawnRot) as GameObject;
        launchMove = newEnemy.GetComponent<Script_Enemy_Move>();
        if (launchMove != null)
        {
            launchMove.aerialDisplacementTime = .4f;
            launchMove.aerialDisplacementSpeed = 4;
            launchMove.aerialDisplacementDir = new Vector3(-1.0f, 0, 0);
        }
        controller.enemies.Add(newEnemy);
        controller.waveSize += 1;

    }
}
