using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Formation
{

    public GameObject[] members;
    public List<int>choices;
    public int[] positions;

    public void Init(int size)
    {
        members = new GameObject[size];
        choices = new List<int>();
        positions = new int[size];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = 0;
        }
    }
	
    public bool IsFirst(GameObject tgt)
    {
        for (int i = 0; i < members.Length; i++)
        {
            if (members[i] == tgt)
            {
                if (positions[i] >= choices.Count)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsMember(GameObject tgt)
    {
        for (int i = 0; i < members.Length; i++)
        {
            if (members[i] == tgt)
            {
                return true;
            }
        }
        return false;
    }

    public int GetChoice(GameObject tgt)
    {
        for (int i = 0; i < members.Length; i++)
        {
            if (members[i] == tgt)
            {
                return choices[positions[i]];
            }
        }
        return 0;
    }

    public void AddChoice(int tgt)
    {
        choices.Add(tgt);
    }

    public void positionUp(GameObject tgt)
    {
        for (int i = 0; i < members.Length; i++)
        {
            if (members[i] == tgt)
            {
                positions[i]++;
            }
        }
    }

    public bool IsLast()
    {
        int memcount = 0;
        for (int i = 0; i < members.Length; i++)
        {
            if (members[i] != null)
            {
                memcount++;
            }
        }
        if (memcount == 1)
            return true;
        else return false;
    }
}
