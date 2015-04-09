using UnityEngine;
using System.Collections;

public class Script_Base : MonoBehaviour {

    public float maxHealth = 100;
    public float health = 100;

    public GameObject healthBar;

    GameObject gameController;
    Script_GameController controller;
    Script_HealthBar healthProjection;

	// Use this for initialization
	void Start () {

        gameController = GameObject.FindWithTag("GameController");
        controller = gameController.GetComponent<Script_GameController>();

        healthProjection = healthBar.GetComponent<Script_HealthBar>();
	
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
                //controller.ResetLevel();
            }
        }
	}

    void OnTriggerEnter( Collider other )
    {
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            health--;
        }
    }
}
