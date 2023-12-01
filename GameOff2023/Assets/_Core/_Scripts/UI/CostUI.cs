using _Core._Scripts.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Core._Scripts.UI
{
    public class CostUI : MonoBehaviour
    {
        [SerializeField] private Image _costImage;
        [SerializeField] private TextMeshProUGUI _costLabel;

        #region Lifecycle Methods

        private void Awake()
        {
            if(_costImage == null) _costImage = GetComponentInChildren<Image>();
            if(_costLabel == null) _costLabel = GetComponentInChildren<TextMeshProUGUI>();
        }

        #endregion

        #region Main Methods

        public void SetCost(ResourceCostData resourceCostData)
        {
            _costImage.sprite = resourceCostData.Currency.Sprite;
            _costLabel.text = resourceCostData.Amount.ToString();
        }

        #endregion
    }
}
