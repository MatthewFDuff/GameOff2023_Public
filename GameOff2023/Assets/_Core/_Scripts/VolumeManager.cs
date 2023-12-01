using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace _Core._Scripts
{
    public class VolumeManager : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// The volume slider that this volume manager controls.
        /// </summary>
        public Slider VolumeSlider => _volumeSlider;
        
        /// <summary>
        /// The logarithmic volume value of the volume slider.
        /// </summary>
        public float LogVolume => Mathf.Log10(_volumeSlider.value) * 20;

        #endregion

        #region Inspector Fields
        [Header("UI Elements")]
        [Tooltip("The volume slider component that controls the volume.")]
        [SerializeField] private Slider _volumeSlider;
        
        [Header("Volume Manager Settings")]
        [Tooltip("The name of the Player Prefs key that stores the volume value.")]
        [SerializeField] private string _prefKey;
        
        [Tooltip("The audio mixer group that the volume slider controls.")]
        [SerializeField] private AudioMixer _mixerGroup;
        
        #endregion

        #region Lifecycle Methods

        private void Start()
        {
            // If the Player Prefs key does not exist, create it and set the volume to 1.
            if (!PlayerPrefs.HasKey(_prefKey))
            {
                PlayerPrefs.SetFloat(_prefKey, 1f);
                Load();
            }
            else
            {
                Load();
            }
        }

        #endregion

        #region Main Methods
        
        /// <summary>
        /// Updates the volume of the mixer group and saves the new value to the Player Prefs.
        /// </summary>
        public void ChangeVolume()
        {
            _mixerGroup.SetFloat(_prefKey, LogVolume);
            Save();
        }

        /// <summary>
        /// Loads the volume value from the Player Prefs.
        /// </summary>
        private void Load()
        {
            _volumeSlider.value = PlayerPrefs.GetFloat(_prefKey);
            ChangeVolume();
        }

        /// <summary>
        /// Saves the current volume value to the Player Prefs.
        /// </summary>
        private void Save()
        {
            PlayerPrefs.SetFloat(_prefKey, _volumeSlider.value);
        }

        #endregion
    }
}

