using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SFXsource;

    [Header("---------- Audio Clip ----------")]
    public AudioClip standardMole;
    public AudioClip jesterMole;
    public AudioClip kingMole;
    public AudioClip startUp;

    private void Start()
    {
        SFXsource.clip = startUp;
        SFXsource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXsource.PlayOneShot(clip);
    }
}
