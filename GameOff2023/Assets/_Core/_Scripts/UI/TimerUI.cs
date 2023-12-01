using System;
using HeathenEngineering;
using TMPro;
using UnityEngine;

namespace _Core._Scripts.UI
{
    public class TimerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerLabel;
        [SerializeField] private FloatReference _timerValue;
        
        private void Start()
        {
            if (_timerLabel == null) _timerLabel = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timerValue.Value);
            string timerText = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
            _timerLabel.text = timerText;
        }
    }
}
