﻿using UnityEngine;
using System.Collections.Generic;

public class Script_HostileWeapon : MonoBehaviour
{

    public GameObject[] weapons;

    public Buff buff;
    public float range = 15.0f;

    public float baseDamage = 10.0f;
    public float damage;

    public float fireRate = 1.0f;

    GameObject GameController;

    Script_Weapon[] weaponTargeting;
    Script_GameController controller;
    List<GameObject> targets = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        GameController = GameObject.FindWithTag("GameController");
        controller = GameController.GetComponent<Script_GameController>();
        weaponTargeting = new Script_Weapon[weapons.Length];

        damage = baseDamage;
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponTargeting[i] = weapons[i].GetComponent<Script_Weapon>();
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateStats();

        targets.Clear();
        List<GameObject> potentialTargets = new List<GameObject>();
        for (int i = 0; i < controller.GetTowers().Count; i++)
        {
            GameObject tgt = controller.GetTowers()[i];
            if (tgt != null)
            {
                float dist = Vector3.Distance(transform.position, tgt.transform.position);
                if (dist < range)
                {
                    targets.Add(tgt);
                }
            }
        }
        Fire();
	}

    void UpdateStats()
    {
        foreach (Script_Weapon weap in weaponTargeting)
        {
            weap.SetDamage(damage);
            weap.fireRate = fireRate;
            weap.tgtType = targetType.tower;
            weap.buff = buff;
        }
    }


    void Fire()
    {
        GameObject[] tgt = new GameObject[weapons.Length];


        //this is the 'target nearest' variation.
        for (int i = 0; i < weapons.Length; i++) //cycles through all weapons.
        {
            float minDist = Mathf.Infinity;
            for (int k = 0; k < targets.Count; k++) //cycles through all potential targets.
            {
                float dist = Vector3.Distance(transform.position, targets[k].transform.position); //gets distance between tower & target.
                if (dist < minDist) //if distance is shorter than current shortest...
                {
                    bool taken = false;
                    for (int j = 0; j < weapons.Length; j++) //cycles through all weapons.
                    {
                        if (tgt[j] == targets[k]) //if this enemy is already targeted by another weapon...
                        {
                            taken = true; //sets it as taken, aborts loop.
                        }
                    }
                    if (!taken) //if enemy isn't taken sets target as this enemy, updates shortest distance.
                    {
                        tgt[i] = targets[k];
                        minDist = dist;
                    }
                }
            }
        }

        for (int i = 0; i < weapons.Length; i++)
        {
            if (tgt[i] != null)
            {
                weapons[i].transform.LookAt(tgt[i].transform);
                if (buff != null)
                {
                    if (weaponTargeting[i].Fire(tgt[i]))
                    {
                    }
                }
            }
        }
    }
}
