using UnityEngine;

namespace _Core._Scripts.Utility
{
    [ExecuteInEditMode]
    public class FindMissingReferences : MonoBehaviour
    {
        private void Start()
        {
            CheckForMissingReferences();
        }

        [ContextMenu("Check For Missing References")]
        private void CheckForMissingReferences()
        {
            // Iterate through all GameObjects in the scene
            GameObject[] allGameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            foreach (var go in allGameObjects)
            {
                // Check for missing references in each component of the GameObject
                Component[] components = go.GetComponents<Component>();

                foreach (var component in components)
                {
                    if (component == null)
                    {
                        Debug.LogError($"Missing reference in GameObject: {go.name}");
                    }
                }
            }
        }
    }
}