using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip AudioClip;
    void Start()
    {
        AudioSource.PlayOneShot(AudioClip);
    }
}
