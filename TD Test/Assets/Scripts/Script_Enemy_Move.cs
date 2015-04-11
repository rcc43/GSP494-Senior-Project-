using UnityEngine;
using System.Collections;

public class Script_Enemy_Move : MonoBehaviour {

    public bool flying = false;
    public float diveRange = 10.0f; //the range at which a flying unit will dive at the base.

    public string enemyName;
    public string description;

    public GameObject tgt; //the road element currently being targeted.
    public float baseSpeed = 10; //the base speed of this entity.
    public float aboveHeight = .5f; //the height that this entity's center should hover over tiles or the terrain.

    public bool stunned = false; //whether or not this entity is stunned.

    public float speed; //the movement speed of the entity, with debuffs/buffs factored in.

    Script_Road tgtRoad; //the script of a target road piece.

	// Use this for initialization
	void Start ()
    {

        speed = baseSpeed; //sets up initial speed.

        if (flying == false) //if the enemy is a non-flying one, targets the ground it spawns on.
        {
            Ray ray = new Ray(transform.position, new Vector3(0.0f, -1.0f, 0.0f)); //raycasts downward for the road piece it is on.

            RaycastHit[] hits; //the results of the raycast.
            hits = Physics.RaycastAll(ray);

            for (int i = 0; i < hits.Length; i++) //cycles through all raycast hits.
            {
                if (hits[i].transform.gameObject.tag == "Road") //if the hit is a road, sets it as the target.
                {
                    tgt = hits[i].transform.gameObject;

                }
            }
        }
        else tgt = GameObject.FindWithTag("Base");

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (flying == false)
        {
            tgtRoad = tgt.GetComponent<Script_Road>(); //gets the script of the road piece it is targeting.

            //finds the distance between it and the center of the target road piece. if it is close...
            if (Vector3.Distance((tgt.transform.position + new Vector3(0.0f, aboveHeight, 0.0f)), transform.position) < tgtRoad.turnProximity)
            {
                int rand = Random.Range(0, tgtRoad.numBranches);
                if (tgtRoad.next[rand] != null) //checks if the target road points to a new road piece.
                {
                    tgt = tgtRoad.next[rand]; //if so, sets that piece as the new target, gets its script.
                    tgtRoad = tgt.GetComponent<Script_Road>();
                }
            }

            if (tgtRoad != null) //if it has a viable target...
            {
                transform.forward = (tgtRoad.transform.position + new Vector3(0.0f, aboveHeight, 0.0f)) - transform.position; //rotates to face that road piece.
                rigidbody.velocity = transform.forward * speed; //moves toward that piece.
            }
        }
        else
        {
            if (Vector3.Distance(tgt.transform.position, transform.position) > diveRange)
            {
                transform.forward = (tgt.transform.position + new Vector3(0.0f, aboveHeight, 0.0f)) - transform.position;
                rigidbody.velocity = transform.forward * speed;
            }
            else
            {
                transform.forward = tgt.transform.position - transform.position;
                rigidbody.velocity = transform.forward * speed;
            }
        }
	}
}
