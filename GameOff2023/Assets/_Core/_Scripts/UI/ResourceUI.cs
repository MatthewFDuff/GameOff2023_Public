using _Core._Scripts.Data;
using HeathenEngineering.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Core._Scripts.UI
{
    public class ResourceUI : MonoBehaviour
    {
        /// <summary>
        /// The resource data that is displayed in the UI.
        /// </summary>
        [Header("UI Data")]
        [SerializeField] private CurrencyData _currencyData;

        /// <summary>
        /// The label that displays the resource amount.
        /// </summary>
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI _resourceAmountText;
        /// <summary>
        /// The image that displays the resource sprite.
        /// </summary>
        [SerializeField] private Image _resourceImage;
        
        [Tooltip("The canvas group that determines the visibility of the resource UI.")]
        [SerializeField] private CanvasGroup _canvasGroup;

        #region Lifecycle Methods

        private void Awake()
        {
            if(_resourceAmountText == null) _resourceAmountText = GetComponentInChildren<TextMeshProUGUI>();
            if(_resourceImage == null) _resourceImage = GetComponentInChildren<Image>();
            if(_canvasGroup == null) _canvasGroup = GetComponentInChildren<CanvasGroup>();
        }

        private void Start()
        {
            _canvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            _currencyData.CurrencyAmountVariable.AddListener(HandleResourceAmountChanged);
        }

        private void OnDisable()
        {
            _currencyData.CurrencyAmountVariable.RemoveListener(HandleResourceAmountChanged);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the event when the resource amount changes.
        /// </summary>
        /// <param name="eventData"></param>
        private void HandleResourceAmountChanged(EventData eventData)
        {
            UpdateUIData();
        }

        #endregion

        /// <summary>
        /// Updates the resource UI with the current resource data.
        /// </summary>
        private void UpdateUIData()
        {
            if (!_currencyData.IsUnlocked) return;
            
            _resourceAmountText.text = _currencyData.CurrentAmount.ToString();
            _resourceImage.sprite = _currencyData.Sprite;
            _canvasGroup.alpha = 1;
        }
    }
}
