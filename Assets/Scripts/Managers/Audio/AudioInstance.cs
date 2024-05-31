using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInstance : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void PlayAudio(AudioObject audioObject)
    {
        AudioClip clip = audioObject.audioClips[Random.Range(0, audioObject.audioClips.Length)];
        audioSource.clip = clip;
        audioSource.volume = audioObject.volume;
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
