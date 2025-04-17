using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerMole : MonoBehaviour
{
    [SerializeField] AudioSource SFXsource;
    [SerializeField] AudioSource BGMsource;

    [Header("---------- Music Clip ----------")]
    public AudioClip moleMusic; 

    [Header("---------- Audio Clip ----------")]
    public AudioClip standardMole;
    public AudioClip jesterMole;
    public AudioClip kingMole;
    public AudioClip startUp;

    private void Start()
    {
        SFXsource.clip = startUp;
        BGMsource.clip = moleMusic;
        SFXsource.Play();
        BGMsource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXsource.PlayOneShot(clip);
    }
}
