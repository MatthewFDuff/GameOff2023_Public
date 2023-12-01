using System.Collections.Generic;
using _Core._Scripts.Data;
using _Core._Scripts.Systems;
using UnityEngine;

namespace _Core._Scripts.UI
{
    /// <summary>
    /// This script is used to create scales and make them pan across the screen.
    /// </summary>
    public class ScaleBackground : MonoBehaviour
    {
        [SerializeField] private GridGenerator _gridGenerator;
        
        [SerializeField] private GameObject _scalePrefab;
        
        [SerializeField] private List<ScaleData> _scaleTypes;
        
        [SerializeField] private Transform _scaleParentTransform;
        
        [SerializeField] private List<Color> _colors;
        
        [SerializeField] private int _chainReactions = 2; 
        
        private List<GameObject> _scales;
        
        

        private void Start()
        {
            _scales = new List<GameObject>();
            GenerateScales();
        }

        private void GenerateScales()
        {
            foreach (var position in _gridGenerator.ScalePositions)
            {
                var scaleGameObject = Instantiate(_scalePrefab, _scaleParentTransform);
                scaleGameObject.transform.position = position;
                var scale = scaleGameObject.GetComponent<Scale>();
                scale.SetScaleData(_scaleTypes[UnityEngine.Random.Range(0, _scaleTypes.Count)]);
                scale.SetPosition(position);
                // _spriteRenderer.color = _colors[UnityEngine.Random.Range(0, _colors.Count)];
                _scales.Add(scaleGameObject);
            }
            
            SetAllAdjacentScales();
        }
        
        
        public void SetAllAdjacentScales()
        {
            foreach (GameObject scaleObjectData in _scales)
            {
                var scale = scaleObjectData.GetComponent<Scale>();
                scale.AdjacentScales = GetAdjacentScales(scale.ScalePosition);
            }
            
            // Pick a random scale to start a chain reaction.
            for (int i = 0; i < _chainReactions; i++)
            {
                var randomScale = _scales[Random.Range(0, _scales.Count)].GetComponent<Scale>();
                randomScale.TriggerChain();
            }
        }

        private void ShakeRandomScale()
        {
            var randomScale = _scales[UnityEngine.Random.Range(0, _scales.Count)].GetComponent<Scale>();
            randomScale.TriggerChain();
        }
        
        
        /// <summary>
        /// Takes an x and y coordinate and returns a list of adjacent scales.
        /// </summary>
        /// <param name="position"></param>
        private List<Scale> GetAdjacentScales(Vector2 position)
        {
            List<Vector2> adjacentPositions = new();
            List<Vector2> directions = new();
            // Calculate adjacent positions based on column.
            if (IsOddColumn(position))
            {
                // Upper Right
                directions.Add(new Vector2(position.x + 1, position.y + 1));
                
                // Middle Right
                directions.Add(new Vector2(position.x + 2, position.y));
                
                // Bottom Right
                directions.Add(new Vector2(position.x + 1, position.y));
                
                // Bottom Left
                directions.Add(new Vector2(position.x - 1, position.y));
                
                // Middle Left
                directions.Add(new Vector2(position.x - 2, position.y));
                
                // Upper Left
                directions.Add(new Vector2(position.x - 1, position.y + 1));
            }
            else
            {
                // Upper Right
                directions.Add(new Vector2(position.x + 1, position.y));
                
                // Middle Right
                directions.Add(new Vector2(position.x + 2, position.y));
                
                // Bottom Right
                directions.Add(new Vector2(position.x + 1, position.y - 1));
                
                // Bottom Left
                directions.Add(new Vector2(position.x - 1, position.y - 1));
                
                // Middle Left
                directions.Add(new Vector2(position.x - 2, position.y));
                
                // Upper Left
                directions.Add(new Vector2(position.x - 1, position.y));
            }
            
            // Loop through all directions
            foreach (Vector2 direction in directions)
            {
                // Check if the adjacent position is within the grid
                if (IsWithinGrid(direction))
                {
                    adjacentPositions.Add(direction);
                }
            }
            
            return adjacentPositions.ConvertAll(GetScaleAt);
        }
        
        private Scale GetScaleAt(Vector2 position)
        {
            foreach (GameObject scaleObjectData in _scales)
            {
                var scale = scaleObjectData.GetComponent<Scale>();
                if (scale.ScalePosition == position)
                {
                    return scale;
                }
            }

            return null;
        }
        private static bool IsOddColumn(Vector2 position)
        {
            return position.x % 2 != 0;
        }
        private bool IsWithinGrid(Vector2 position)
        {
            return position.x >= 0 && position.x < _gridGenerator.Width && position.y >= 0 && position.y < _gridGenerator.Height;
        }
    }
}
