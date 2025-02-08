using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private Dictionary<string, AudioSource> _activeSounds = new Dictionary<string, AudioSource>();

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayAudio(AudioClip clip, string key, float volume, bool loop)
    {

        if (_activeSounds.ContainsKey(key))
        {
            Debug.Log("Stopping previous audio: " + key);
            StopAudio(key);
        }
        // Create new audio object
        GameObject audioObject = new GameObject(key);
        audioObject.transform.SetParent(transform); // Keep hierarchy clean

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();

        // Store the new AudioSource in the dictionary
        _activeSounds[key] = audioSource;

        // If it's not looping, start a coroutine to remove it when finished
        if (!loop)
        {
            StartCoroutine(CheckAudio(key, audioSource));
        }
    }

    // Function to stop audio
    public void StopAudio(string key)
    {
        Debug.Log("Key:" + key);
        if (_activeSounds.ContainsKey(key))
        {
            AudioSource source = _activeSounds[key];
            if (source != null)
            {
                source.Stop(); // Stop the audio
                Destroy(source.gameObject); // Destroy the GameObject
            }

            _activeSounds.Remove(key); // Remove from dictionary
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
