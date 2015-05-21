using UnityEngine;
using System.Collections.Generic;

public class Script_Tower : MonoBehaviour {

    public string towerName = "Generic Tower";

    public string description = "This is a generic test tower";

    public int cost = 300;

    public bool canHitAir = false;

    public Color ringColor;
    LineRenderer projector;

    public AudioClip fire;
    public bool soundless = false;

    public bool demo = false;

    public Buff buff;

    public GameObject GameController;
    public GameObject[] weapons;
    public GameObject head;
    public GameObject gun;
    public float range = 15.0f;

    public int numPoints = 10;

    public float baseDamage = 10.0f;
    public float damage;

    public float fireRate = 1.0f;

    public bool stunned = false;
    public bool selected = false;

    public int infoCard_width = 300;
    public int infoCard_height = 150;
    public int infoCard_offsetX = 20;
    public int infoCard_offsetY = 20;

    public float damageCost = 50.0f;
    public float damageUpgradeCurve = 1.65f;
    public float damageCostCurve = 2.0f;

    public float rangeCost = 50.0f;
    public float rangeUpgradeCurve = 1.15f;
    public float rangeCostCurve = 2.0f;

    public float fireRateCost = 50.0f;
    public float fireRateUpgradeCurve = .65f;
    public float fireRateCostCurve = 2.0f;

    public int maxDamageLevels = 5;
    int damageLevel = 0;

    public int maxRangeLevels = 5;
    int rangeLevel = 0;

    public int maxSpeedLevels = 5;
    int speedLevel = 0;

    bool GUIReserveHit = false; //this keeps a tower selected if a click occurred on its GUI buttons.
    
    Script_Weapon[] weaponTargeting;
    Script_GameController controller;
    List<GameObject> targets = new List<GameObject>();
    Script_BuffList buffs;

	// Use this for initialization
	void Start ()
    {
        GameController = GameObject.FindWithTag("GameController");
        controller = GameController.GetComponent<Script_GameController>();
        weaponTargeting = new Script_Weapon[weapons.Length];

        buffs = gameObject.GetComponent<Script_BuffList>();

        damage = baseDamage;
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponTargeting[i] = weapons[i].GetComponent<Script_Weapon>();
        }


        projector = gameObject.AddComponent<LineRenderer>();
        projector.SetVertexCount(numPoints + 1);
        projector.material = new Material(Shader.Find("Particles/Additive"));
        projector.SetColors(ringColor, ringColor);
        projector.SetWidth(.1f, .1f);
        projector.enabled = false;

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

        if (!demo)
        {
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

            UpdateStats();
        }

        targets.Clear();
        List<GameObject> potentialTargets = new List<GameObject>();
        for (int i = 0; i < controller.GetEnemies().Count; i++)
        {
            GameObject tgt = controller.GetEnemies()[i];
            if (tgt != null)
            {
                float dist = Vector3.Distance(transform.position, tgt.transform.position);
                Script_Enemy_Move tgtMove = tgt.GetComponent<Script_Enemy_Move>();
                if (tgtMove != null)
                {
                    if (dist < range && tgtMove.flying == canHitAir)
                    {
                        targets.Add(tgt);
                    }
                }
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
            weap.tgtType = targetType.enemy;
        }

        Script_AreaEffect area = GetComponent<Script_AreaEffect>();
        if (area)
        {
            buff.magnitude = baseDamage;
            area.buff = buff;
            area.range = range;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shot") //checks if the things colliding with it is a shot.
        {
            Script_Shot shot = other.GetComponent<Script_Shot>(); //acquires shot script.
            if (shot != null)
            {
                if (shot.tgtType == targetType.tower)
                {
                    if (shot.buff != null && buffs != null)
                    {
                        buffs.AddBuff(shot.buff); //applies the buff attached to this shot (if any).
                    }
                    Destroy(other.gameObject); //destroys the shot.
                }
            }
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

        if (!stunned)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                if (tgt[i] != null)
                {
                    head.transform.eulerAngles = new Vector3(0.0f, (180 / Mathf.PI) * Mathf.Atan2((tgt[i].transform.position.x - head.transform.position.x), tgt[i].transform.position.z - head.transform.position.z), 0.0f); // weapons[i].transform.LookAt(tgt[i].transform);
                    gun.transform.localRotation = new Quaternion(Mathf.Atan2(-(gun.transform.position.y - tgt[i].transform.position.y), -Mathf.Sqrt (Mathf.Pow((gun.transform.position.x - tgt[i].transform.position.x), 2) + Mathf.Pow((gun.transform.position.z - tgt[i].transform.position.z),2.0f))), 0.0f, 0.0f, 1.0f);         
                    if (buff != null)
                    {
                        if (weaponTargeting[i].Fire(tgt[i]))
                        {
                            if (!soundless)
                            {
                                audio.Play();
                            }
                        }
                    }
                    else
                    {
                        if (weaponTargeting[i].Fire(tgt[i]))
                        {
                            if (!soundless)
                            {
                                audio.Play();
                            }
                        }
                    }
                }
            }
        }
    }



