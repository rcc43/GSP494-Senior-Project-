using UnityEngine;
using System.Collections;

public class Script_Weapon : MonoBehaviour {

    public GameObject shotPrefab; //the shot the weapon will fire.
    public float fireRate; //the fire rate of the weapon.

    public Buff buff;

    float damage; //the damage the weapon will deal.
    GameObject target; //the target of the weapon.
    float cooldown; //the cooldown timer for the weapon.

	// Use this for initialization
	void Start () {

        cooldown = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        cooldown -= Time.deltaTime;
	}

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetTarget(GameObject tgt)
    {
        target = tgt;
    }

    //fires the weapon.
    public void Fire()
    {
        if (cooldown <= 0) //if the weapon is ready to fire.
        {
            Script_Shot shot = Instantiate(shotPrefab, transform.position, transform.rotation) as Script_Shot; //creates the shot.
            shot.SetDamage(damage); //sets shot's damage to the weapon's damage.
            shot.SetTarget(target); //sets shot's target to the weapon's target.
            cooldown = fireRate; //resets the cooldown.
        }
    }

    //targeted variation of the fire weapon function. Same as the non-targeted version, but takes an external target.

    public void Fire(GameObject tgt)
    {
        if (cooldown <= 0)
        {
            GameObject shot = Instantiate(shotPrefab, transform.position, transform.rotation) as GameObject;
            Script_Shot shotData = shot.GetComponent<Script_Shot>();
            shotData.SetDamage(damage);
            shotData.SetTarget(tgt);
            cooldown = fireRate;
            if (buff != null)
            {
                Buff newBuff = new Buff(buff);
                shotData.buff = newBuff;
            }
        }
    }
}
