using UnityEngine;
using UnityEngine.UI;

namespace _Core._Scripts
{
    public class VolumeIconController : MonoBehaviour 
    {
        #region Inspector Fields
        
        [Header("References")]
        [Tooltip("Reference to the volume manager component.")]
        [SerializeField] private VolumeManager _volumeManager;
        
        [Header("UI Elements")]
        [Tooltip("The image that displays the volume icon.")]
        [SerializeField] private Image _iconImage;
        
        [Header("Volume Icons")]
        [Tooltip("The sprite that is displayed when the volume is at its maximum value.")]
        [SerializeField] private Sprite _maxVolumeSprite;
        
        [Tooltip("The sprite that is displayed between the maximum and minimum volume value.")]
        [SerializeField] private Sprite _halfVolumeSprite;
        
        [Tooltip("The sprite that is displayed when the volume is at its minimum value.")]
        [SerializeField] private Sprite _minVolumeSprite;

        #endregion 

        #region Lifecycle Methods

        private void Update()
        {
            UpdateVolumeIcon(_volumeManager.VolumeSlider.value); // Initial update based on the current volume value
        }

        #endregion

        #region Main Methods

        private void UpdateVolumeIcon(float volumeValue)
        {
            // Determine the volume level and update the icon accordingly
            if(volumeValue == _volumeManager.VolumeSlider.minValue)
            {
                _iconImage.sprite = _minVolumeSprite;
            }
            else if (volumeValue > _volumeManager.VolumeSlider.maxValue - 0.25 )
            {
                _iconImage.sprite = _maxVolumeSprite;
            }
            else 
            {
                _iconImage.sprite = _halfVolumeSprite;
            }
        }

        #endregion
    
    }
}
