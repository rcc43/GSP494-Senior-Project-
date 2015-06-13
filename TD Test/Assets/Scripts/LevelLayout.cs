using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class LevelLayout
{

    public List<string> loadName;
    public string name;
    public string description;
    public bool campaign;

    public List<bool> enableBoss;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
