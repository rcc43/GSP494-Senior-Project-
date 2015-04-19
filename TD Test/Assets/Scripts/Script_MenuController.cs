using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Script_MenuController : MonoBehaviour
{

    public LevelLayout[] levels;
    public GameObject mainMenuLayer;
    public GameObject levelMenuLayer;
    public Canvas UICanvas;

    public float buttonStartX;
    public float buttonStartY;
            
    public float buttonOffsetX;
    public float buttonOffSetY;

    public Text nameText;
    public Text descText;

    public GameObject levelButton_prefab;
    
	// Use this for initialization
	void Start () {

        for (int i = 0; i < levels.Length; i++)
        {
            GameObject button = Instantiate(levelButton_prefab) as GameObject;
            RectTransform buttonPos = button.GetComponent<RectTransform>();
            buttonPos.anchoredPosition = new Vector2(buttonStartX + (i * buttonOffsetX), buttonStartY);
            Text text = button.GetComponentInChildren<Text>();
            text.text = levels[i].name;
            Script_LevelButton buttonData = button.GetComponent<Script_LevelButton>();
            buttonData.level = levels[i];

            buttonPos.SetParent(levelMenuLayer.transform);
        }
	
	}

    public void Quit()
    {
        Application.Quit();
    }
}
