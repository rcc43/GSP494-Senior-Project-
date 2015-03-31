using UnityEngine;
using System.Collections.Generic;

public class Script_Tower : MonoBehaviour {

    LineRenderer projector;

    public GameObject GameController;
    public GameObject[] weapons;
    public float range = 15.0f;

    public int numPoints = 10;

    public float damage = 10.0f;

    public bool stunned = false;
    public bool selected = false;
    
    Script_Weapon[] weaponTargeting;
    Script_GameController controller;
    List<GameObject> targets = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        GameController = GameObject.FindWithTag("GameController");
        controller = GameController.GetComponent<Script_GameController>();
        weaponTargeting = new Script_Weapon[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponTargeting[i] = weapons[i].GetComponent<Script_Weapon>();
        }

        projector = gameObject.AddComponent<LineRenderer>();
        projector.SetVertexCount(numPoints + 1);
        projector.material = new Material(Shader.Find("Particles/Additive"));
        projector.SetColors(Color.green, Color.green);
        projector.SetWidth(.1f, .1f);

        UpdateStats();
	}
	
	// Update is called once per frame
	void Update ()
    {
        bool selectHit = false;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition); //raycasts from the camera to the mouse.

        RaycastHit[] hits; //scans through all hits from the raycast.
        hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject == gameObject) //if the raycast hits a tower...
            {
                if (!controller.building)
                {
                     selectHit = true;
                }
            }
        }

        if (Input.GetButton("Fire1"))
        {
            if (selectHit)
            {
                selected = true;
            }
            else selected = false;
        }

        if (selected)
        {
            projector.enabled = true;
        }
        else projector.enabled = false;

        targets.Clear();
        List<GameObject> potentialTargets = new List<GameObject>();
        for (int i = 0; i < controller.GetEnemies().Count; i++)
        {
            GameObject tgt = controller.GetEnemies()[i];
            float dist = Vector3.Distance(transform.position, tgt.transform.position);
            if (dist < range)
            {
                targets.Add(tgt);
            }
        }
        Fire();

        int index = 0;
        for (float i = 0f; index < numPoints; i = i + ((Mathf.PI * 2) / numPoints))
        {
            projector.SetPosition(index++, new Vector3(transform.position.x + range * Mathf.Sin(i), transform.position.y + .5f, transform.position.z + range * Mathf.Cos(i)));
        }
        projector.SetPosition(index++, new Vector3(transform.position.x + range * Mathf.Sin(0.0f), transform.position.y + .5f, transform.position.z + range * Mathf.Cos(0.0f)));
	}

    void UpdateStats()
    {
        foreach (Script_Weapon weap in weaponTargeting)
        {
            weap.SetDamage(damage);
        }
    }

    void Fire()
    {
        GameObject[] tgt = new GameObject[weapons.Length];


        //this is the 'target nearest' variation.
        for (int i = 0; i < weapons.Length; i++) //cycles through all weapons.
        {
            float minDist = Mathf.Infinity;
            for( int k = 0; k < targets.Count; k++) //cycles through all potential targets.
            {
                float dist = Vector3.Distance(transform.position, targets[k].transform.position); //gets distance between tower & target.
                if (dist < minDist) //if distance is shorter than current shortest...
                {
                    bool taken = false;
                    for (int j = 0; j < weapons.Length; j++) //cycles through all weapons.
                    {
                        if( tgt[j] == targets[k]) //if this enemy is already targeted by another weapon...
                        {
                            taken = true; //sets it as taken, aborts loop.
                        }
                    }
                    if (!taken) //if enemy isn't taken sets target as this enemy, updates shortest distance.
                    {
                        tgt[i] = targets[k];
                        minDist = dist;
                    }
                }
            }
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (tgt[i] != null)
            {
                weapons[i].transform.LookAt(tgt[i].transform);
                weaponTargeting[i].Fire(tgt[i]);
            }
        }

    }
}
