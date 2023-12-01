using System.Collections.Generic;
using _Core._Scripts.Data;
using _Core._Scripts.UI;
using HeathenEngineering;
using HeathenEngineering.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Core._Scripts
{
    public class UpgradeManager : MonoBehaviour
    {
        public UpgradeData upgrade;
        public int maxLevel => upgrade.Costs.Count;
        public int itemLevel = 0; // Set the initial level of the item
        

        [Header("CostUI")]
        [Tooltip("The prefab used to display each cost for an upgrade.")]
        [SerializeField] private GameObject _costPrefab;
        [Tooltip("The parent transform for the cost UI elements.")]
        [SerializeField] private Transform _costParentTransform;
        [Tooltip("The button used to upgrade the item.")]
        [SerializeField] private Button _upgradeButton;
        [Tooltip("The text used in the upgrade button.")] 
        [SerializeField] private TextMeshProUGUI _upgradeButtonLabel;
        [Tooltip("The list of UI elements used to display the cost of the upgrade.")]
        [SerializeField] private List<GameObject> _costUIElements = new();
    
        //public Text coinsText; // Reference to a UI text element to display the current coins

        [FormerlySerializedAs("textField")] [SerializeField] TextMeshProUGUI descriptionLabel;
        [SerializeField] Image spriteImageTechnally;

        [SerializeField] List<IntVariable> _resourceVariables = new();
    
        private bool _canAffordUpgrade = false;
    
        private UpgradeLevelData CurrentUpgrade => upgrade.Costs[itemLevel];
    
        #region Lifecycle Methods

        private void Start()
        {
            UpdateDescription();
            UpdateSprite();
            UpdateCostButton();
        }

        private void OnEnable()
        {
            foreach (var resource in _resourceVariables)
            {
                resource.AddListener(HandleResourceValueChanged);
            }
        }
    
        private void OnDisable()
        {
            foreach (var resource in _resourceVariables)
            {
                resource.RemoveListener(HandleResourceValueChanged);
            }
        }

        #endregion


        #region Event Handlers

        /// <summary>
        /// When a resource that is required for ths upgrade changes, check if the player can now afford it.
        /// </summary>
        /// <param name="eventData"></param>
        public void HandleResourceValueChanged(EventData<int> eventData)
        {
            Debug.Log($"{name} HandleResourceValueChanged");
            _canAffordUpgrade = CheckIfCanAffordUpgrade();
            _upgradeButton.interactable =  itemLevel != maxLevel && _canAffordUpgrade;
        }

        #endregion

    
        // Call this function when the player clicks the upgrade button
        public void UpgradeItem()
        {
            // Check if the player has enough coins to afford the upgrade
            if (itemLevel == maxLevel) return;

            // Player cannot afford the upgrade.
            if (!_canAffordUpgrade) return;
        
            // Deduct the upgrade cost from the player's coins
            upgrade.PurchaseUpgrade();

            // Perform the upgrade
            itemLevel++;
            ApplyUpgradeValue();
        
        
            if (itemLevel == maxLevel)
            {
                descriptionLabel.text = "MAX LEVEL";
            }
            else
            {
                UpdateDescription();
                UpdateSprite();
                UpdateCostButton();
            }

        }
    
        private void UpdateCostButton()
        {
            _upgradeButtonLabel.text = itemLevel == maxLevel ? "MAX" : "COST";
        
            _costUIElements.Clear();
        
            // Remove the existing cost UI elements from the parent.
            foreach (Transform child in _costParentTransform)
            {
                Destroy(child.gameObject);
            }
        
            // Add the new cost UI elements to the parent.
            foreach (var cost in CurrentUpgrade.ResourceCost)
            {
                GameObject costUI = Instantiate(_costPrefab, _costParentTransform);
                costUI.GetComponent<CostUI>().SetCost(cost);
                _costUIElements.Add(costUI);
            }
        }
        private bool CheckIfCanAffordUpgrade()
        {
            bool canAfford = true;
        
            foreach (ResourceCostData cost in CurrentUpgrade.ResourceCost)
            {
                if (cost.Currency.CurrencyAmountVariable.Value < cost.Amount)
                {
                    canAfford = false;
                    break;
                }
            }
        
            return canAfford;
        }

        /// <summary>
        /// Changes the appropriate value based on the upgrade value and type.
        /// </summary>
        private void ApplyUpgradeValue()
        {
            float value = (float) upgrade.UpgradeVariable.ObjectValue;
            switch (upgrade.CalculationType)
            { 
            
                case UpgradeCalculationType.Subtract:
                    value -= CurrentUpgrade.Value;
                    upgrade.UpgradeVariable.ObjectValue = value;
                    break;
                case UpgradeCalculationType.Multiply:
                
                    value *= CurrentUpgrade.Value;
                    upgrade.UpgradeVariable.ObjectValue = value;
                    break;
                case UpgradeCalculationType.Divide:
                
                    value /= CurrentUpgrade.Value;
                    upgrade.UpgradeVariable.ObjectValue = value;
                    break;
                case UpgradeCalculationType.Add:
                default:
                    value += CurrentUpgrade.Value;
                    upgrade.UpgradeVariable.ObjectValue = value;
                    break;
            }   
        }
    
        private void UpdateDescription()
        {
            descriptionLabel.text = CurrentUpgrade.Description;
        }

        private void UpdateSprite()
        {
            spriteImageTechnally.sprite = CurrentUpgrade.Sprite;
        }
    }
}

