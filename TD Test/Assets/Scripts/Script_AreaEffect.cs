using UnityEngine;
using System.Collections.Generic;

public enum targetType : int { tower, enemy };

public class Script_AreaEffect : MonoBehaviour
{

    GameObject gameController;
    Script_GameController controller;

    public Buff buff;
    public float range;
    public float fireRate = 1.0f;
    public float fireCounter = 0;

    public bool holdFire = false;

    public targetType tgtClass;

	// Use this for initialization
	void Start ()
    {
        gameController = GameObject.FindWithTag("GameController");
        controller = gameController.GetComponent<Script_GameController>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!holdFire)
        {
            fireCounter -= Time.deltaTime;
            Fire();
        }
	}

    public void Fire()
    {
        List<GameObject> targets = new List<GameObject>();
        if (fireCounter <= 0.0f)
        {
            switch (tgtClass)
            {
                case targetType.enemy:
                {
                    targets = controller.GetEnemies();
                    break;
                }
                case targetType.tower:
                {
                    targets = controller.GetTowers();
                    break;
                }
            }
            fireCounter = fireRate;
        }

        for (int i = 0; i < targets.Count; ++i)
        {
            float dist = Vector3.Distance(transform.position, targets[i].transform.position);
            if (dist < range && targets[i] != gameObject)
            {
                Script_BuffList buffList = targets[i].GetComponent<Script_BuffList>();
                if (buffList != null)
                {
                    Buff applyBuff = new Buff(buff);
                    buffList.AddBuff(applyBuff);
                }
            }
        }
    }
}
