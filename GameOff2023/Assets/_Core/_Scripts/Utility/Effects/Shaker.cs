using UnityEngine;

namespace _Core._Scripts.Utility.Effects
{
    /// <summary>
    /// Provides an easy way to trigger a camera shake without any additional logic for why the shake is occurring. 
    /// </summary>
    public class Shaker : MonoBehaviour
    {
        private CameraShake _cameraShake;

        /// <summary>
        /// Cache the camera shake component if it exists. 
        /// </summary>
        private void Start()
        {
            if (Camera.main != null) _cameraShake = Camera.main.GetComponent<CameraShake>();

            if (_cameraShake == null)
            {
                Debug.Log("CameraShake could not be found. Please add the CameraShake component to the main camera.");
            }
        }

        /// <summary>
        /// Public to allow Unity events to call this method and trigger a camera shake.
        /// That's why we call this class the Shaker, because it shakes.
        /// </summary>
        public void ShakeTheCamera()
        {
            _cameraShake.CamShake();
        }

    }
}
