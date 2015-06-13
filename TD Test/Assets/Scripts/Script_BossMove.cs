using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct Box
{
    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;
}

public class Script_BossMove : MonoBehaviour
{
    Script_GameController controller;

    public float blitzRange = .25f; //the range at which a flying unit will dive at the base.

    public string enemyName;
    public string description;

    public float baseSpeed = 10; //the base speed of this entity.
    public float aboveHeight = .5f; //the height that this entity's center should hover over tiles or the terrain.

    public Vector3 tgt; //the road element currently being targeted.

    public float speed; //the movement speed of the entity, with debuffs/buffs factored in.

    public float distance; //distance along the road.

    public float pauseTime = .5f;
    float pauseTimer;

	// Use this for initialization
	void Start ()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>();
	    speed = baseSpeed;
        Vector3 meep = new Vector3(UnityEngine.Random.Range(controller.bounds.xMin, controller.bounds.xMax), aboveHeight, UnityEngine.Random.Range(controller.bounds.yMin, controller.bounds.yMax));
        tgt = meep;

        pauseTimer = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        pauseTimer -= Time.deltaTime;
        if (Vector3.Distance(gameObject.transform.position, tgt) < blitzRange)
        {
            if (pauseTimer <= 0)
            {
                Debug.Log(pauseTimer);
                Vector3 meep = new Vector3(UnityEngine.Random.Range(controller.bounds.xMin, controller.bounds.xMax), aboveHeight, UnityEngine.Random.Range(controller.bounds.yMin, controller.bounds.yMax));
                tgt = meep;
                pauseTimer = pauseTime;
            }
        }
        if (pauseTimer <= 0)
        {
            Vector3 forward = tgt - transform.position;
            rigidbody.velocity = forward * speed;
            Debug.Log(speed);
        }

	}
}
