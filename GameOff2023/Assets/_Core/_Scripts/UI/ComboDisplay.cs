using System;
using HeathenEngineering;
using HeathenEngineering.Events;
using TMPro;
using UnityEngine;

namespace _Core._Scripts.UI
{
    public class ComboDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currentComboLabel;
        [SerializeField] private TextMeshProUGUI _maxComboLabel;
        

        [SerializeField] private IntVariable _currentComboVariable;
        [SerializeField] private FloatVariable _maxComboVariable;
        [SerializeField] private FloatVariable _currentComboMultiplier;

        private void OnEnable()
        {
            _currentComboVariable.AddListener(HandleComboUpdate);
            _maxComboVariable.AddListener(HandleMaxComboUpdate);
        }

        private void OnDisable()
        {
            _currentComboVariable.RemoveListener(HandleComboUpdate);
            _maxComboVariable.RemoveListener(HandleMaxComboUpdate);
        }

        private void Start()
        {
            float bonusScales = Math.Max(_currentComboVariable.Value - 3, 0) * _currentComboMultiplier.Value;
            _currentComboLabel.text = $"Combo: {_currentComboVariable.Value.ToString()} (+{bonusScales.ToString()})";
            _maxComboLabel.text = $"Max Combo: {_maxComboVariable.Value}";
        }

        private void HandleComboUpdate(EventData e)
        {
            float bonusScales = Math.Max(_currentComboVariable.Value - 3, 0) * _currentComboMultiplier.Value;
            _currentComboLabel.text = $"Combo: {_currentComboVariable.Value.ToString()} (+{bonusScales.ToString()})";
        }

        private void HandleMaxComboUpdate(EventData e)
        {
            _maxComboLabel.text = $"Max Combo: {_maxComboVariable.Value}";
        }
    }
}
