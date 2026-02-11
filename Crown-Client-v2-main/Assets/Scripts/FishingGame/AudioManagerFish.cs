using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerFish : MonoBehaviour
{
    //Audio sources for the sound effects and the Background Music
    [SerializeField] AudioSource SFXsource;
    [SerializeField] AudioSource BGMsource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip bgm;

    [Header("---------- Audio Clip ----------")]
    public AudioClip fishCaught;
    public AudioClip lineCast;
    public AudioClip startUp;

    private void Start()
    {
        SFXsource.clip = startUp;
        BGMsource.clip = bgm;
        SFXsource.Play();
        BGMsource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXsource.PlayOneShot(clip);
    }
}

