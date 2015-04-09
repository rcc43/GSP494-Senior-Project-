using UnityEngine;
using System.Collections;

public class Script_GhostTower : MonoBehaviour {

    LineRenderer projector;

    public GameObject spawnedTower; //prefab of the tower that is created by this ghost.
    public float alpha = .5f; //the transparency of the ghost.
    public float height = 1.0f; //the height of the tower (so that the ghost doesn't clip through the ground).

    public float range = 15;

    public int numPoints = 50;

    bool foundLocation; //varaible denoting when the ghost is over a viable build location.
    Script_GameController controller; //reference to the gamecontroller.

    Script_Ground groundTarget; //reference to the ground tile that is getting placed on.

	// Use this for initialization
	void Start ()
    {

        foundLocation = false;

        controller = GameObject.FindWithTag("GameController").GetComponent<Script_GameController>(); //sets up gamecontroller.

        Color color = gameObject.renderer.material.color; //sets alpha to alpha value.
        color.a = alpha;
        gameObject.renderer.material.color = color;

        projector = gameObject.AddComponent<LineRenderer>();
        projector.SetVertexCount(numPoints + 1);
        projector.material = new Material(Shader.Find("Particles/Additive"));
        projector.SetColors(Color.green, Color.green);
        projector.SetWidth(.1f, .1f);

	}
	
	// Update is called once per frame
	void Update ()
    {
        foundLocation = false;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition); //raycasts from the camera to the mouse.

        RaycastHit[] hits; //scans through all hits from the raycast.
        hits = Physics.RaycastAll(ray);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.gameObject.tag == "Ground") //if the raycast hits a ground tile...
            {
                foundLocation = true; //sets foundLocation as true.
                groundTarget = hits[i].transform.GetComponent<Script_Ground>(); //gets the script for that patch of ground.
                transform.position = hits[i].transform.position + new Vector3(0.0f, height / 2, 0.0f); //moves the tower outline into the middle of the ground piece.
            }
        }

        if (!foundLocation) //if the tower outline is not hovering over ground, follows the mouse.
        {
            Vector3 drawPos = Input.mousePosition;
            drawPos.z = 10 - (height / 2);
            transform.position = Camera.main.ScreenToWorldPoint(drawPos);
        }

        if (Input.GetButton("Fire1")) //if the left mouse is hit... 
        {
            if (foundLocation && groundTarget != null && controller != null) //checks if the ground already has a tower on it.
            {
                if (groundTarget.tower == null)
                {
                    GameObject newTower = Instantiate(spawnedTower, transform.position, transform.rotation) as GameObject; //if not, builds a tower at this location.
                    groundTarget.tower = newTower;
                    Destroy(gameObject);
                    controller.building = false;
                    controller.GetTowers().Add(newTower);
                }
            }
        }

        if (Input.GetButton("Fire2"))
        {
            Destroy(gameObject);
            controller.building = false;
        }

        int index = 0;
        for (float i = 0f; index < numPoints; i = i + ((Mathf.PI * 2) / numPoints))
        {
            projector.SetPosition(index++, new Vector3(transform.position.x + range * Mathf.Sin(i), transform.position.y + .5f, transform.position.z + range * Mathf.Cos(i)));
        }
        projector.SetPosition(index++, new Vector3(transform.position.x + range * Mathf.Sin(0.0f), transform.position.y + .5f, transform.position.z + range * Mathf.Cos(0.0f)));
	
	}
}
