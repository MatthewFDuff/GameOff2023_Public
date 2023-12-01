using _Core._Scripts.Data.Variables;
using HeathenEngineering.Events;
using TMPro;
using UnityEngine;

namespace _Core._Scripts.UI
{
    public class LevelUI : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private LevelVariable _currentLevel;

        [Header("UI Elements")] 
        [SerializeField] private TextMeshProUGUI _levelText;

        #region Lifecycle Methods

        private void Awake()
        {
            if (_levelText == null) _levelText = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _currentLevel.AddListener(HandleLevelChanged);
        }

        private void OnDisable()
        {
            _currentLevel.RemoveListener(HandleLevelChanged);
        }

        #endregion

        #region Event Handlers

        private void HandleLevelChanged(EventData eventData)
        {
            UpdateUIData();
        }

        #endregion

        #region Main Methods

        private void UpdateUIData()
        {
            if (_currentLevel.Value == null) return;
        
            _levelText.text = _currentLevel.Value.Name;
        }

        #endregion
    }
}
