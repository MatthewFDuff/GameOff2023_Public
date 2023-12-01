using System.Collections.Generic;
using _Core._Scripts.Data;
using _Core._Scripts.Data.Variables;
using HeathenEngineering;
using HeathenEngineering.Events;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Core._Scripts.Systems
{
    
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelData> _levels;
        
        [SerializeField] private IntVariable _currentLevel;
        [SerializeField] private LevelVariable _currentLevelData;

        /// <summary>
        /// Levels that can be used to randomly generate an infinite amount of levels.
        /// </summary>
        [SerializeField] private List<LevelData> _infiniteLevels;

        [Header("Boss")] 
        [Tooltip("The level that is used as the boss level.")]
        [SerializeField] private LevelData _bossLevel;
        [SerializeField] private Button _bossLevelButton;
        [SerializeField] private GameObject _bossTimerGameObject;
        [SerializeField] private FloatVariable _bossTimer;
        [SerializeField] private float _bossTimerMax = 60f;
        private bool _isFightingBoss = false;
        private bool _bossLevelDefeated = false;
        
        [Header("Dragon")]
        [SerializeField] private DragonVariable _currentDragonData;

        [Header("Events")]
        [SerializeField] private GameEvent OnLevelStarted;
        [SerializeField] private GameEvent OnBossLevelWon;
        [SerializeField] private GameEvent OnBossLevelLost;
        
        #region Lifecycle Methods

        private void Awake()
        {
            // Start at level 1.
            _currentLevel.Value = 0;
            _currentLevelData.Value = _levels[_currentLevel.Value];
            _currentDragonData.Value = _currentLevelData.Value.DragonData;
        }

        private void Update()
        {
            if (_isFightingBoss && _bossTimer.Value > 0f)
            {
                _bossTimer.Value -= Time.deltaTime;
                
                if (_bossTimer.Value <= 0f)
                {
                    HandleBossLevelLost();
                }
            }
        }

        #endregion

        #region Private Methods

        private void SetNextLevel()
        {
            if (_currentLevelData.Value == _bossLevel)
            {
                HandleBossLevelWon();
                return;
            }
            
            if (_currentLevel.Value < _levels.Count - 1)
            {
                Debug.Log("Going to predefined level.");
                _currentLevel.Value++;
                _currentLevelData.Value = _levels[_currentLevel.Value];
                _currentDragonData.Value = _currentLevelData.Value.DragonData;
                EnableBossButton();
                OnLevelStarted.Raise();
            }
            else
            {
                Debug.Log("Generating random level.");
                _currentLevel.Value++; 
                _currentLevelData.Value = GenerateRandomLevel();
                _currentDragonData.Value = _currentLevelData.Value.DragonData;
                
                OnLevelStarted.Raise();
            }
        }
        
        /// <summary>
        /// Generates a random level.
        /// </summary>
        /// <returns>Level generation information used by ScaleManager.</returns>
        private LevelData GenerateRandomLevel()
        {
            return _infiniteLevels[Random.Range(0, _infiniteLevels.Count)];
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Transition to next level when the current level is completed.
        /// </summary>
        public void HandleLevelCompleted()
        {
            SetNextLevel();
        }

        public void EnableBossButton()
        {
            if(!_bossLevelDefeated)
                _bossLevelButton.gameObject.SetActive(true);
        }

        public void HandleBossLevelStart()
        {
            _bossTimer.Value = _bossTimerMax;
            
            _bossTimerGameObject.SetActive(true);
            _bossLevelButton.gameObject.SetActive(false);
            
            _currentDragonData.Value = _bossLevel.DragonData;
            _currentLevelData.Value = _bossLevel;
            OnLevelStarted.Raise();

            _isFightingBoss = true;
        }
        
        public void HandleBossLevelWon()
        {
            Debug.Log("You won!");
            _isFightingBoss = false;
            _bossLevelDefeated = true;
            _bossTimerGameObject.SetActive(false);
            _bossLevelButton.gameObject.SetActive(false);
        }
        
        public void HandleBossLevelLost()
        {
            Debug.Log("You lost!");
            
            _isFightingBoss = false;
            _bossTimerGameObject.SetActive(false);
            _bossLevelButton.gameObject.SetActive(true);
            
            // Generate random level.
            _currentLevelData.Value = GenerateRandomLevel();
            _currentDragonData.Value = _currentLevelData.Value.DragonData;
            OnLevelStarted.Raise();
        }
        
        #endregion
    }
}
