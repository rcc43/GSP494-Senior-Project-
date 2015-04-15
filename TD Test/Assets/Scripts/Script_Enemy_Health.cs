using UnityEngine;
using System.Collections.Generic;


//reflects the health on an enemy.
public class Script_Enemy_Health : MonoBehaviour {


    public GameObject GameController; //reference to gamecontroller.
    public float maxHealth = 20;
    public float health; //health value.

    public float resourceYield;

    Script_GameController controller; //the gamecontroller script.
    Script_BuffList buffs; //the buffs applied to this entity.

    public GameObject infoCard_prefab;
    GameObject infocard;
    Script_Infocard_Enemy infocardData;
    public int infoCard_x;
    public int infoCard_y;

    public bool selected = false;

    bool drawingCard = false;

    LineRenderer projector;

    public float projectorRange = 1.0f;

    public int numPoints = 10;
    

	// Use this for initialization
	void Start ()
    {
        GameController = GameObject.FindWithTag("GameController"); //sets up the gamecontroller.
        controller = GameController.GetComponent<Script_GameController>();

        buffs = gameObject.GetComponent<Script_BuffList>(); //sets up the buff list.

        health = maxHealth;

        Script_AreaEffect area = gameObject.GetComponent<Script_AreaEffect>();

        if (area != null)
        {
            projectorRange = area.range;
        }

        Script_HostileWeapon weap = gameObject.GetComponent<Script_HostileWeapon>();

        if (weap != null)
        {
            projectorRange = weap.range;
        }

        projector = gameObject.AddComponent<LineRenderer>();
        projector.SetVertexCount(numPoints + 1);
        projector.material = new Material(Shader.Find("Particles/Additive"));
        projector.SetColors(Color.red, Color.red);
        projector.SetWidth(.05f, .05f);
        projector.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (health <= 0) //if health is less than 0, destroys self.
        {
            DestroySelf();
        }

        bool selectHit = false;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition); //raycasts from the camera to the mouse.

        RaycastHit[] hits; //scans through all hits from the raycast.
        hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject == gameObject) //if the raycast hits a tower...
            {
                selectHit = true;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (selectHit)
            {
                selected = true;
            }
            else selected = false;
        }

        if (!drawingCard)
        {
            if (selected)
            {
                infocard = Instantiate(infoCard_prefab) as GameObject;
                infocardData = infocard.GetComponent<Script_Infocard_Enemy>();
                infocardData.health = this;
                infocardData.movement = gameObject.GetComponent<Script_Enemy_Move>();
                infocard.transform.SetParent(controller.UICanvas.transform);
                drawingCard = true;
            }
        }
        else
        {
            infocardData.UpdateData();
            RectTransform infoTrans = infocard.GetComponent<RectTransform>();
            infoTrans.anchoredPosition = new Vector2(infoCard_x, infoCard_y);
        }

        if (!selected)
        {
            Destroy(infocard);
            drawingCard = false;
            projector.enabled = false;
        }
        else
        {
            projector.enabled = true;

            int index = 0;
            for (float i = 0f; index < numPoints; i = i + ((Mathf.PI * 2) / numPoints))
            {
                projector.SetPosition(index++, new Vector3(transform.position.x + projectorRange * Mathf.Sin(i), transform.position.y + .5f, transform.position.z + projectorRange * Mathf.Cos(i)));
            }
            projector.SetPosition(index++, new Vector3(transform.position.x + projectorRange * Mathf.Sin(0.0f), transform.position.y + .5f, transform.position.z + projectorRange * Mathf.Cos(0.0f)));
        }

	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shot") //checks if the things colliding with it is a shot.
        {
            Script_Shot shot = other.GetComponent<Script_Shot>(); //acquires shot script.
            if (shot != null)
            {
                if (shot.tgtType == targetType.enemy)
                {
                    health -= shot.damage; //reduces health by the damage of the shot.
                    if (shot.buff != null && buffs != null)
                    {
                        buffs.AddBuff(shot.buff); //applies the buff attached to this shot (if any).
                    }
                    Destroy(other.gameObject); //destroys the shot.
                }
            }
        }
    }

    public void SetHealth(float newHealth)
    {
        maxHealth = newHealth;
        health = maxHealth;
    }

    public void DestroySelf()
    {
        controller.GetEnemies().Remove(gameObject); //removes itself from the enemy list.
        controller.Resources += resourceYield;

        if (drawingCard)
        {
            Destroy(infocard);
        }

        Destroy(gameObject); //destroys self.
    }
}
