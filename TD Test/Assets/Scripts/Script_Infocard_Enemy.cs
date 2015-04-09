using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_Infocard_Enemy : MonoBehaviour {


    public Text[] text;
    public Script_Enemy_Health health;
    public Script_Enemy_Move movement;

    Script_HealthBar healthBar;

	// Use this for initialization
	void Start ()
    {
        healthBar = transform.FindChild("HealthBar").GetComponent<Script_HealthBar>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void UpdateData()
    {

        text = gameObject.GetComponentsInChildren<Text>();
        if (health && movement)
        {
            text[0].text = movement.enemyName;
            text[1].text = health.health.ToString("F1") + "/" + health.maxHealth.ToString("F1");
            text[2].text = "Speed: " + movement.speed.ToString("F1") + "/" + movement.baseSpeed.ToString("F1");
            text[3].text = movement.description;
        }

        if (healthBar)
        {
            healthBar.health = health.health;
            healthBar.maxHealth = health.maxHealth;
            healthBar.ResizeBar();
        }
    }
}
