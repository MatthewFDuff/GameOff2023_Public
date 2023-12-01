using UnityEngine;

namespace _Core._Scripts
{
    public class DraggableItem : MonoBehaviour
    {
        private Vector3 originalPosition;
        private bool isSelected;

        // Update is called once per frame
        void Update()
        {
            if (isSelected)
            {
                Vector3 mousePosition = Input.mousePosition;
                mousePosition.z = 10; // distance from the camera
                transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            }
        }

        private void OnMouseDown()
        {
            isSelected = true;
        }

        private void OnMouseUp()
        {
            isSelected = false;
            transform.position = originalPosition;
        }

        void Start()
        {
            originalPosition = transform.position;
        }
    }
}
