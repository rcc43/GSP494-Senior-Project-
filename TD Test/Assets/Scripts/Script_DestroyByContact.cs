using UnityEngine;
using System.Collections;

public class Script_DestroyByContact : MonoBehaviour {

    public GameObject EXPLOSIONS;
    public GameObject PlayerExplosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Boundary")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
            Instantiate(EXPLOSIONS, transform.position, transform.rotation);
            if (other.tag == "Player")
            {
                Instantiate(PlayerExplosion, other.transform.position, other.transform.rotation);
            }
        }
    }
}
