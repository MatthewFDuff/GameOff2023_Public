using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Core._Scripts.Data;
using _Core._Scripts.Data.Variables;
using _Core._Scripts.Utility;
using HeathenEngineering.Events;
using UnityEngine;

namespace _Core._Scripts.Systems
{
    /// <summary>
    /// Instantiates and manages the scales in the level.
    /// </summary>
    public class ScaleManager : MonoBehaviour
    {
        public static ScaleManager Instance;
        
        #region Inspector Variables
        
        /// <summary>
        /// The prefab used to instantiate the scale game objects.
        /// </summary>
        [Header("Prefab Settings")]
        [SerializeField] private GameObject _scalePrefab;
        
        /// <summary>
        /// The prefab used to instantiate the border scale game objects.
        /// </summary>
        [SerializeField] private GameObject _borderScalePrefab;
        
        /// <summary>
        /// The parent transform of the scale game objects, where they will be instantiated.
        /// </summary>
        [SerializeField] private Transform _scaleParentTransform;

        /// <summary>
        /// The parent transform of the border scale game objects, where they will be instantiated.
        /// </summary>
        [SerializeField] private Transform _borderScaleParentTransform;
        
        /// <summary>
        /// The event that is raised when all scales are cleaned.
        /// </summary>
        [Header("Events")]
        [SerializeField] private GameEvent _allScalesCleanedEvent;
        
        /// <summary>
        /// The data for the current level, used to generate specific scale types based on the distribution.
        /// </summary>
        [Header("Generation Settings")]
        [SerializeField] private LevelVariable _currentLevel;

        /// <summary>
        /// The generator calculates all the positions of the scales in the level based on generation settings.
        /// </summary>
        [SerializeField] private GridGenerator _gridGenerator;
        
        [Header("Scales")]
        [SerializeField] public ScaleData DefaultScale;
        [SerializeField] public ScaleData CleanScale;
        [SerializeField] public ScaleData DirtyScale;
        [SerializeField] public ScaleData CrystalScale;
        [SerializeField] public ScaleData FireScale;
        [SerializeField] public ScaleData AirScale;
        [SerializeField] public ScaleData EarthScale;
        [SerializeField] public ScaleData WaterScale;
        [SerializeField] public ScaleData NatureScale;
        
        #endregion

        #region Private Variables

        private List<ScaleType> _currentLevelScaleTypes;
        private readonly List<Scale> _scaleGameObjects = new();
        private readonly List<Scale> _borderScaleGameObjects = new();
        
        #endregion
        
        #region Lifecycle Methods

        private void Awake()
        {
            if (Instance == null) Instance = this;
            
            ValidateData();
            
            if(_gridGenerator == null) _gridGenerator = GetComponent<GridGenerator>();
        }

        private void Start()
        {
            if (_currentLevel.Value == null || _currentLevel.Value.ScaleTypeDistribution == null)
            {
                Debug.Log("Current level is null.");
                return;
            }

            _currentLevelScaleTypes = InitializeScaleTypeData();
            SetScaleData();
        }

        #endregion

        /// <summary>
        /// Initializes the scales in the level based on the scale type distribution.
        /// </summary>
        /// <returns>A list of the scale types in the level.</returns>
        private List<ScaleType> InitializeScaleTypeData()
        {
            _currentLevelScaleTypes = new List<ScaleType>();

            // Scale distributions are given in the format of ScaleType:Percentage,
            // e.g. Default:80, Clean:15, Dirty:5
            // This needs to be converted into a list of scale types, where each scale type
            // is added to the list x times, based on the percentage.
            foreach (LevelScaleData scaleTypeDistribution in _currentLevel.Value.ScaleTypeDistribution)
            {
                // Calculate the number of scales based on the total number of scales in the level.
                int numberOfScales = Mathf.CeilToInt(scaleTypeDistribution.Percentage / 100f * _gridGenerator.TotalScales);
                
                // Add that many scales to the list, to be randomized.
                for (int i = 0; i < numberOfScales; i++)
                {
                    _currentLevelScaleTypes.Add(scaleTypeDistribution.ScaleType);
                }
                
                // Debug.Log($"Added {numberOfScales.ToString()} {scaleTypeDistribution.ScaleType} scales.");
            }
            
            // List of scale types needs to be shuffled to ensure that the scales 
            // are randomly distributed throughout the level.
            Utilities.ShuffleList(_currentLevelScaleTypes);

            return _currentLevelScaleTypes;
        }
        
        private ScaleType GetNextScaleType()
        {
            if (_currentLevelScaleTypes.Count == 0)
            {
                Debug.Log("No more scale types left.");
                return ScaleType.Clean;
            }

            ScaleType nextScaleType = _currentLevelScaleTypes[0];
            _currentLevelScaleTypes.RemoveAt(0);
            return nextScaleType;
        } 
        
        private void SetScaleData()
        {
            foreach (Vector2 position in _gridGenerator.BorderScalePositions)
            {
                Scale scaleGameObject = Instantiate(_borderScalePrefab, position, Quaternion.identity, _borderScaleParentTransform).GetComponent<Scale>();
                scaleGameObject.SetPosition(new Vector2(position.x, position.y));
                scaleGameObject.SetScaleData(GetScaleDataByType(ScaleType.Default));
                _borderScaleGameObjects.Add(scaleGameObject);
            }
            
            foreach (Vector2 position in _gridGenerator.ScalePositions)
            {
                Scale scaleGameObject = Instantiate(_scalePrefab, position, Quaternion.identity, _scaleParentTransform).GetComponent<Scale>();
                scaleGameObject.SetPosition(new Vector2(position.x, position.y));
                scaleGameObject.SetScaleData(GetScaleDataByType(GetNextScaleType()));
                
                _scaleGameObjects.Add(scaleGameObject);
            }

            SetAllAdjacentScales();
        }