    void DrawInfocard()
    {
        GUI.skin = controller.basicSkin;
        Vector3 displayPos = new Vector3(0, 150, 0);
        /*
        Vector3 displayPos = Camera.main.WorldToScreenPoint(transform.position);
        displayPos.x += infoCard_offsetX;
         */
        displayPos.y = Screen.height - (displayPos.y - infoCard_offsetY);
        GUI.Box(new Rect(displayPos.x, displayPos.y, infoCard_width, infoCard_height), towerName);
        //displays current damage
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .15f)), infoCard_width * .5f, infoCard_height * .5f), "Damage: ");
        if (damage > baseDamage)
        {
            GUI.skin = controller.buffedSkin; //if the damage is being buffed, changes text color
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .13f) + 15), infoCard_width * .5f, infoCard_height * .5f), damage.ToString("F1"));
            GUI.skin = controller.basicSkin;
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .21f) + 15), infoCard_width * .5f, infoCard_height * .5f), baseDamage.ToString("F1"));
        }
        else
        {
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .15f) + 15), infoCard_width * .5f, infoCard_height * .5f), damage.ToString("F1"));
        }
        //button for upgrading
        if (damageLevel < maxDamageLevels)
        {
            //displays damage if upgraded         
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .15f)), infoCard_width * .5f, infoCard_height * .5f), "Upgrade:");
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .15f) + 15), infoCard_width * .5f, infoCard_height * .5f), (baseDamage * damageUpgradeCurve).ToString("F1"));
            if (GUI.Button(new Rect(displayPos.x + (infoCard_width * .55f), (displayPos.y + (infoCard_height * .15f) + 15), infoCard_width * .4f, infoCard_height * .2f), "Cost: " + (damageCost).ToString("F1")))
            {
                if (controller.EnoughResources(damageCost))
                {
                    baseDamage = baseDamage * damageUpgradeCurve;
                    damage = damage * damageUpgradeCurve;
                    damageCost *= damageCostCurve;
                    damageLevel++;
                    GUIReserveHit = true;
                    UpdateStats();
                }
            }
        }

        //displays current Range
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .4f)), infoCard_width * .5f, infoCard_height * .5f), "Range: ");
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .4f) + 15), infoCard_width * .5f, infoCard_height * .5f), range.ToString("F1"));
        //button for upgrading
        if (rangeLevel < maxRangeLevels)
        {
            //displays range if upgraded        
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .4f)), infoCard_width * .5f, infoCard_height * .5f), "Upgrade:");
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .4f) + 15), infoCard_width * .5f, infoCard_height * .5f), (range * rangeUpgradeCurve).ToString("F1"));
            if (GUI.Button(new Rect(displayPos.x + (infoCard_width * .55f), (displayPos.y + (infoCard_height * .4f) + 15), infoCard_width * .4f, infoCard_height * .2f), "Cost: " + (rangeCost).ToString("F1")))
            {
                if (controller.EnoughResources(rangeCost))
                {
                    range = range * rangeUpgradeCurve;
                    rangeCost *= rangeCostCurve;
                    rangeLevel++;
                    GUIReserveHit = true;
                    UpdateStats();
                }
            }
        }

        //displays current speed
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .65f)), infoCard_width * .5f, infoCard_height * .5f), "Speed: ");
        GUI.Label(new Rect(displayPos.x + (infoCard_width * .02f), (displayPos.y + (infoCard_height * .65f) + 15), infoCard_width * .5f, infoCard_height * .5f), (1 / fireRate).ToString("F1"));
        //button for upgrading
        if (speedLevel < maxSpeedLevels)
        {
            //displays speed if upgraded         
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .65f)), infoCard_width * .5f, infoCard_height * .5f), "Upgrade:");
            GUI.Label(new Rect(displayPos.x + (infoCard_width * .25f), (displayPos.y + (infoCard_height * .65f) + 15), infoCard_width * .5f, infoCard_height * .5f), (1 / (fireRate * fireRateUpgradeCurve)).ToString("F1"));
            if (GUI.Button(new Rect(displayPos.x + (infoCard_width * .55f), (displayPos.y + (infoCard_height * .65f) + 15), infoCard_width * .4f, infoCard_height * .2f), "Cost: " + (fireRateCost).ToString("F1")))
            {
                if (controller.EnoughResources(fireRateCost))
                {
                    fireRate = fireRate * fireRateUpgradeCurve;
                    fireRateCost *= fireRateCostCurve;
                    speedLevel++;
                    GUIReserveHit = true;
                    UpdateStats();
                }
            }
        }
    }
}
