using UnityEngine;
using System.Collections;


//kills a gameobject after a set amount of time.
public class Script_KillAfterTime : MonoBehaviour {

    public float lifeTime = 3.0f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
