using System;
using _Core._Scripts.Data;
using _Core._Scripts.Data.Variables;
using UnityEngine;
using UnityEngine.UI;

namespace _Core._Scripts.UI
{
    public class ToolButton : MonoBehaviour
    {
        [SerializeField] private ToolData _tool;
        [SerializeField] private Button _button;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        
        [SerializeField] private ToolVariable _selectedTool;
        
        public static event Action<ToolData> OnButtonClicked;
        
        [SerializeField] private Color _defaultAlphaColor;
        [SerializeField] private Color _selectedAlphaColor;
            
        [SerializeField] private float _defaultAlpha = 0.7f;
        [SerializeField] private float _selectedAlpha = 1f;
        
        


        private readonly Color _emptyColor = new(0, 0, 0, 0);
        #region Lifecycle Methods

        private void Start()
        {
            if (_defaultAlphaColor == _emptyColor)
            {
                _defaultAlphaColor = new Color(1, 1, 1, 0.7f);
            }
            
            if (_selectedAlphaColor == _emptyColor)
            {
                _selectedAlphaColor = new Color(1, 1, 1, 1f);
            }
        }

        private void OnEnable()
        {
            if (_button != null)
            {
                _button.onClick.AddListener(ButtonClicked);
            }
        }

        private void OnDisable()
        {
            if (_button != null)
            {
                _button.onClick.RemoveListener(ButtonClicked);
            }
        }

        private void Update()
        {
            if (_button == null) return;
            if(_tool.Sprite != null) _button.image.sprite = _tool.Sprite;
            
            if (_selectedTool.Value == null) return;
            
            _canvasGroup.alpha = _selectedTool.Value == _tool
                ? _selectedAlpha
                : _defaultAlpha;
        }

        #endregion
    
        private void ButtonClicked()
        {
            if (_tool == null) {
                Debug.Log("This button doesn't have a tool assigned yet.");
                return;
            }
        
            OnButtonClicked?.Invoke(_tool);
        }
    }
}
