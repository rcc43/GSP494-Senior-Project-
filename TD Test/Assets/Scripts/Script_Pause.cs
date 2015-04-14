using UnityEngine;
using System.Collections;

public class Script_Pause : MonoBehaviour {

    bool paused = false;
    public float timeScalingEffect = 0;

    public void OnClick()
    {
        if (Time.timeScale != timeScalingEffect)
        {
            Time.timeScale = timeScalingEffect;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
    }
	
}
