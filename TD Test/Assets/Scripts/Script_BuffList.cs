using UnityEngine;
using System.Collections.Generic;

public class Script_BuffList : MonoBehaviour {

    public List<Buff> buffs = new List<Buff>(); //a list of the buffs on this target.

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //updates each buff.

        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].BuffUpdate(gameObject);
        }
	}


    //adds a buff to the buff list.
    public void AddBuff(Buff newBuff)
    {
        if (newBuff.duration > 0)
        {
            Buff targetBuff; //other buffs in the buff list, checked to see if they have the same effect.
            int stacks = 0; //the number of times a given effect appears in the buff list.
            int allowed = newBuff.allowedStacks; //the allowed number of stacked buffs.
            for (int i = 0; i < buffs.Count; i++) //cycles through all present buffs.
            {
                targetBuff = buffs[i];
                if (targetBuff.type == newBuff.type) //if the current buff has the same effect as the buff being applied, increments stacks.
                {
                    stacks++;
                }
            }
            if (stacks + 1 > allowed) //if adding this buff will exceed the number of allowed buffs...
            {
                Buff oldestBuff = OldestBuffofType(newBuff.type); //gets the oldest buff applied of this type.
                if (oldestBuff.triggered == true) //oldest buff has activated, it gets deactivated.
                {
                    oldestBuff.BuffEnd(gameObject);
                }
                buffs.Remove(oldestBuff); //removes the oldest buff.
            }
            buffs.Add(newBuff); //adds the new buff.
            newBuff.ApplyBuffEffect(gameObject);
            newBuff.triggered = true;
        }
    }


    //removes the first buff of a given type.
    void RemoveBuffofType(buffType targetType)
    {
        Buff targetBuff;
        Buff removalBuff = new Buff();
        for (int i = 0; i < buffs.Count; i++)
        {
            targetBuff = buffs[i];
            if (targetBuff.type == targetType)
            {
                targetBuff.BuffEnd(gameObject);
                buffs.Remove(targetBuff);
            }
        }
    }


    //returns the buff with the least time left applied to this entity.
    public Buff OldestBuffofType(buffType targetType)
    {
        Buff targetBuff; //the target buff.
        Buff removalBuff = new Buff(); //the buff that will be removed.
        float lowestTime = Mathf.Infinity; //the least time left on any buff of this type.
        for (int i = 0; i < buffs.Count; i++) //cycles through buffs.
        {
            targetBuff = buffs[i];
            if (targetBuff.type == targetType) //if buff type is the one specified...
            {
                if (targetBuff.duration < lowestTime) //checks if this buff has the least time left.
                {
                    lowestTime = targetBuff.duration; //if it does, sets this as new shortest time.
                    removalBuff = targetBuff; //sets this buff to be removed.
                }
            }
        }
        return removalBuff;
    }
}
