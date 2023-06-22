using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public AudioSource AudioSource => audioSource;
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
        Destroy(gameObject, audioClip.length);
    }
}
