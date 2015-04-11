using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Script_TowerBuildButton : MonoBehaviour {

    public Text[] text;
    public GameObject infoCard_Prefab;
    public GameObject representedTower_Prefab;

    GameObject infoCard;

    bool drawingCard = false;

    EventTrigger trigger;

	// Use this for initialization
	void Start ()
    {

        trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry onEnter = new EventTrigger.Entry();
        EventTrigger.Entry onLeave = new EventTrigger.Entry();
        onEnter.eventID = EventTriggerType.PointerEnter;
        onLeave.eventID = EventTriggerType.PointerExit;
        onEnter.callback.AddListener((eventData) => { DrawCard(); });
        onLeave.callback.AddListener((eventData) => { UndrawCard(); });
        trigger.delegates.Add(onEnter);
        trigger.delegates.Add(onLeave);


	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateData(Script_Tower towerType)
    {
        text = gameObject.GetComponentsInChildren<Text>();
        text[0].text = towerType.towerName;
        text[1].text = towerType.cost.ToString();
    }

    public void DrawCard()
    {
        if (!drawingCard)
        {
            infoCard = Instantiate(infoCard_Prefab) as GameObject;
            RectTransform trans = infoCard.GetComponent<RectTransform>();
            trans.anchoredPosition = new Vector2(595, 400);
            trans.SetParent(transform.parent);

            Script_Infocard data = infoCard.GetComponent<Script_Infocard>();
            if (data != null)
            {
                data.representedTower_Prefab = representedTower_Prefab;
                data.UpdateData();
            }

            drawingCard = true;
        }
    }

    public void UndrawCard()
    {
        if (drawingCard)
        {
            Destroy(infoCard);
            drawingCard = false;
        }
    }
}
