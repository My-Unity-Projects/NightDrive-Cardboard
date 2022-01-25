using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attach this script to any gameobject you want to keep through scenes
public class MusicSource : MonoBehaviour
{
    private AudioSource _audioSource;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        _audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
