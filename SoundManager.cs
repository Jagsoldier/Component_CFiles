using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //instantiating audiosource array 
    public AudioSource[] audioSources;

    //declaring a soundmanager instance for singleton
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public void PlaySFX(int SFX, float volume)
    {
        if (audioSources[SFX] != null)
        {
            audioSources[SFX].volume = volume;
            audioSources[SFX].pitch = Random.Range(0.7f, 1.0f);
            audioSources[SFX].Play();
        }
        else
            Debug.Log(audioSources[SFX] + "does not exist!");
    }
}