        /// <summary>
        /// Used when the level has already been generated. Updates the existing scales.
        /// </summary>
        private IEnumerator UpdateScaleData()
        {
            foreach (Vector2 position in _gridGenerator.ScalePositions)
            {
                Scale scaleGameObject = GetScaleByPosition(position);
                scaleGameObject.SetScaleData(GetScaleDataByType(GetNextScaleType()));
                yield return new WaitForSeconds(0.01f);
            }
        }

        private IEnumerator UpdateBorderScales()
        {
            foreach (Vector2 position in _gridGenerator.BorderScalePositions)
            {
                Scale scaleGameObject = GetBorderScaleByPosition(position);
                scaleGameObject.SetScaleData(GetScaleDataByType(ScaleType.Default));
                yield return new WaitForSeconds(0.001f);
            }
        }

        private Scale GetScaleByPosition(Vector2 position)
        {
            return _scaleGameObjects.FirstOrDefault(scale => scale.ScalePosition == new Vector2(position.x, position.y));
        }
        
        private Scale GetBorderScaleByPosition(Vector2 position)
        {
            return _borderScaleGameObjects.FirstOrDefault(scale => scale.ScalePosition == new Vector2(position.x, position.y));
        }

        private ValidationResult ValidateData()
        {
            ValidationResult validationResult = new("Scale Manager");

            // Ensure there is a parent transform set.
            if (_scaleParentTransform == null)
            {
                validationResult.ErrorMessages.Add("Parent transform is not set.");
            }

            // Ensure the scale prefab is set, otherwise scale gameobjects can't be instantiated.
            if (_scalePrefab == null)
            {
                validationResult.ErrorMessages.Add("Scale prefab is not set.");
            }

            // Check validation result.
            if (!validationResult.IsValid)
            {
                Debug.LogError(validationResult.CombinedMessages);
            }

            return validationResult;
        }

        internal ScaleData GetScaleDataByType(ScaleType scaleType)
        {
            return scaleType switch
            {
                ScaleType.Default => DefaultScale,
                ScaleType.Clean => CleanScale,
                ScaleType.Dirty => DirtyScale,
                ScaleType.Fire => FireScale,
                ScaleType.Earth => EarthScale,
                ScaleType.Water => WaterScale,
                ScaleType.Air => AirScale,
                ScaleType.Crystal => CrystalScale,
                ScaleType.Nature => NatureScale,
                _ => DefaultScale
            };
        }
        
        public void SetAllAdjacentScales()
        {
            foreach (Scale scaleObjectData in _scaleGameObjects)
            {
                scaleObjectData.AdjacentScales = GetAdjacentScales(scaleObjectData.ScalePosition);
            }
        }

        /// <summary>
        /// Get a list of the scales ordered by distance from the given position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>A list of scales ordered by distance.</returns>
        public List<Scale> ScaleDistances(Vector2 position)
        {
            return _scaleGameObjects
                .Where(scale => scale.ScaleData.ScaleType != ScaleType.Clean && scale.ScaleData.ScaleType != ScaleType.Default)
                .OrderBy(scale => Vector2.Distance(position, scale.ScalePosition)).ToList();
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
                directions.Add(new Vector2(position.x + 0.5f, position.y + 0.5f));
                
                // Middle Right
                // directions.Add(new Vector2(position.x + 2, position.y));
                
                // Bottom Right
                directions.Add(new Vector2(position.x + 0.5f, position.y - 0.5f));
                
                // Bottom Left
                directions.Add(new Vector2(position.x - 0.5f, position.y - 0.5f));
                
                // Middle Left
                // directions.Add(new Vector2(position.x - 2, position.y));
                
                // Upper Left
                directions.Add(new Vector2(position.x - 0.5f, position.y + 0.5f));
            }
            else
            {
                // Upper Right
                directions.Add(new Vector2(position.x + 0.5f, position.y + 0.5f));
                
                // Middle Right
                // directions.Add(new Vector2(position.x + 2, position.y));
                
                // Bottom Right
                directions.Add(new Vector2(position.x + 0.5f, position.y - 0.5f));
                
                // Bottom Left
                directions.Add(new Vector2(position.x - 0.5f, position.y - 0.5f));
                
                // Middle Left
                // directions.Add(new Vector2(position.x - 2, position.y));
                
                // Upper Left
                directions.Add(new Vector2(position.x - 0.5f, position.y + 0.5f));
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
            foreach (Scale scaleObjectData in _scaleGameObjects)
            {
                if (scaleObjectData.ScalePosition == position)
                {
                    return scaleObjectData;
                }
            }

            return null;
        }
        
        private static bool IsOddColumn(Vector2 position)
        {
            return position.x % 1 != 0;
        }
        private bool IsWithinGrid(Vector2 position)
        {
            return position.x >= 0 && position.x < _gridGenerator.Width && position.y >= 0 && position.y < _gridGenerator.Height;
        }

        #region Event Handlers

        /// <summary>
        /// Called when a scale is cleaned.
        ///
        /// Note: An alternative way to do this could be to tally up to the total number of scales in the level.
        /// </summary>
        public void HandleScaleCleaned()
        {
            // Check if all scales are clean.
            if (_scaleGameObjects.Any(scale => scale.ScaleData.ScaleType != ScaleType.Clean))
            {
                return;
            }

            // All scales are clean.
            _allScalesCleanedEvent.Raise();
        }

        /// <summary>
        /// 
        /// </summary>
        public void HandleLevelStarted()
        {
            _currentLevelScaleTypes = InitializeScaleTypeData();
            StartCoroutine(UpdateScaleData());
            StartCoroutine(UpdateBorderScales());
        }

        #endregion
         
    }
}