using UnityEngine;
using UnityEngine.Events;

namespace _Core._Scripts
{
    /// <summary>
    /// Provides an easy way to make an object with a collider interactable, regardless of hierarchy.
    /// e.g.
    ///     RootObject
    ///         - SpriteGameObject
    ///         - ColliderGameObject
    ///
    /// This configuration allows ColliderGameObject to call methods from RootObject.
    /// </summary>
    public class Interactable : MonoBehaviour
    {
        /// <summary>
        /// The events to raise when the player interacts with this object.
        /// </summary>
        public UnityEvent OnInteractEvents;
    
        /// <summary>
        /// When the player clicks on this object, the interaction events are invoked.
        /// </summary>
        private void OnMouseDown()
        {
            OnInteractEvents.Invoke();
        }
    }
}
