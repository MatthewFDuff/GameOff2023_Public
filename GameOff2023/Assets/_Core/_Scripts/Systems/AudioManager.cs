using UnityEngine;

namespace _Core._Scripts.Systems
{
    /// <summary>
    /// A singleton that manages the audio in the game.
    ///
    /// Reference: https://www.youtube.com/watch?v=tEsuLTpz_DU
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectsSource;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        
        public void PlayMusic(AudioClip clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
        }
        
        public void PlayEffect(AudioClip clip)
        {
            _effectsSource.PlayOneShot(clip);
        }
    }
}