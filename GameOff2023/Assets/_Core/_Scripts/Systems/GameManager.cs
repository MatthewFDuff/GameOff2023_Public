using System;
using System.Collections.Generic;
using _Core._Scripts.Data;
using HeathenEngineering;
using UnityEngine;

namespace _Core._Scripts.Systems
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Tooltip("The list of all resources in the game, so they can be easily reset on game start.")]
        [SerializeField] public List<IntVariable> _resourceVariables = new();
        [SerializeField] private List<UpgradeData> _upgrades = new();
        
        #region Lifecycle Methods
    
        private void Awake()
        {
            Instance = this;

            SetDefaultValues();
        }

        #endregion

        #region General
        
        public int CurrentMoney
        {
            get => _currentMoney.Value;
            set
            {
                if (value < 0) return;
                _currentMoney.Value = value;
            }
        }
        
        [SerializeField] private IntVariable _currentMoney;

        #endregion

        /// <summary>
        /// Resets all key values to their defaults at the start of the game.
        /// </summary>
        private void SetDefaultValues()
        {
            // Upgrades
            foreach (UpgradeData upgrade in _upgrades)
            {
                try
                {
                    upgrade.Initialize();

                }
                catch (Exception exception)
                {
                    Debug.Log("Error initializing upgrade: " + upgrade.name + "\n" + exception.Message + "\n" + exception.StackTrace);
                }
            }
            
            // Resources
            foreach (IntVariable resource in _resourceVariables)
            {
                resource.Value = 0;
            }
        }
    }
}
