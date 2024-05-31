using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioObject", menuName = "ScriptableObjects/AudioObject")]
public class AudioObject : ScriptableObject
{
    public AudioClip[] audioClips;
    public float volume;

    public void PlayAudio(Vector3 position)
    {
        GameManager.Instance.AudioManager.PlayAudio(this, position);
    }
}
