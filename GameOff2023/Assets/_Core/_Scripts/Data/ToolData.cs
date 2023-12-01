using UnityEngine;

namespace _Core._Scripts.Data
{
    [CreateAssetMenu(fileName = "New Tool", menuName = "Custom/Tool")]
    public class ToolData : ScriptableObject
    {
        /// <summary>
        /// The name of the tool.
        /// </summary>
        public string Name => _name;
        
        /// <summary>
        /// The description of the tool.
        /// </summary>
        public string Description => _description;
        
        public Tools ToolType => _toolType;
        
        /// <summary>
        /// The sprite of the tool that is displayed in the UI.
        /// </summary>
        public Sprite Sprite => _sprite;
        
        /// <summary>
        /// The amount of cleaning power the tool has when used on a scale.
        /// </summary>
        public int Damage => _damage;
        
        /// <summary>
        /// True, when the tool is unlocked and can be used by the player.
        /// </summary>
        public bool IsUnlocked => _isUnlocked;

        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Tools _toolType;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private int _damage;
        
        [SerializeField] private bool _isUnlocked;

        #region Lifecycle Methods

        /// <summary>
        /// Set initial values.
        /// </summary>
        private void OnValidate()
        {
            // Set the name value to the name of the object.
            if (string.IsNullOrEmpty(_name))
            {
                _name = name;
            }
        }

        #endregion
    }
}