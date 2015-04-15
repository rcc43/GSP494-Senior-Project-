using UnityEngine;
using System.Collections;

public class Script_HealthBar : MonoBehaviour {

    public float maxHealth;
    public float health;

    RectTransform healthLeft;

	// Use this for initialization
	void Start ()
    {
        healthLeft = transform.FindChild("Health").gameObject.GetComponent<RectTransform>();
        ResizeBar();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (health >= 0)
        {
            Vector3 scale = healthLeft.localScale;
            scale.x = (health / maxHealth);
            healthLeft.localScale = scale;
        }
	}

    public void ResizeBar()
    {
        healthLeft = transform.FindChild("Health").gameObject.GetComponent<RectTransform>();
        RectTransform trans = gameObject.GetComponent<RectTransform>(); //gathers the transform of the health background.
        RectTransform healthChild = transform.FindChild("Health/HealthLeft").GetComponent<RectTransform>(); //resizes health-left bar to match background.
        healthChild.sizeDelta = trans.sizeDelta;
    }
}
