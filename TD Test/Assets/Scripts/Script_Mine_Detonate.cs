using UnityEngine;
using System.Collections.Generic;

public class Script_Mine_Detonate : MonoBehaviour
{

    public float range = 1.0f;
    Script_AreaEffect payload;

    Script_GameController controller;
    public GameObject explosion;


	// Use this for initialization
	void Start ()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        controller = gameController.GetComponent<Script_GameController>();
        payload = gameObject.GetComponent<Script_AreaEffect>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        List<GameObject> targets = controller.enemies;
        for (int i = 0; i < targets.Count; ++i)
        {
            float dist = Vector3.Distance(transform.position, targets[i].transform.position);
            if (dist < range && targets[i] != gameObject)
            {
                payload.Fire();
                Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
	}
}
