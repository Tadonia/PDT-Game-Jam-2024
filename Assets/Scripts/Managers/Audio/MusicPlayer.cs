using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip musicClip;

    private void Start()
    {
        GameManager.Instance.MusicManager.PlayMusic(musicClip);
    }
}
