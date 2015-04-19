using UnityEngine;
using System.Collections;

public class Script_Base : MonoBehaviour {

    public float maxHealth = 100;
    public float health = 100;

    public bool invincible = false;


    public GameObject healthBar;

    GameObject gameController;
    Script_GameController controller;
    public Script_HealthBar healthProjection;

	// Use this for initialization
	void Start () {

        gameController = GameObject.FindWithTag("GameController");
        controller = gameController.GetComponent<Script_GameController>();

	}
	
	// Update is called once per frame
	void Update () {

        if (healthProjection != null)
        {
            healthProjection.maxHealth = maxHealth;
            healthProjection.health = health;
        }

        if (health <= 0)
        {
            if (controller != null)
            {
                if (!controller.defeated)
                {
                    controller.defeated = true;
                    controller.defeatTimer = 5.0f;
                }
            }
        }
	}

    void OnTriggerEnter( Collider other )
    {
        if (other.tag == "Enemy")
        {
            Script_Enemy_Health tgtHealth = other.GetComponent<Script_Enemy_Health>();
            tgtHealth.DestroySelf(false);
            if (!invincible)
            {
                health--;
            }
        }
    }
}
