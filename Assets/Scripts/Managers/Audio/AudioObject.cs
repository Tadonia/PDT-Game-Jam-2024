using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioObject", menuName = "ScriptableObjects/AudioObject")]
public class AudioObject : ScriptableObject
{
    public AudioClip[] audioClips;
    public float volume;

    public AudioInstance PlayAudio(Vector3 position)
    {
        return GameManager.Instance.AudioManager.PlayAudio(this, position);
    }
}
