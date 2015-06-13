using UnityEngine;
using System.Collections;

public class Script_Road : MonoBehaviour {

    public int numBranches = 1; //the number of roads this one can lead to.

    public GameObject mine;

    public GameObject[] next; //reference to next road piece in path.
    public float turnProximity = .25f; //the distance from the center an object must be to target the next road piece.

    int allowedBranches;
    int index;
    bool[] direction = { false, false, false, false };

	// Use this for initialization
	void Start ()
    {
        allowedBranches = numBranches;
        index = 0;
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < next.Length; i++)
        {
            if (next[i] != null)
            {
                Debug.DrawRay(transform.position, (next[i].transform.position - transform.position));
            }
        }

	}

    public void FindNext( Vector3 inVec)
    {

        allowedBranches = numBranches;

        if (index >= allowedBranches)
        {
            return;
        }

        next = new GameObject[numBranches];

        inVec = -inVec;

        Vector3 north = new Vector3(0.0f, 0.0f, -1.0f);
        Vector3 south = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 east = new Vector3(-1.0f, 0.0f, 0.0f);
        Vector3 west = new Vector3(1.0f, 0.0f, 0.0f);

        if (inVec == north)
        {
            direction[0] = true;
        }
        if (inVec == south)
        {
            direction[1] = true;
        }
        if (inVec == east)
        {
            direction[2] = true;
        }
        if (inVec == west)
        {
            direction[3] = true;
        }

        if (index < allowedBranches)
        {
            if (!direction[0])
            {
                Vector3 hoverPos = gameObject.transform.position + north + new Vector3(0.0f, 0.5f, 0.0f);
                var ray = new Ray(hoverPos, new Vector3(0.0f, -1.0f, 0.0f));

                Debug.DrawRay(ray.origin, ray.direction, Color.red);

                RaycastHit[] hits; //scans through all hits from the raycast.
                hits = Physics.RaycastAll(ray, 1.0f);

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.tag == "Road") //if the raycast hits a ground tile...
                    {
                        if (hits[i].transform.gameObject != gameObject)
                        {
                            next[index] = hits[i].transform.gameObject;
                            index++;
                            break;
                        }
                    }
                }
            }
        }

        if (index < allowedBranches)
        {
            if (!direction[1])
            {
                Vector3 hoverPos = gameObject.transform.position + south + new Vector3(0.0f, 0.5f, 0.0f);
                var ray = new Ray(hoverPos, new Vector3(0.0f, -1.0f, 0.0f));

                Debug.DrawRay(ray.origin, ray.direction, Color.red);

                RaycastHit[] hits; //scans through all hits from the raycast.
                hits = Physics.RaycastAll(ray, 1.0f);

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.tag == "Road") //if the raycast hits a ground tile...
                    {
                        if (hits[i].transform.gameObject != gameObject)
                        {
                            next[index] = hits[i].transform.gameObject;
                            index++;
                            break;
                        }
                    }
                }
            }
        }


        if (index < allowedBranches)
        {
            if (!direction[2])
            {
                Vector3 hoverPos = gameObject.transform.position + east + new Vector3(0.0f, 0.5f, 0.0f);
                var ray = new Ray(hoverPos, new Vector3(0.0f, -1.0f, 0.0f));

                Debug.DrawRay(ray.origin, ray.direction, Color.red);

                RaycastHit[] hits; //scans through all hits from the raycast.
                hits = Physics.RaycastAll(ray, 1.0f);

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.tag == "Road") //if the raycast hits a ground tile...
                    {
                        if (hits[i].transform.gameObject != gameObject)
                        {
                            next[index] = hits[i].transform.gameObject;
                            index++;
                            break;
                        }
                    }
                }
            }
        }

        if (index < allowedBranches)
        {
            if (!direction[3])
            {
                Vector3 hoverPos = gameObject.transform.position + west + new Vector3(0.0f, 0.5f, 0.0f);
                var ray = new Ray(hoverPos, new Vector3(0.0f, -1.0f, 0.0f));

                Debug.DrawRay(ray.origin, ray.direction, Color.red);

                RaycastHit[] hits; //scans through all hits from the raycast.
                hits = Physics.RaycastAll(ray, 1.0f);

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.gameObject.tag == "Road") //if the raycast hits a ground tile...
                    {
                        if (hits[i].transform.gameObject != gameObject)
                        {
                            next[index] = hits[i].transform.gameObject;
                            index++;
                            break;
                        }
                    }
                }
            }
        }




        for (int i = 0; i < index; i++)
        {
            if (next[i] != null)
            {
                Script_Road tgtRoad = next[i].GetComponent<Script_Road>();
                if (tgtRoad != null)
                {
                    tgtRoad.FindNext((next[i].transform.position - transform.position));
                }
            }
        }
    }


    Vector3 bendLeft (Vector3 inVec)
    {
        Vector3 bent = new Vector3(0.0f, 0.0f, 0.0f);

        if (inVec.x != 0.0f)
        {
            bent = new Vector3(0.0f, 0.0f, -1.0f);
        }
        else if (inVec.z != 0.0f)
        {
            bent = new Vector3(-1.0f, 0.0f, 0.0f);
        }

        return bent;
    }
}
