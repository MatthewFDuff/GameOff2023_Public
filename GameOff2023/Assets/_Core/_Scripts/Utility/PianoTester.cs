using System.Collections;
using UnityEngine;

public class PianoTester : MonoBehaviour
{
    public AudioClip[] SoundFiles;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Check if there are sound files assigned
        if (SoundFiles.Length == 0)
        {
            Debug.LogError("No sound files assigned!");
            return;
        }

        StartCoroutine(PlaySoundsWithDelay());
    }

    IEnumerator PlaySoundsWithDelay()
    {
        foreach (var soundFile in SoundFiles)
        {
            // Play the current sound file
            _audioSource.clip = soundFile;
            _audioSource.Play();

            // Debug.Log("Playing sound: " + soundFile.name);
            // Wait for 0.5 seconds before playing the next sound
            yield return new WaitForSeconds(0.5f);
        }
    }
}

