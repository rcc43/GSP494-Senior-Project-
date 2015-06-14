using UnityEngine;
using System.Collections;

public class Script_DishRotate : MonoBehaviour
{

    public float rotationRate;
    GameObject dish;

	// Use this for initialization
	void Start ()
    {
        dish = gameObject.transform.Find("Dish").gameObject;
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        dish.transform.Rotate(new Vector3(0, 1, 0), rotationRate * Time.deltaTime);
	}
}
