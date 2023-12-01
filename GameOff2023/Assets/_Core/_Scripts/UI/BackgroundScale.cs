using UnityEngine;

namespace _Core._Scripts.UI
{
    /// <summary>
    /// Background scales are used in the main menu and pan from right to left.
    /// When they reach the left side of the screen, they are teleported to the right side of the screen.
    /// Based on the viewport size.
    /// </summary>
    public class BackgroundScale : MonoBehaviour
    {
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float viewportRightSide = 10f;
        
        private void Start()
        {
            // Get the position of the right side of the screen.
            viewportRightSide = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        }

        private void Update()
        {
            transform.position += Vector3.left * (_speed * Time.deltaTime);
        
            if (transform.position.x < -1f)
            {
                var teleportPosition = new Vector3(viewportRightSide + 1f, 0, 0);
                transform.position += teleportPosition;
            }
        }
    }
}
