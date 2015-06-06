using UnityEngine;
using System.Collections;

public class Script_Enemy_Move : MonoBehaviour
{
    public bool flying = false;
    public float diveRange = 10.0f; //the range at which a flying unit will dive at the base.

    public string enemyName;
    public string description;

    public GameObject tgt; //the road element currently being targeted.
    public float baseSpeed = 10; //the base speed of this entity.
    public float aboveHeight = .5f; //the height that this entity's center should hover over tiles or the terrain.

    public bool stunned = false; //whether or not this entity is stunned.

    public float speed; //the movement speed of the entity, with debuffs/buffs factored in.

    public float distance; //distance along the road.

    Script_Road tgtRoad; //the script of a target road piece.
    public Formation formation;

    public float aerialDisplacementTime;
    public float aerialDisplacementSpeed;
    public Vector3 aerialDisplacementDir;

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

            Script_Road projectionRoad = tgt.GetComponent<Script_Road>();
            if (formation.IsFirst(gameObject))
            {
                while (projectionRoad.next[0] != null)
                {
                    if (projectionRoad.numBranches > 1)
                    {
                        if (formation != null)
                        {
                                int rand = Random.Range(0, projectionRoad.numBranches);
                                formation.AddChoice(rand);
                                projectionRoad = projectionRoad.next[rand].GetComponent<Script_Road>();
                        }
                    }
                    else
                    {
                        if (projectionRoad.next[0] != null)
                        {
                        projectionRoad = projectionRoad.next[0].GetComponent<Script_Road>();
                        }
                    }
                }
            }
        }
        else tgt = GameObject.FindWithTag("Base");
        //aerialDispalacementTime = 0;
        //aerialDisplacementDir = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        EvaluateDistance();
        if (flying == false)
        {
            tgtRoad = tgt.GetComponent<Script_Road>(); //gets the script of the road piece it is targeting.

            //finds the distance between it and the center of the target road piece. if it is close...
            if (Vector3.Distance((tgt.transform.position + new Vector3(0.0f, aboveHeight, 0.0f)), transform.position) < tgtRoad.turnProximity)
            {
                if (tgtRoad.numBranches > 1)
                {
                    if (formation != null)
                    {
                        /*
                        if (formation.IsFirst(gameObject))
                        {
                            int rand = Random.Range(0, tgtRoad.numBranches);
                            formation.AddChoice(rand);
                            formation.positionUp(gameObject);
                            if (tgtRoad.next[rand] != null) //checks if the target road points to a new road piece.
                            {
                                tgt = tgtRoad.next[rand]; //if so, sets that piece as the new target, gets its script.
                                tgtRoad = tgt.GetComponent<Script_Road>();
                            }
                        }
                        else
                        {
                         * */
                            int branchChoice = formation.GetChoice(gameObject);
                            formation.positionUp(gameObject);
                            if (tgtRoad.next[branchChoice] != null) //checks if the target road points to a new road piece.
                            {
                                tgt = tgtRoad.next[branchChoice]; //if so, sets that piece as the new target, gets its script.
                                tgtRoad = tgt.GetComponent<Script_Road>();
                            }
                        //}
                    }
                    else
                    {
                        int rand = Random.Range(0, tgtRoad.numBranches);
                        if (tgtRoad.next[rand] != null) //checks if the target road points to a new road piece.
                        {
                            tgt = tgtRoad.next[rand]; //if so, sets that piece as the new target, gets its script.
                            tgtRoad = tgt.GetComponent<Script_Road>();
                        }
                    }
                }
                else
                {
                    if (tgtRoad.next[0] != null) //checks if the target road points to a new road piece.
                    {
                        tgt = tgtRoad.next[0]; //if so, sets that piece as the new target, gets its script.
                        tgtRoad = tgt.GetComponent<Script_Road>();
                    }
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
            if (aerialDisplacementTime <= 0)
            {
                if (Vector3.Distance(tgt.transform.position, transform.position) > diveRange)
                {
                    transform.forward = (tgt.transform.position + new Vector3(0.0f, aboveHeight, 0.0f)) - transform.position;
                }
                else
                {
                    transform.forward = tgt.transform.position - transform.position;
                }
                rigidbody.velocity = transform.forward * speed;
            }
            else
            {
                aerialDisplacementTime -= Time.deltaTime;
                transform.forward = aerialDisplacementDir;
                rigidbody.velocity = transform.forward * aerialDisplacementSpeed;
            }
        }
	}

    public float EvaluateDistance()
    {
        if (!flying)
        {
            distance = Vector3.Distance(gameObject.transform.position, tgt.transform.position);
            Script_Road projectionRoad = tgt.GetComponent<Script_Road>();
            int choice = 0;
            while (projectionRoad.next[0] != null)
            {
                if (projectionRoad.numBranches > 1)
                {
                    if (formation != null)
                    {
                        projectionRoad = projectionRoad.next[formation.choices[choice]].GetComponent<Script_Road>();
                        choice++;
                    }
                }
                else
                {
                    if (projectionRoad.next[0] != null)
                    {
                        projectionRoad = projectionRoad.next[0].GetComponent<Script_Road>();
                    }
                }
                distance += 1;
            }
        }
        else
        {
            distance = Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<Script_Enemy_Health>().controller.Base.transform.position);
        }
        return distance;
    }
}
