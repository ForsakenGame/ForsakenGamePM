using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private List<GameObject> _activeSounds;

    private void Awake()
    {
        if(!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            _activeSounds = new List<GameObject>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public AudioSource PlayAudio(AudioClip clip, string objectName, float volume, bool isLoop)
    {
        GameObject audioObject = new GameObject(objectName);
        AudioSource audioSourceComponent = audioObject.AddComponent<AudioSource>();
        audioSourceComponent.clip = clip;
        audioSourceComponent.volume = volume;
        audioSourceComponent.loop = isLoop;

        audioSourceComponent.Play();

        if (!isLoop)
        {
            _activeSounds.Add(audioObject);
            StartCoroutine(CheckAudio(audioSourceComponent));
        }

        return audioSourceComponent;
    }


    IEnumerator CheckAudio(AudioSource audioSource)
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }
        _activeSounds.Remove(audioSource.gameObject);
        Destroy(audioSource.gameObject);
    }
}
