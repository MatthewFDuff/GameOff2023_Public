using HeathenEngineering;
using UnityEngine;

namespace _Core._Scripts.Data
{
    [CreateAssetMenu(fileName = "New Currency", menuName = "Custom/Currency")]
    public class CurrencyData : ScriptableObject
    {

        /// <summary>
        /// The name of the currency. 
        /// </summary>
        public string Name => _name;
    
        /// <summary>
        /// The description of the currency.
        /// </summary>
        public string Description => _description;
    
        /// <summary>
        /// The sprite of the currency that is displayed in the UI.
        /// </summary>
        public Sprite Sprite => _sprite;
        
        /// <summary>
        /// True if the currency is unlocked and can be used by the player.
        /// </summary>
        public bool IsUnlocked => _isUnlocked;
        
        /// <summary>
        /// The amount of the currency that the player currently has.
        /// </summary>
        public int CurrentAmount => _amount.Value;
        
        /// <summary>
        /// A reference to the variable that stores the amount of the currency.
        /// Used to subscribe to changes in the amount.
        /// </summary>
        public IntVariable CurrencyAmountVariable => _amount;
    
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private IntVariable _amount;
        [SerializeField] private int _maxAmount = 1000000;
        [SerializeField] private bool _isUnlocked;

        #region Main Methods

        public void AddResource(int amountToAdd)
        {
            if (CurrentAmount + amountToAdd > _maxAmount)
            {
                _amount.Value = _maxAmount;
                return;
            }
            
            _amount.SetValue(CurrentAmount + amountToAdd);
            _isUnlocked = true;
        }

        /// <summary>
        /// Resources are spent when the user buys something in the shop.
        /// </summary>
        /// <param name="amountToSpend">The amount of resources being spent.</param>
        /// <returns>True if the player was able to spend the resources.</returns>
        public bool SpendResource(int amountToSpend)
        {
            if (CurrentAmount < amountToSpend) return false;
            
            _amount.SetValue(CurrentAmount - amountToSpend);
            
            return true;
        }

        #endregion
        
        #region Lifecycle Methods

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_name))
            {
                _name = name;
            }
        }

        #endregion
    }
}
