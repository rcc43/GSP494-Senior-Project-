using UnityEngine;
using System.Collections;

public class Script_Colorize : MonoBehaviour {

    public Color color;

	// Use this for initialization
	void Start () {

        gameObject.renderer.material.SetColor("_TintColor", color);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
