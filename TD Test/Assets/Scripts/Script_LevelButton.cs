using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Script_LevelButton : MonoBehaviour
{

    public GameObject MenuController;

    public Script_MenuController controller;

    public LevelLayout level;

    EventTrigger trigger;

    // Use this for initialization
    void Start()
    {
        MenuController = GameObject.FindWithTag("MenuController");
        controller = MenuController.GetComponent<Script_MenuController>();

        trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry onEnter = new EventTrigger.Entry();
        onEnter.eventID = EventTriggerType.PointerEnter;
        onEnter.callback.AddListener((eventData) => { ChangeText(); });
        trigger.delegates.Add(onEnter);

        EventTrigger.Entry onClick = new EventTrigger.Entry();
        onClick.eventID = EventTriggerType.PointerClick;
        onClick.callback.AddListener((eventData) => { LoadLevel();  });
        trigger.delegates.Add(onClick);
    }

    void ChangeText()
    {
        controller.nameText.text = level.name;
        controller.descText.text = level.description;
    }

    void LoadLevel()
    {
        Application.LoadLevel(level.loadName);
    }

}