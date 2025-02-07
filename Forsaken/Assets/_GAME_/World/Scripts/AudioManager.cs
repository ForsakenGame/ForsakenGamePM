using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private Dictionary<string, AudioSource> _activeSounds = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlayAudio(AudioClip clip, string key, float volume, bool loop)
    {
        if (_activeSounds.ContainsKey(key)) // Prevent stacking
        {
            return;
        }

        GameObject audioObject = new GameObject("Audio_" + key);
        audioObject.transform.SetParent(transform); // Keep hierarchy clean

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();

        _activeSounds[key] = audioSource;

        if (!loop)
        {
            StartCoroutine(CheckAudio(key, audioSource));
        }
    }

    public void StopAudio(string key)
    {
        if (_activeSounds.ContainsKey(key))
        {
            AudioSource source = _activeSounds[key];
            source.Stop();
            Destroy(source.gameObject); // Destroy the GameObject properly
            _activeSounds.Remove(key);
        }
    }

    public bool IsPlaying(string key)
    {
        return _activeSounds.ContainsKey(key) && _activeSounds[key].isPlaying;
    }

    private IEnumerator CheckAudio(string key, AudioSource audioSource)
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);

        if (_activeSounds.ContainsKey(key))
        {
            _activeSounds.Remove(key);
        }

        Destroy(audioSource.gameObject);
    }
}
