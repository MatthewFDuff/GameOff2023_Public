using HeathenEngineering;
using HeathenEngineering.Events;
using Unity.Mathematics;
using UnityEngine;

namespace _Core._Scripts.Utility
{
    /// <summary>
    /// Used to update the position of an object when the position value changes.
    /// </summary>
    public class UpdatePosition : MonoBehaviour
    {
        [SerializeField] private Vector2Variable _position;

        private void OnEnable()
        {
            if (_position == null) return;
            
            _position.AddListener(SetPosition);
        }

        private void OnDisable()
        {
            if (_position == null) return;
            
            _position.RemoveListener(SetPosition);
        }

        /// <summary>
        /// Called when the position value changes.
        /// </summary>
        private void SetPosition(EventData<float2> eventData)
        {
            if(_position == null) return;
            
            transform.position = new Vector3(_position.Value.x, _position.Value.y, transform.position.z);
        }
    }
}
