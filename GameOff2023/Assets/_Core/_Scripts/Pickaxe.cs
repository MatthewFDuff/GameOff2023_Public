using UnityEditor.Animations;
using UnityEngine;

namespace _Core._Scripts
{
    /// <summary>
    /// Manages the current pickaxe sprites and animations.
    /// </summary>
    [CreateAssetMenu(menuName = "Custom/Pickaxe", fileName = "New Pickaxe")]
    public class Pickaxe : ScriptableObject
    {
        public PickaxeType PickaxeType => _pickaxeType;
        public Sprite PickaxeSprite => _pickaxeSprite;
        public AnimatorController PickaxeAnimatorController => _pickaxeAnimatorController;
        public AnimationClip IdleAnimation => _idleAnimation;
        public AnimationClip PickaxeAnimation => _pickaxeAnimation;
        
        [Tooltip("The type of pickaxe that this is.")]
        [SerializeField] private PickaxeType _pickaxeType;
        [Tooltip("The sprite that displays when the player is idle (not clicking).")]
        [SerializeField] private Sprite _pickaxeSprite;
        [Tooltip("The animator controller that controls the pickaxe animations.")]
        [SerializeField] private AnimatorController _pickaxeAnimatorController;
        [Tooltip("The animation that plays when the player is idle (not clicking).")]
        [SerializeField] private AnimationClip _idleAnimation;
        [Tooltip("The animation that plays when the player is mining (clicking).")]
        [SerializeField] private AnimationClip _pickaxeAnimation;
    }

    public enum PickaxeType
    {
        Rusty = 1,
        Malachite = 2,
        Onyx = 3,
        Amethyst = 4,
        Emerald = 5,
        Topaz = 6,
        Ruby = 7,
        Diamond = 8
    }
}