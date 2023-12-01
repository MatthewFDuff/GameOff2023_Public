using UnityEngine;

namespace _Core._Scripts.Data
{
    /// <summary>
    /// The data for each dragon is used to customize the appearance of each level.
    /// Water = Blue
    /// Fire = Red
    /// Air = Green
    /// Earth = Brown
    /// Omni = Purple/White
    ///
    /// The colour of each type of scales is changed to reflect the dragon type. 
    /// </summary>
    [CreateAssetMenu(fileName = "New Dragon", menuName = "Custom/Dragon")]
    public class DragonData : ScriptableObject
    {
        #region Properties

        /// <summary>
        /// The name of the dragon. Not really used anywhere in the game, but it's nice to have.
        /// </summary>
        public string Name => _name;
        
        /// <summary>
        /// A description of the dragon. Not really used anywhere in the game, but it's nice to have.
        /// </summary>
        public string Description => _description;
        
        public Color BorderScaleColor => _borderScaleColor;
        public Color CleanScaleColor => _cleanScaleColor;
        public Color DirtyScaleColor => _dirtyScaleColor;
        #endregion

        #region Private Variables
        
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Color _borderScaleColor;
        [SerializeField] private Color _cleanScaleColor;
        [SerializeField] private Color _dirtyScaleColor;
        

        #endregion

        #region Lifecycle Methods

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_name))
            {
                _name = name;
            }
        }

        #endregion
    }
}
