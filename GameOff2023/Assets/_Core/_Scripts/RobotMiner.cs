using _Core._Scripts.Data;
using _Core._Scripts.Systems;
using HeathenEngineering;
using UnityEngine;

namespace _Core._Scripts
{
    [RequireComponent(typeof(Animator))]
    public class RobotMiner : MonoBehaviour
    {
        private ToolData _toolData;

        [Header("Variables")] 
        [SerializeField] protected FloatVariable MiningSpeed;
        [SerializeField] protected FloatVariable MovementSpeed;
        
        /// <summary>
        /// The states the robot can be in.
        /// Idle - No available scales to mine.
        /// Moving - Moving to a scale.
        /// Mining - Mining a scale.
        /// </summary>
        protected enum RobotState
        {
            Idle,
            Moving,
            Mining
        }

        protected float MiningTimer;
        
        [SerializeField] private float _xOffset = 0.5f;
        
        public int MiningDamage = 1;

        protected RobotState CurrentState = RobotState.Idle;
        
        public Scale TargetScale { get; private set; }

        private Animator _animator;
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        private static readonly int IsMining = Animator.StringToHash("IsMining");
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            switch (CurrentState)
            {
                case RobotState.Idle:
                    FindMiningTarget();
                    break;
                case RobotState.Moving:
                    CancelActivity();
                    MoveToScale();
                    break;
                case RobotState.Mining:
                    CancelActivity();
                    MineScale();
                    break;
                default:
                    CurrentState = RobotState.Idle;
                    break;
            }

            UpdateAnimatorParameters();
        }

        // If a player mines a scale before the robot gets there, or before the robot finishes mining, cancel the activity.
        protected virtual void CancelActivity()
        {
            if (TargetScale.IsCleaned)
            {
                // Reset to idle so it can find a new target.
                CurrentState = RobotState.Idle;
            }
        }

        private void UpdateAnimatorParameters()
        {
            if (_animator == null) return;
            
            _animator.SetBool(IsIdle, CurrentState == RobotState.Idle);
            _animator.SetBool(IsMoving, CurrentState == RobotState.Moving);
            _animator.SetBool(IsMining, CurrentState == RobotState.Mining);
        }
        
        private void FindMiningTarget()
        {
            // Implement logic to find an available mining node
            TargetScale = FindAvailableScale();

            if (TargetScale != null)
            {
                CurrentState = RobotState.Moving;
            }
        }

        private void MoveToScale()
        {
            // Calculate the target position for the robot.
            // Doesn't actually need to be in proximity to the scale to mine it.
            // Just for visual purposes.
            Vector3 targetPosition = TargetScale.transform.position;
            targetPosition.x += _xOffset;
            
            // Movement logic. Lerp for juice.
            Vector3 currentPosition = transform.position;
            // float distance = Vector3.Distance(currentPosition, targetPosition);
            // float timeToTarget = distance / _maxDistance / MovementSpeed;
            // transform.DOMove(targetPosition, timeToTarget).SetEase(_movementEase);
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, MovementSpeed.Value * Time.deltaTime);

            if (transform.position == targetPosition)
            {
                CurrentState = RobotState.Mining;
            }
        }

        protected virtual void MineScale()
        {
            MiningTimer += Time.deltaTime;

            // Check if enough time has passed to mine ore
            if (MiningTimer >= MiningSpeed.Value)
            {
                TargetScale.MineScale(MiningDamage, Tools.RobotMiner);
                
                // Reset the timer
                MiningTimer = 0f;

                if (TargetScale.IsCleaned)
                {
                    // Go back to idling and search for a new node to mine. 
                    CurrentState = RobotState.Idle;
                }
            }
        }

        protected virtual Scale FindAvailableScale()
        {
            Vector3 currentPosition = transform.position;
            currentPosition.x -= _xOffset;
            
            return RobotManager.Instance.GetNextTarget(currentPosition);
        }
    }
}
