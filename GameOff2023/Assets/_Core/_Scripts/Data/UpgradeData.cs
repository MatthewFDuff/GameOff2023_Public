using System;
using System.Collections.Generic;
using HeathenEngineering;
using UnityEngine;

namespace _Core._Scripts.Data
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "Custom/Upgrade")]
    public class UpgradeData : ScriptableObject
    {
        public string Name => _name; // Damage++
        
        public string Description => _description; // Increases the damage of the tool.
        
        public Sprite Sprite => _sprite; // The sprite of the tool that is displayed in the UI.
        public List<UpgradeLevelData> Costs => _costs;
        
        public float DefaultValue => _defaultValue;
        public DataVariable UpgradeVariable => _upgradeVariable;
        public UpgradeCalculationType CalculationType => _calculationType;
        
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private List<UpgradeLevelData> _costs;
        [SerializeField] private float _defaultValue;
        [SerializeField] private DataVariable _upgradeVariable;
        [SerializeField] private UpgradeCalculationType _calculationType;

        public void Initialize()
        {
            _upgradeVariable.ObjectValue = _defaultValue;
        }
        
        /// <summary>
        /// Reduces resources based on the cost of the upgrade.
        /// </summary>
        public void PurchaseUpgrade()
        {
            foreach (UpgradeLevelData upgradeLevel in _costs)
            {
                foreach (var cost in upgradeLevel.ResourceCost)
                {
                    if (cost.Amount <= cost.Currency.CurrencyAmountVariable.Value)
                    {
                        cost.Currency.CurrencyAmountVariable.Value -= cost.Amount;
                    }
                }
            }
        }
    }
 
    public enum UpgradeCalculationType
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }
    
    [Serializable] 
    public struct UpgradeLevelData
    {
        public List<ResourceCostData> ResourceCost;
        public string Description;
        public Sprite Sprite;
        public float Value;
    }

    [Serializable]
    public struct ResourceCostData
    {
        [Tooltip("The amount of resources required to purchase this upgrade.")]
        public int Amount;
        [Tooltip("Reference to the resource variable that is required to purchase this upgrade.")]
        public CurrencyData Currency;
    }
}
