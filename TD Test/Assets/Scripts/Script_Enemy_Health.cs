using UnityEngine;
using System.Collections.Generic;


//reflects the health on an enemy.
public class Script_Enemy_Health : MonoBehaviour {

    public GameObject GameController; //reference to gamecontroller.
    public float health; //health value.

    Script_GameController controller; //the gamecontroller script.
    Script_BuffList buffs; //the buffs applied to this entity.
    

	// Use this for initialization
	void Start ()
    {
        GameController = GameObject.FindWithTag("GameController"); //sets up the gamecontroller.
        controller = GameController.GetComponent<Script_GameController>();

        buffs = gameObject.GetComponent<Script_BuffList>(); //sets up the buff list.
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (health <= 0) //if health is less than 0, destroys self.
        {
            DestroySelf();
        }
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shot") //checks if the things colliding with it is a shot.
        {
            Script_Shot shot = other.GetComponent<Script_Shot>(); //acquires shot script.
            if (shot != null)
            {
                health -= shot.damage; //reduces health by the damage of the shot.
                if (shot.buff != null && buffs != null)
                {
                    buffs.AddBuff(shot.buff); //applies the buff attached to this shot (if any).
                }
            }
            Destroy(other.gameObject); //destroys the shot.
        }
    }

    void DestroySelf()
    {
        controller.GetEnemies().Remove(gameObject); //removes itself from the enemy list.
        Destroy(gameObject); //destroys self.
    }
}
