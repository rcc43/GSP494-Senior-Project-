using UnityEngine;
using System.Collections;

public class Script_PlayMusic : MonoBehaviour
{

    public AudioClip[] music;
    public AudioSource player;
    float clipTimer;
    float musicStartTime;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<AudioSource>();
        int randClip = Random.Range(0, music.Length);
        player.clip = music[randClip];
        musicStartTime = Time.realtimeSinceStartup;
        player.Play();
        clipTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        clipTimer = Time.realtimeSinceStartup - musicStartTime;
        if (clipTimer >= player.clip.length + 5.0f)
        {
            PlayMusic();
        }
	}

    void PlayMusic()
    {
        int randClip = Random.Range(0, music.Length);
        player.clip = music[randClip];
        musicStartTime = Time.realtimeSinceStartup;
        clipTimer = 0.0f;
        player.Play();
    }
}
