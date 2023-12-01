using _Core._Scripts.Systems;
using TMPro;
using UnityEngine;

namespace _Core._Scripts.UI
{
    public class MoneyDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyLabel;

        private void Update()
        {
            _moneyLabel.text = $"SCALES: {GameManager.Instance.CurrentMoney.ToString()} ";
        }
    }
}
