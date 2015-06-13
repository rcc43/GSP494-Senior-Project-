using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class Script_LevelButton : MonoBehaviour
{

    public GameObject MenuController;

    public Script_MenuController controller;

    public LevelLayout level;

    EventTrigger trigger;

    GameObject CampaignSave;
    Script_CampaignData CampaignData;

    // Use this for initialization
    void Start()
    {
        MenuController = GameObject.FindWithTag("MenuController");
        controller = MenuController.GetComponent<Script_MenuController>();

        CampaignSave = GameObject.FindWithTag("CampaignDataSave");
        CampaignData = CampaignSave.GetComponent<Script_CampaignData>();

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
        if (!level.campaign)
        {
            CampaignData.isCampaign = false;
            Application.LoadLevel(level.loadName[0]);
        }
        else
        {
            CampaignData.isCampaign = true;
            CampaignData.campaignQueue = level.loadName;
            CampaignData.enableBoss = level.enableBoss;
            Application.LoadLevel(level.loadName[0]);
        }
    }

}