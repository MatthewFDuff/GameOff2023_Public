using System.Collections.Generic;
using System.Linq;
using _Core._Scripts.Data;
using _Core._Scripts.Data.Variables;
using _Core._Scripts.Systems;
using DG.Tweening;
using HeathenEngineering;
using HeathenEngineering.Events;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Core._Scripts
{
    public class Scale : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ScaleData _scaleData;
        [SerializeField] private Vector2 _scalePosition;
        
        [SerializeField] private FloatVariable _currentPickaxeDamage;

        [SerializeField] private GameEvent OnScaleCleaned;

        [SerializeField] private AudioClip _scaleCleanedSoundEffect;
        [SerializeField] private AudioClip _scaleFullyCleanedSoundEffect;

        [SerializeField] private bool _shakeOnStart;

        /// <summary>
        /// How much health the scale has left, before it is cleaned.
        /// </summary>
        [SerializeField] private int _scaleHealth;

        /// <summary>
        /// The total health of the scale before it is cleaned.
        /// </summary>
        [SerializeField] private int _scaleTotalHealth;

        [SerializeField] private DragonVariable _currentDragon;

        [Header("Scale Sprites")] 
        [SerializeField] private Sprite _dirtyScaleSpritePartial;

        [SerializeField] private Sprite _dirtyScaleSpriteHalf;
        [SerializeField] private Sprite _dirtyScaleSpriteFull;

        public List<Scale> AdjacentScales = new();

        [SerializeField] private IntVariable _currentLevel;


        [SerializeField] private bool _isHealthDisplayEnabled = true;
        [SerializeField] private TextMeshPro _healthDisplay;

        /// <summary>
        /// The previously cleaned scale. Can be null at the start.
        /// </summary>
        private static Scale _previousScale;
        [SerializeField] private IntVariable _currentCombo;
        [SerializeField] private FloatVariable _maximumCombo;
        [SerializeField] private FloatVariable _comboMultiplier;
        [SerializeField] private List<AudioClip> _comboSoundEffects;
        public ScaleData ScaleData => _scaleData;
        public Vector2 ScalePosition => _scalePosition;

        public bool IsCleaned => _scaleHealth <= 0;
        public int AdjacentDirtyScaleCount => AdjacentScales.Where(scale => scale != null).Count(scale => !scale.IsCleaned);

        #region Lifecycle Methods

        private void Awake()
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        #endregion

        private void UpdateScaleSprite()
        {
            if (_scaleData == null) return;

            if (_scaleData.ScaleType == ScaleType.Dirty)
            {
                _healthDisplay.enabled = true;
                UpdateDirtySprite();
            }
            else
            {
                _healthDisplay.enabled = false;
                _spriteRenderer.sprite = _scaleData.Sprite;
            }

            UpdateColor();
        }

        private void UpdateColor()
        {
            if (_currentDragon.Value == null)
            {
                _spriteRenderer.color = Color.white;
                return;
            }

            switch (_scaleData.ScaleType)
            {
                case ScaleType.Default:
                    _spriteRenderer.color = _currentDragon.Value.BorderScaleColor;
                    break;
                case ScaleType.Clean:
                    _spriteRenderer.color = _currentDragon.Value.CleanScaleColor;
                    break;
                case ScaleType.Dirty:
                    _spriteRenderer.color = _currentDragon.Value.DirtyScaleColor;
                    break;
                case ScaleType.Fire:
                case ScaleType.Air:
                case ScaleType.Earth:
                case ScaleType.Water:
                case ScaleType.Crystal:
                case ScaleType.Nature:
                default:
                    _spriteRenderer.color = Color.white;
                    break;
            }
        }

        public void SetPosition(Vector2 position)
        {
            _scalePosition = position;
        }

        public void SetScaleData(ScaleData scaleData, Color color = default)
        {
            if (_scaleData == null) return;

            _scaleData = scaleData;

            // Apply level scaling. Every three levels, increase hp by 1.
            int maxBonusHealth = Mathf.FloorToInt((_currentLevel.Value + 1) / 3);
            int minHealth = _scaleData.BaseHealth;
            int randomHealth = Random.Range(minHealth, minHealth + maxBonusHealth);
            int health = minHealth;// Clean/Default Scales
            if (_scaleData.ScaleType == ScaleType.Dirty)
            {
                // Dirty scales.
                health = randomHealth;
            }
            else if (_scaleData.ScaleType != ScaleType.Clean && _scaleData.ScaleType != ScaleType.Default && scaleData.ScaleType != ScaleType.Dirty)
            {
                //Special scales.
                health = minHealth + maxBonusHealth;
            }
            
            _scaleHealth = health;
            _scaleTotalHealth = health;
            
            SetHealthDisplay();
            
            UpdateScaleSprite();

            if (_shakeOnStart)
            {
                ShakeScale();
            }
        }

        /// <summary>
        /// Updates the total health of the scale based on the selected tool.
        /// </summary>
        public void UpdateHealth()
        {
            MineScale((int)_currentPickaxeDamage.Value);
        }

        /// <summary>
        /// Used by mining robot and bombs.
        /// </summary>
        /// <param name="damage"></param>
        public void MineScale(int damage, Tools tool = Tools.Pickaxe)
        {
            // Scale is already cleaned.
            if (_scaleHealth <= 0) return;

            // Scale is not interactable.
            if (_scaleData.RequiredTool == Tools.None) return;

            _scaleHealth -= damage;
            
            SetHealthDisplay();
            ShakeScale();
            CleanScale(tool);
        }

        
        private void OnDrawGizmosSelected()
        {
            foreach (var adjacentScale in AdjacentScales)
            {
                Gizmos.color = Color.red;
                var position = transform.position;
                var adjacentPosition = adjacentScale.transform.position;
                position += Vector3.right * 0.5f;
                adjacentPosition += Vector3.right * 0.5f;
                Gizmos.DrawLine(position, adjacentPosition);
            }
        }

        private void CleanScale(Tools tool = Tools.Pickaxe)
        {
            if (IsCleaned)
            {
                // When a scale is cleaned, we need to update the current combo based on if the player cleaned a scale
                // that was adjacent to the previously cleaned scale.
                
                // If the previous scale is null, then this is the first scale cleaned.
                if (tool == Tools.Pickaxe)
                {
                    if (_previousScale == null)
                    {
                        // New combo
                        AudioManager.Instance.PlayEffect(_comboSoundEffects[0]);
                        _currentCombo.Value = 1;
                        _previousScale = this;
                    }
                    else
                    {
                        // Check if the previous scale is adjacent to this scale.
                        if (AdjacentScales.Contains(_previousScale))
                        {
                            // Continue combo up to max combo amount.
                            // AudioManager.Instance.PlayEffect(_comboSoundEffects[_currentCombo.Value]);
                            if (_currentCombo.Value + 1 > _maximumCombo.Value)
                            {
                                _currentCombo.Value = (int)_maximumCombo.Value;
                            }
                            else
                            {
                                _currentCombo.Value++;
                            }
                            
                            _previousScale = this;
                        }
                        else
                        {
                            // Reset combo
                            // AudioManager.Instance.PlayEffect(_comboSoundEffects[0]);
                            _currentCombo.Value = 1;
                            _previousScale = this;
                        }    
                    }
                }

                if (tool == Tools.Pickaxe)
                {
                    _scaleData.DropResources(_currentCombo.Value, _comboMultiplier.Value, transform.position);
                }
                else
                {
                    _scaleData.DropResources(transform.position);
                }
                
                // The scale will become the next type down. 
                SetScaleData(ScaleManager.Instance.GetScaleDataByType(_scaleData.TransformedScaleType));
                AudioManager.Instance.PlayEffect(_scaleFullyCleanedSoundEffect);
                OnScaleCleaned.Raise();
            }
            else
            {
                // Scale is still dirty.
                SetHealthDisplay();
                
                // Update sprite based on health.
                UpdateDirtySprite();

                // Play default sound effect.
                AudioManager.Instance.PlayEffect(_scaleCleanedSoundEffect);
            }
        }

        private void SetHealthDisplay()
        {
            return;
            if(_scaleData.ScaleType == ScaleType.Clean || _scaleData.ScaleType == ScaleType.Default) return;

            if (_scaleHealth < _scaleTotalHealth)
            {
                _healthDisplay.text = _scaleHealth.ToString();
            }
            else
            {
                _healthDisplay.text = "";
            }
        }

        private void UpdateDirtySprite()
        {
            if (_scaleData.ScaleType != ScaleType.Dirty) return;

            switch (_scaleHealth)
            {
                // Unique situations for scales with low health. They shouldn't appear completely dirty.
                case 1:
                    _spriteRenderer.sprite = _dirtyScaleSpritePartial;
                    return;
                case 2:
                    _spriteRenderer.sprite = _dirtyScaleSpriteHalf;
                    return;
                case 3: _spriteRenderer.sprite = _dirtyScaleSpriteFull;
                    return;
            }

            if(_scaleHealth <= _scaleTotalHealth * 0.5)  // e.g. 100HP * 0.5 = 50HP
            {
                _spriteRenderer.sprite = _dirtyScaleSpritePartial;
            }
            else if (_scaleHealth <= _scaleTotalHealth * 0.7) // e.g. 100HP * 0.7 = 70HP
            {
                _spriteRenderer.sprite = _dirtyScaleSpriteHalf;
            }
            else
            {
                _spriteRenderer.sprite = _dirtyScaleSpriteFull;
            }
        }

        public void ShakeScale()
        {
            transform.DOShakePosition(0.3f, 0.1f).OnComplete(ResetPosition);
        }
        
        /// <summary>
        /// Used to fix an issue where scales were eventually going out of place.
        /// </summary>
        private void ResetPosition()
        {
            transform.position = _scalePosition;
        }

        /// <summary>
        /// When called, this scale will shake and then select a random adjacent scale to trigger.
        /// </summary>
        public void TriggerChain()
        {
            transform.DOScale(1.1f, 0.3f).Play().OnComplete(TriggerAdjacentScale);
        }

        private void TriggerAdjacentScale()
        {
            if (AdjacentScales.Count <= 0) return;

            AdjacentScales[Random.Range(0, AdjacentScales.Count)].TriggerChain();
        }
    }
}