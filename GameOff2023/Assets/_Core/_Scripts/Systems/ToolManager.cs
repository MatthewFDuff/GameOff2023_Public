using System.Linq;
using _Core._Scripts.Data;
using _Core._Scripts.Data.Variables;
using _Core._Scripts.UI;
using UnityEngine;

namespace _Core._Scripts.Systems
{
    /// <summary>
    /// Handles the selection and deselection of tools.
    /// </summary>
    public class ToolManager : MonoBehaviour
    {
        /// <summary>
        /// The currently selected tool.
        /// </summary>
        [SerializeField] private ToolVariable _selectedTool;

        [SerializeField] private ToolData _defaultTool;
        
        /// <summary>
        /// A list of the tools that are available to the player.
        /// </summary>
        [SerializeField] private ToolData[] _tools;
        
        #region Lifecycle Methods

        private void Start()
        {
            if (_selectedTool == null)
            {
                Debug.LogError("No selected tool variable assigned to ToolManager!");
            }
            
            if (_tools == null || _tools.Length == 0)
            {
                Debug.LogError("No tools assigned to ToolManager!");
            }
            
            if (_defaultTool == null)
            {
                Debug.LogError("No default tool assigned to ToolManager!");
            }
            
            _selectedTool.Value = _defaultTool;
        }

        /// <summary>
        /// Subscribe to events.
        /// </summary>
        private void OnEnable()
        {
            ToolButton.OnButtonClicked += HandleToolSelected;
        }

        /// <summary>
        /// Unsubscribe from events.
        /// </summary>
        private void OnDisable()
        {
            ToolButton.OnButtonClicked -= HandleToolSelected;
        }
        
        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the selection of a tool. 
        /// </summary>
        /// <param name="index">The index of the tool to be selected.</param>
        private void HandleToolSelected(int index)
        {
            ToolData tool = _tools[index];
            
            _selectedTool.Value = tool == _selectedTool.Value ? _defaultTool : tool;
        }
        
        /// <summary>
        /// Handles the selection of a tool. 
        /// </summary>
        /// <param name="toolData">The data of the tool to be selected.</param>
        private void HandleToolSelected(ToolData toolData)
        {
            ToolData tool = _tools.First(t => t == toolData);
            
            _selectedTool.Value = tool == _selectedTool.Value ? _defaultTool : tool;
        }

        #endregion
    }
}
