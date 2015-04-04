using UnityEngine;
using System.Collections.Generic;

public class Script_Tower : MonoBehaviour {

    LineRenderer projector;

    public AudioClip fire;

    public Buff buff;

    public GameObject GameController;
    public GameObject[] weapons;
    public float range = 15.0f;

    public int numPoints = 10;

    public float damage = 10.0f;

    public float fireRate = 1.0f;

    public bool stunned = false;
    public bool selected = false;

    public int infoCard_width = 300;
    public int infoCard_height = 150;
    public int infoCard_offsetX = 20;
    public int infoCard_offsetY = 20;

    public float damageUpgradeCurve = 1.65f;
    public float damageCostCurve = 2.0f;

    public float rangeUpgradeCurve = 1.15f;
    public float rangeCostCurve = 2.0f;

    public float fireRateUpgradeCurve = .65f;
    public float fireRateCostCurve = 2.0f;

    bool GUIReserveHit = false; //this keeps a tower selected if a click occurred on its GUI buttons.
    
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

        audio.clip = fire;

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

        if (Input.GetButtonUp("Fire1"))
        {
            if (selectHit || GUIReserveHit)
            {
                selected = true;
                GUIReserveHit = false;
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


    void OnGUI()
    {
        if (selected)
        {
            DrawInfocard();
        }
    }

    void UpdateStats()
    {
        foreach (Script_Weapon weap in weaponTargeting)
        {
            weap.SetDamage(damage);
            weap.fireRate = fireRate;
            weap.buff = buff;
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
                if (buff != null)
                {
                    if (weaponTargeting[i].Fire(tgt[i]))
                    {
                        //audio.Play();
                    }
                }
                else
                {
                    if (weaponTargeting[i].Fire(tgt[i]))
                    {
                      //  audio.Play();
                    }
                }
            }
        }

    }



    void DrawInfocard()
    {
        Vector3 displayPos = Camera.main.WorldToScreenPoint(transform.position);
        displayPos.x += infoCard_offsetX;
        displayPos.y = Screen.height - (displayPos.y - infoCard_offsetY);
        GUI.Box(new Rect(displayPos.x, displayPos.y, infoCard_width, infoCard_height), gameObject.name);
        //displays current damage
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .15f)), infoCard_width * .5f, infoCard_height * .5f), "Damage: ");
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .15f) + 15), infoCard_width * .5f, infoCard_height * .5f), damage.ToString("F1"));
        //displays range if damage         
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .15f)), infoCard_width * .5f, infoCard_height * .5f), "Upgrade:");
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .15f) + 15), infoCard_width * .5f, infoCard_height * .5f), (damage * damageUpgradeCurve).ToString("F1"));
        //button for upgrading
        if (GUI.Button(new Rect(displayPos.x + (infoCard_width * .55f), (displayPos.y + (infoCard_height * .15f) + 15), infoCard_width * .4f, infoCard_height * .1f), "Cost: " + (range * 2).ToString("F1")))
        {
            damage = damage * damageUpgradeCurve;
            GUIReserveHit = true;
            UpdateStats();
        }

        //displays current Range
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .4f)), infoCard_width * .5f, infoCard_height * .5f), "Range: ");
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .4f) + 15), infoCard_width * .5f, infoCard_height * .5f), range.ToString("F1"));
        //displays speed if upgraded        
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .4f)), infoCard_width * .5f, infoCard_height * .5f), "Upgrade:");
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .4f) + 15), infoCard_width * .5f, infoCard_height * .5f), (range * rangeUpgradeCurve).ToString("F1"));
        //button for upgrading
        if (GUI.Button(new Rect(displayPos.x + (infoCard_width * .55f), (displayPos.y + (infoCard_height * .4f) + 15), infoCard_width * .4f, infoCard_height * .1f), "Cost: " + (range * 2).ToString("F1")))
        {
            range = range * rangeUpgradeCurve;
            GUIReserveHit = true;
            UpdateStats();
        }

        //displays current speed
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .65f)), infoCard_width * .5f, infoCard_height * .5f), "Speed: ");
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .65f) + 15), infoCard_width * .5f, infoCard_height * .5f), fireRate.ToString("F1"));
        //displays speed if upgraded         
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .65f)), infoCard_width * .5f, infoCard_height * .5f), "Upgrade:");
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .65f) + 15), infoCard_width * .5f, infoCard_height * .5f), (fireRate * fireRateUpgradeCurve).ToString("F1"));
        //button for upgrading
        if (GUI.Button(new Rect(displayPos.x + (infoCard_width * .55f), (displayPos.y + (infoCard_height * .65f) + 15), infoCard_width * .4f, infoCard_height * .1f), "Cost: " + (fireRate / 2).ToString("F1")))
        {
            fireRate = fireRate * fireRateUpgradeCurve;
            GUIReserveHit = true;
            UpdateStats();
        }
    }
}
