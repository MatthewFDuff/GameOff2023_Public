using System.Collections.Generic;
using System.Linq;
using HeathenEngineering;
using HeathenEngineering.Events;
using UnityEngine;

namespace _Core._Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MouseCursor : MonoBehaviour
    {
        [Tooltip("The currently selected tool.")]
        [SerializeField] private FloatVariable _currentPickaxeUpgrade;
        
        [SerializeField] private List<Pickaxe> _pickaxes = new();
        [SerializeField] private SpriteRenderer _cursorImage;
        [SerializeField] private bool _isCursorVisible = true;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if (_cursorImage == null) _cursorImage = GetComponent<SpriteRenderer>();
            if (_animator == null) _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_cursorImage == null) return;
            if (Camera.main == null) return;
            
            Cursor.visible = _isCursorVisible;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cursorPosition = new(mousePos.x, mousePos.y, -6f);
            _cursorImage.transform.position = cursorPosition;
        }

        private void OnEnable()
        {
            _currentPickaxeUpgrade.AddListener(UpdatePickaxe);
        }
        
        private void OnDisable()
        {
            _currentPickaxeUpgrade.RemoveListener(UpdatePickaxe);
        }

        private void Start()
        {
            UpdatePickaxe(new EventData<float>());
        }

        private void UpdatePickaxe(EventData<float> eventData)
        {
            var pickaxe = GetCurrentPickaxe();
            
            if (pickaxe == null) return;
            
            _cursorImage.sprite = pickaxe.PickaxeSprite;
            _animator.runtimeAnimatorController = pickaxe.PickaxeAnimatorController;
        }

        private Pickaxe GetCurrentPickaxe()
        {
            int pickAxeIndex = (int) _currentPickaxeUpgrade.Value;
            
            PickaxeType pickaxeType = (PickaxeType) pickAxeIndex;
            Debug.Log("Current pickaxe type: " + pickaxeType);

            return _pickaxes.FirstOrDefault(pickaxe => pickaxe.PickaxeType == pickaxeType);
        }
    }
}
