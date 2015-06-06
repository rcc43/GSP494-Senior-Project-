﻿using UnityEngine;
using System.Collections.Generic;

public class Script_CampaignData : MonoBehaviour {

    public List<string> campaignQueue = new List<string>();
    public bool isCampaign;

	// Use this for initialization
	void Start ()
    {
        DontDestroyOnLoad(gameObject);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clear()
    {
        campaignQueue = new List<string>();
        isCampaign = false;
    }
}