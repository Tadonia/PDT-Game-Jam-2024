using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] GameObject audioInstancePrefab;
    [SerializeField] int numOfInstances = 25;

    Queue<AudioInstance> audioInstances = new Queue<AudioInstance>();

    private void Awake()
    {
        for (int i = 0; i < numOfInstances; i++)
        {
            AudioInstance instance = Instantiate(audioInstancePrefab).GetComponent<AudioInstance>();
            audioInstances.Enqueue(instance);
        }
    }

    public AudioInstance PlayAudio(AudioObject audioObject, Vector3 position)
    {
        AudioInstance audioInstance = audioInstances.Dequeue();
        audioInstance.transform.position = position;
        audioInstance.PlayAudio(audioObject);
        audioInstances.Enqueue(audioInstance);
        return audioInstance;
    }
}
