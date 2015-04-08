using UnityEngine;
using System.Collections;

public class Script_Base : MonoBehaviour {

    public int health = 100;

    GameObject gameController;
    Script_GameController controller;

	// Use this for initialization
	void Start () {

        gameController = GameObject.FindWithTag("GameController");
        controller = gameController.GetComponent<Script_GameController>();
	
	}
	
	// Update is called once per frame
	void Update () {

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
