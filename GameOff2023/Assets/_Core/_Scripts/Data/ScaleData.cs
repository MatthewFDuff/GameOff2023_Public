using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Core._Scripts.Data
{
    [CreateAssetMenu(fileName = "New Scale", menuName = "Custom/Scale")]
    public class ScaleData : ScriptableObject
    {
        /// <summary>
        /// The name of the scale.
        /// </summary>
        public string Name => _name;
        
        /// <summary>
        /// A useful description of the scale that might give the player hints on how to interact with it.
        /// </summary>
        public string Description => _description;
        
        /// <summary>
        /// The sprite of the scale, how it appears in-game.
        /// </summary>
        public Sprite Sprite => _sprite;
        
        /// <summary>
        /// The type of scale that this is.
        /// </summary>
        public ScaleType ScaleType => _scaleType;
        
        /// <summary>
        /// Once a scale is interacted with using the required tool, it will transform into this scale type.
        /// </summary>
        public ScaleType TransformedScaleType => _transformedScaleType;
        
        /// <summary>
        /// The tool that is required to interact with this scale and cause it to change.
        /// </summary>
        public Tools RequiredTool => _requiredTool;
        
        /// <summary>
        /// The base value of health that the scale has. Will be scaled based on the current level.
        /// </summary>
        public int BaseHealth => _baseHealth;

        public int BaseValue => _baseValue;
        
        public List<ScaleDropData> ResourceDropData => _resourceDropData;
        
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private Sprite _sprite;
        [SerializeField] private ScaleType _scaleType = ScaleType.Default;
        [SerializeField] private ScaleType _transformedScaleType = ScaleType.Clean;
        [SerializeField] private Tools _requiredTool = Tools.Pickaxe;
        [SerializeField] private int _baseHealth;
        [SerializeField] private int _baseValue;

        [SerializeField] private List<ScaleDropData> _resourceDropData;

        public GameObject objectToInstantiate;


        #region Lifecycle Methods

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_name))
            {
                _name = name;
            }
        }

        #endregion

        /// <summary>
        /// Use this method to mine the scale and get the resources that it drops.
        /// </summary>
        public void DropResources(int combo, float comboMultiplier = 1f, Vector3 position = default)
        {
            float offsetX = 0f;
            float offsetY = 0f;
            float offsetZ = 0f;

            Vector3 offsetVector = new Vector3(offsetX, offsetY, offsetZ);

            foreach (ScaleDropData potentialDrop in _resourceDropData)
            {
                int amountToDrop = GetRandomAmountOfResourceDrops(potentialDrop);
                int comboBonus = (int) (Math.Max(combo - 3, 0) * comboMultiplier);
                var totalAmountToDrop = amountToDrop + comboBonus;
                
                // Debug.Log($"Base Amount to Drop: {amountToDrop.ToString()}, " +
                          // $"Combo Bonus: {comboBonus.ToString()}, " +
                          // $"Total: {totalAmountToDrop.ToString()}");
                potentialDrop.CurrencyData.AddResource(totalAmountToDrop);

                position += offsetVector;

                var uiText = Instantiate(objectToInstantiate, position, Quaternion.identity);
                uiText.GetComponent<CurrencyGainText>().displayInfo(potentialDrop.CurrencyData.Sprite, totalAmountToDrop.ToString());

                offsetVector.x += 0.3f;
                offsetVector.y += 0.3f;

            }
        }

        public void DropResources(Vector3 position = default)
        {
            float offsetX = 0f;
            float offsetY = 0f;
            float offsetZ = 0f;

            Vector3 offsetVector = new Vector3(offsetX, offsetY, offsetZ);

            foreach (ScaleDropData potentialDrop in _resourceDropData)
            {
                int amountToDrop = GetRandomAmountOfResourceDrops(potentialDrop);
                
                // Debug.Log($"Base Amount to Drop: {amountToDrop.ToString()}, " +
                // $"Combo Bonus: {comboBonus.ToString()}, " +
                // $"Total: {totalAmountToDrop.ToString()}");
                potentialDrop.CurrencyData.AddResource(amountToDrop);

                position += offsetVector;

                var uiText = Instantiate(objectToInstantiate, position, Quaternion.identity);
                uiText.GetComponent<CurrencyGainText>().displayInfo(potentialDrop.CurrencyData.Sprite, amountToDrop.ToString());

                offsetVector.x += 0.3f;
                offsetVector.y += 0.3f;

            }
        }
        
        private static int GetRandomAmountOfResourceDrops(ScaleDropData scaleDropData)
        {
            if (scaleDropData.CurrencyData == null) return 0;
            
            if(Random.Range(0, 100) <= scaleDropData.DropChance)
            {
                return Random.Range(scaleDropData.MinAmount, scaleDropData.MaxAmount);
            }
            
            return 0;
        }
        
        [Serializable]
        public struct ScaleDropData
        {
            public CurrencyData CurrencyData;
            [Range(0,100)]
            public int DropChance;  
            public int MinAmount;
            public int MaxAmount;
        } 
    }
}
