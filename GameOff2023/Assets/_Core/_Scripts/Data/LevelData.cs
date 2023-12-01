using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Core._Scripts.Data
{
    [Serializable]
    public struct LevelScaleData
    {
        /// <summary>
        /// The type of the scale that is generated in this level.
        /// </summary>
        public ScaleType ScaleType;
        
        /// <summary>
        /// The percentage of scale of this type that will appear in the level. Must be a value between 0 and 100.
        /// </summary>
        [Range(0, 100)]
        public int Percentage;
    }
    
    /// <summary>
    /// Contains all data required for a single level in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "New Level", menuName = "Custom/Level")]
    public class LevelData : ScriptableObject
    {
        #region Properties
        
        /// <summary>
        /// The name of the level (e.g. "Level 1", "A New Beginning", etc.)
        /// </summary>
        public string Name => _name;
        
        /// <summary>
        /// A description of the level (e.g. "Learn how to use the brush to clean the dragon!")
        /// </summary>
        public string Description => _description;
        
        /// <summary>
        /// The data for the dragon that determines the appearance of the level.
        /// </summary>
        public DragonData DragonData => _dragonData;
        
        /// <summary>
        /// Whether or not the level is unlocked yet.
        /// </summary>
        public bool IsUnlocked => _isUnlocked;

        /// <summary>
        /// The quantity and variety of scales that are randomly generated in this level.
        /// e.g.
        /// 30 x Clean Scales
        /// 25 x Dirty Scales
        /// 15 x Fire Scales
        /// </summary>
        public List<LevelScaleData> ScaleTypeDistribution => _scaleTypeDistribution;
        
        #endregion

        #region Private Variables
        
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private bool _isUnlocked;
        [SerializeField] private DragonData _dragonData;
        [SerializeField] private List<LevelScaleData> _scaleTypeDistribution = new();
        [SerializeField] private bool _is100Percent;
        #endregion
        
        #region Lifecycle Methods
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_name))
            {
                _name = name;
            }
            
            if (_scaleTypeDistribution.Count == 0)
            {
                _scaleTypeDistribution.Add(new LevelScaleData
                {
                    ScaleType = ScaleType.Dirty,
                    Percentage = 100
                });
            }
            
            // Ensure that the percentage values add up to 100.
            var totalPercentage = 0;
            foreach (var levelScaleData in _scaleTypeDistribution)
            {
                totalPercentage += levelScaleData.Percentage;
            }

            _is100Percent = totalPercentage == 100;
        }
        
        #endregion
    }
    
}
