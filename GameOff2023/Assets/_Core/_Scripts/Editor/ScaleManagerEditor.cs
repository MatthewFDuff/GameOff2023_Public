using _Core._Scripts.Systems;
using UnityEditor;

namespace _Core._Scripts.Editor
{
    [CustomEditor(typeof(ScaleManager))]
    public class ScaleManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            ScaleManager scaleManagerEditor = (ScaleManager)target;
            
            // TODO: Remove this. It was previously used but now no longer needed.
        }
    }
}
