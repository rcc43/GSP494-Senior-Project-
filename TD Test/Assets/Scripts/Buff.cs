using UnityEngine;
using System.Collections.Generic;
using System;

public enum buffType : int { doNothing, EnemyReduceSpeedPercent, EnemyStun, TowerStun, DealDOT, TowerUpDamagePercent};

[Serializable]
public class Buff
{ //buff/debuff class.

    public buffType type; //the type of buff.

    public float magnitude; //the intensity of the buff.

    public float duration; //how long the buff will last.

    public float DOTtimer; //time until another DOT hit happens. Currently assumed to be 1hz.

    public bool repeating; //if this buff applies itself repeatedly, or if it has a continuous effect.

    public bool triggered = false; //whether or not this buff has activated on a target. Buffs will activate on the frame after they are applied.

    public int allowedStacks = 1; //number of stacks of this type of buff allowed.


    public Buff()
    {
        type = buffType.doNothing;
        magnitude = 0.0f;
        duration = 0.0f;
        DOTtimer = 0.0f;
        repeating = false;
        triggered = false;
        allowedStacks = 1;
    }

    public Buff(Buff other)
    {
        if (other != null)
        {
            this.type = other.type;
            this.magnitude = other.magnitude;
            this.duration = other.duration;
            this.DOTtimer = other.DOTtimer;
            this.repeating = other.repeating;
            this.triggered = other.triggered;
            this.allowedStacks = other.allowedStacks;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
     //applies the buff effect to a target.
    public void ApplyBuffEffect(GameObject target)
    {
        Script_Enemy_Move enemyMove = target.GetComponent<Script_Enemy_Move>(); // the movement script for an enemy.
        Script_Enemy_Health enemyStats = target.GetComponent<Script_Enemy_Health>(); //the health script for an enemy.
        Script_Tower towerStats = target.GetComponent<Script_Tower>(); //the script for a tower.

        if (duration > 0)
        {
            switch (type)
            {
                case buffType.doNothing:
                    {
                        break;
                    }
                case buffType.EnemyReduceSpeedPercent:
                    {
                        if (enemyMove != null)
                        {
                            enemyMove.speed *= ((100 - magnitude) / 100); //reduces the enemy's current speed.
                        }
                    }
                    break;
                case buffType.EnemyStun:
                    {
                        if (enemyMove != null)
                        {
                            enemyMove.stunned = true; //stun the enemy
                        }
                    }
                    break;
                case buffType.TowerStun:
                    {
                        if (towerStats != null)
                        {
                            towerStats.stunned = true; //stun the tower.
                        }
                        break;
                    }
                case buffType.DealDOT:
                    {
                        if (enemyStats != null)
                        {
                            if (magnitude < 0)
                            {
                                if (enemyStats.health - magnitude < enemyStats.maxHealth)
                                {
                                    enemyStats.health -= magnitude;
                                }
                                else
                                {
                                    enemyStats.health = enemyStats.maxHealth;
                                }
                            }
                        }
                        else
                        {
                            enemyStats.health -= magnitude;
                        }
                        break;
                    }
                case buffType.TowerUpDamagePercent:
                    {
                        if (towerStats != null)
                        {
                            towerStats.damage *= 1 + (magnitude / 100);
                        }
                        break;
                    }
            }
        }
    }


    //ends the effect of a buff.
    public void BuffEnd(GameObject target)
    {
        Script_Enemy_Move enemyMove = target.GetComponent<Script_Enemy_Move>();
        Script_Enemy_Health enemyStats = target.GetComponent<Script_Enemy_Health>();
        Script_Tower towerStats = target.GetComponent<Script_Tower>();

        switch (type)
        {
            case buffType.doNothing:
                    {
                        break;
                    }
            case buffType.EnemyReduceSpeedPercent:
                {
                    if (enemyMove != null)
                    {
                        enemyMove.speed *= (100 / (100 - magnitude)); //reverses the effect of slowing.
                    }
                }
                break;
            case buffType.EnemyStun:
                {
                    if (enemyMove != null)
                    {
                        enemyMove.stunned = false;
                    }
                }
                break;
            case buffType.TowerStun:
                {
                    if (towerStats != null)
                    {
                        towerStats.stunned = false;
                    }
                }
                break;
            case buffType.TowerUpDamagePercent:
                {
                    if (towerStats != null)
                    {
                        towerStats.damage *= (100 / (100 + magnitude) );
                    }
                    break;
                }

        }
    }

    //updates the state of an applied buff.
    public void BuffUpdate(GameObject target)
    {
        Script_BuffList targetBuffs = target.GetComponent<Script_BuffList>(); //reference to the entity's buff list.


        if (duration <= 0) //if the buff has expired...
        {
            BuffEnd(target); //ends the effect
            targetBuffs.buffs.Remove(this); //removes the buff
        }

        if (triggered == false) //if the buff is attached to the target, but hasn't activated yet...
        {
            ApplyBuffEffect(target); //applies the effect
            triggered = true; //sets the buff as triggered.
        }

        if (repeating == true) //if this is a 'repeating' buff (applies an effect every second)..
        {
            DOTtimer -= Time.deltaTime; //reduces DOT timer
            if (DOTtimer <= 0) //if it is time to apply the effect again...
            {
                ApplyBuffEffect(target); //applies the effect.
                DOTtimer = 1; //resets the DOT timer.
            }
        }
        duration -= Time.deltaTime; //reduces the buff's duration.
    }

    public string ParseToString()
    {
        string answer = " ";
        switch (type)
        {
            case (buffType.EnemyReduceSpeedPercent):
                {
                    answer = "Slow: " + magnitude.ToString() + "%, " + duration.ToString("F1") + "s";
                    break;
                }
            case (buffType.TowerUpDamagePercent):
                {
                    answer = "Boost Damage: " + magnitude.ToString("F1") + "%, " + duration.ToString("F1") + "s";
                    break;
                }
            case (buffType.DealDOT):
                {
                    if (magnitude < 0)
                    {
                        answer = "Heal: " + Mathf.Abs(magnitude).ToString("F1") + " hp/s, " + duration.ToString("F1") + "s";
                    }
                    else if (magnitude > 0)
                    {
                        answer = "DOT: " + magnitude.ToString("F1") + " hp/s, " + duration.ToString("F1") + "s";
                    }
                    break;
                }
        }
        return answer;
    }
}

