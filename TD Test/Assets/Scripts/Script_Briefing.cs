using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Script_Briefing : MonoBehaviour
{
    public Script_GameController controller;
    public List<string> page;
    int pageNum = 0;
    Text text;
    Button up;
    Button down;
    public bool live;

	// Use this for initialization
	void Start ()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Script_GameController>();
        text = gameObject.GetComponentInChildren<Text>();
        text.text = page[0];
        live = true;

        Button[] buttons = gameObject.GetComponentsInChildren<Button>();
        up = buttons[0];
        down = buttons[1];

        up.gameObject.SetActive(false);
        if (page.Count == 1)
        {
            down.gameObject.SetActive(false);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PageUp()
    {
        if (page.Count - 1 > pageNum)
        {
            pageNum++;
            text.text = page[pageNum];
            up.gameObject.SetActive(true);
            if (page.Count - 1 > pageNum)
            {
                down.gameObject.SetActive(true);
            }
            else
            {
                down.gameObject.SetActive(false);
            }
        }
    }

    public void PageDown()
    {
        if (pageNum > 0)
        {
            pageNum--;
            text.text = page[pageNum];
            down.gameObject.SetActive(true);
            if (pageNum > 0)
            {
                up.gameObject.SetActive(true);
            }
            else
            {
                up.gameObject.SetActive(false);
            }
        }
    }

    public void Close()
    {
        live = false;
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
