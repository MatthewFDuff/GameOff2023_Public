using _Core._Scripts.Data;
using HeathenEngineering;
using HeathenEngineering.Events;
using UnityEngine;
using UnityEngine.UI;

namespace _Core._Scripts.UI
{
    /// <summary>
    /// A UI element that manages the display of a tool and its upgrades. 
    /// </summary>
    public class ToolUI : MonoBehaviour
    {
        [Header("Data")]
        [Tooltip("The data of the tool that this UI element displays.")]
        [SerializeField] private ToolData _toolData;

        [Header("UI Elements")]
        [Tooltip("The canvas group that manages the visibility of the tool UI.")]
        [SerializeField] private CanvasGroup _canvasGroup;
        
        [Tooltip("The image where the tool sprite is displayed and updated.")]
        [SerializeField] private Image _toolImage;

        [Tooltip("The canvas group of the UI that contains the upgrades.")]
        [SerializeField] private CanvasGroup _upgradePopupCanvasGroup;

        [Tooltip("The button that opens the upgrade popup.")]
        [SerializeField] private Button _openUpgradePopupButton;

        [Tooltip("The resource variable that triggers this tool to be unlocked.")] 
        [SerializeField] private IntVariable _unlockingResource;
        
        [Tooltip("The amount of the resource that the player needs to unlock the tool upgrades.")]
        [SerializeField] private int _unlockingResourceCost;

        [Tooltip("When true, this shop will be enabled at the start of the game.")]
        [SerializeField] private bool _isEnabledAtStart;
        
        #region Lifecycle Methods

        private void OnEnable()
        {
            _openUpgradePopupButton.onClick.AddListener(HandleUpgradeButtonClicked);
            _unlockingResource.AddListener(HandleUnlockingResourceChanged);
        }
        
        private void OnDisable()
        {
            _openUpgradePopupButton.onClick.RemoveListener(HandleUpgradeButtonClicked);
            _unlockingResource.RemoveListener(HandleUnlockingResourceChanged);
        }
        
        private void Start()
        {
            UpdatePopupVisibility();
            UpdateToolVisibility(_isEnabledAtStart);
        }

        #endregion

        #region Event Handlers
        
        private void HandleUnlockingResourceChanged(EventData eventData)
        {
            if (_unlockingResource.Value < _unlockingResourceCost) return;

            UpdateToolVisibility(true);
        }
        
        private void HandleUpgradeButtonClicked()
        {
            UpdatePopupVisibility(true);
        }

        #endregion

        #region Main Methods

        /// <summary>
        /// Used to show the tool ui.
        /// </summary>
        private void UpdateToolVisibility(bool isVisible = false)
        {
            if(_canvasGroup == null) return;
            
            _canvasGroup.alpha = isVisible ? 1 : 0;
            _canvasGroup.interactable = isVisible;
            _canvasGroup.blocksRaycasts = isVisible;
        }
        
        /// <summary>
        /// Used to show the upgrade popup.
        /// </summary>
        private void UpdatePopupVisibility(bool isVisible = false)
        {
            if(_upgradePopupCanvasGroup == null) return;
            
            _upgradePopupCanvasGroup.alpha = isVisible ? 1 : 0;
            _upgradePopupCanvasGroup.interactable = isVisible;
            _upgradePopupCanvasGroup.blocksRaycasts = isVisible;
        }

        #endregion
    }
}
