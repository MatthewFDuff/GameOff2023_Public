using UnityEngine;


// Reference: https://stackoverflow.com/questions/68424961/detecting-a-click-on-a-specific-tile-in-a-tilemap
namespace _Core._Scripts
{
    public class DetectClickOnTile : MonoBehaviour
    {
        Vector2 worldPoint;
        RaycastHit2D hit;

        // Update is called once per frame
        void Update()
        {
            worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(worldPoint, Vector2.down);

                if (hit.collider != null)
                {
                    Debug.Log("click on " + hit.collider.name);
                    Debug.Log(hit.point);
                }
            }
        }
    }
}