using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_Infocard : MonoBehaviour {

    public Text[] text;
    public GameObject representedTower_Prefab;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void UpdateData()
    {
        Script_Tower representedTowerData = representedTower_Prefab.GetComponent<Script_Tower>();

        text = gameObject.GetComponentsInChildren<Text>();
        if (representedTowerData)
        {
            text[0].text = representedTowerData.towerName;
            text[1].text = representedTowerData.description;
            text[2].text = "Damage: " + representedTowerData.damage.ToString("F1") + "\nRange: " + representedTowerData.range.ToString("F1")
                + "\nSpeed: " + representedTowerData.fireRate.ToString("F1") + " shots/s \n\nEffects:\n" + representedTowerData.buff.ParseToString();
        }
    }
}
