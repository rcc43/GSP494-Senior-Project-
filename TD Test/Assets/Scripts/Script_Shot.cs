using UnityEngine;
using System.Collections;

public class Script_Shot : MonoBehaviour {

    public float damage; //damage the shot will deal.
    public float speed = 20.0f; //the speed the shot will travel at.

    public Buff buff; //the buff that the shot will apply.

    GameObject target; //the target this shot will seek.

    public targetType tgtType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (target != null) //if the shot has a target, changes 'forward' to face the target.
        {
            transform.forward = target.transform.position - transform.position;
        }
        rigidbody.velocity = transform.forward * speed; //moves forward.
	}

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetTarget(GameObject tgt)
    {
        target = tgt;
    }
}
