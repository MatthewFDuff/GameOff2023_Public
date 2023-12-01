using System.Collections.Generic;
using HeathenEngineering;
using UnityEngine;

namespace _Core._Scripts.Systems
{
    /// <summary>
    /// Spawns the scales for the level.
    /// </summary>
    [ExecuteInEditMode]
    public class GridGenerator : MonoBehaviour
    {
        public static GridGenerator Instance;
        
        /// <summary>
        /// The position of the starting point for the scales that are generated.
        /// e.g. (0, 0) would be the bottom left corner of the grid of scales.
        /// Scales outside of this range would be border scales.
        /// </summary>
        [Header("Generation Settings")]
        [SerializeField] private Vector2Int _origin;
        
        /// <summary>
        /// The number of scales that make up the width of the play area.
        /// </summary>
        [SerializeField] private int _width;
        
        /// <summary>
        /// The number of scales that make up the height of the play area.
        /// </summary>
        [SerializeField] private int _height;

        /// <summary>
        /// The number of scales that make up the width of the border area.
        /// </summary>
        [SerializeField] private int _borderWidth;
        
        /// <summary>
        /// The number of scales that make up the height of the border area.
        /// </summary>
        [SerializeField] private int _borderHeight;

        /// <summary>
        /// The horizontal offset of the scales.
        /// </summary>
        [SerializeField] private float _horizontalOffset = 0.5f;
        
        /// <summary>
        /// The vertical offset of the scales.
        /// </summary>
        [SerializeField] private float _verticalOffset = 0.5f;

        [Header("Variables")] 
        [SerializeField] private IntVariable _widthVariable;
        [SerializeField] private IntVariable _heightVariable;
        [SerializeField] private IntVariable _totalScalesVariable;
        [SerializeField] private Vector2Variable _centerPointVariable;
        
        [Header("Gizmos")]
        [SerializeField] private bool _showGizmos;
        [Range(0, 1)]
        [SerializeField] private float _scaleGizmoSize = 0.1f;
        [SerializeField] private Color _scaleGizmoColor = Color.red;
        [Range(0, 1)]
        [SerializeField] private float _borderScaleGizmoSize = 0.1f;
        [SerializeField] private Color _borderScaleGizmoColor = Color.blue;
        [Range(0, 1)]
        [SerializeField] private float _centerPointGizmoSize = 0.5f;
        [SerializeField] private Color _centerPointGizmoColor = Color.white;

        #region Properties

        /// <summary>
        /// A list of the positions of the scales that are inside the play area.
        /// </summary>
        public List<Vector2> ScalePositions { get; } = new();

        /// <summary>
        /// A list of the positions of the scales that are outside of the play area.
        /// </summary>
        public List<Vector2> BorderScalePositions { get; } = new();
        
        /// <summary>
        /// The number of scales that make up the width of the play area.
        /// </summary>
        public int Width => _width;
        
        /// <summary>
        /// The number of scales that make up the height of the play area.
        /// </summary>
        public int Height => _height;
        
        /// <summary>
        /// The total number of scales that make up the play area.
        /// </summary>
        public int TotalScales => _width * _height;

        private Vector2 CenterPoint => new Vector2((_origin.x + _width  * _horizontalOffset / 2f) + _horizontalOffset, (_origin.y + _height * _verticalOffset  / 2f) - 0.5f/2);
        
        #endregion

        #region Lifecycle Methods

        private void Awake()
        {
            Instance = this;
        }
        
        private void Start()
        {
            GenerateGridPoints();
        }
        
        private void OnValidate()
        {
            GenerateGridPoints();
        }
        
        private void OnDrawGizmos()
        {
            if (!_showGizmos) return;
            
            if (ScalePositions != null && ScalePositions.Count > 0)
            {
                Gizmos.color = _scaleGizmoColor;
                
                // Draw the grid points for the play area.
                foreach (Vector2 position in ScalePositions)
                {
                    Gizmos.DrawSphere(position, _scaleGizmoSize);
                }
            }

            if (BorderScalePositions != null && BorderScalePositions.Count > 0)
            {
                Gizmos.color = _borderScaleGizmoColor;
                
                // Draw the grid points for the border area.
                foreach (Vector2 position in BorderScalePositions)
                {
                    Gizmos.DrawSphere(position, _borderScaleGizmoSize);
                }
            }

            // Draw the center point.
            Gizmos.color = _centerPointGizmoColor;
            Gizmos.DrawSphere(CenterPoint, _centerPointGizmoSize);
        }

        #endregion
        
        /// <summary>
        /// Get the nearest grid point to the given position.
        /// </summary>
        /// <param name="position">The position to calculate from.</param>
        /// <returns>The nearest grid point.</returns>
        public Vector2 GetNearestGridPoint(Vector2 position)
        {
            Vector2 nearestPoint = Vector2.zero;
            float nearestDistance = float.MaxValue;
            
            foreach (Vector2 gridPoint in ScalePositions)
            {
                float distance = Vector2.Distance(position, gridPoint);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestPoint = gridPoint;
                }
            }

            return nearestPoint;
        }
        
        /// <summary>
        /// Generate the grid positions for the scales, in a hexagonal pattern with a y-offset of 0.5 when the column is odd.
        /// </summary>
        private void GenerateGridPoints()
        {
            ScalePositions.Clear();
            BorderScalePositions.Clear();
            
            if (_width <= 0 || _height <= 0) return;
            
            for (int y = _origin.y - _borderHeight; y < _origin.y + _height + _borderHeight; y++)
            {
                for (int x = _origin.x - _borderWidth; x < _origin.x + _width + _borderWidth; x++)
                {
                    float xPosition = x * _horizontalOffset;
                    float yPosition = y * _verticalOffset;
                    
                    if (x % 2 != 0)
                    {
                        yPosition += 0.5f;
                    }
                    
                    if (x >= _origin.x && x < _origin.x + _width && y >= _origin.y && y < _origin.y + _height)
                    {
                        // Inside the play area.
                        ScalePositions.Add(new Vector2(xPosition, yPosition));
                    }
                    else
                    {
                        // Outside the play area.
                        BorderScalePositions.Add(new Vector2(xPosition, yPosition));
                    }
                }
            }
            
            // Update variable values.
            _widthVariable.SetValue(_width);
            _heightVariable.SetValue(_height);
            _totalScalesVariable.SetValue(TotalScales);
            _centerPointVariable.SetValue(CenterPoint);
            _centerPointVariable.Invoke();
        }
    }
}
