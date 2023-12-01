using System.Collections.Generic;
using _Core._Scripts.Utility;
using HeathenEngineering;
using HeathenEngineering.Events;
using UnityEngine;

namespace _Core._Scripts.Systems
{
    public class RobotManager : Singleton<RobotManager>
    {
        [SerializeField] private GameObject _robotMinerPrefab;
        [SerializeField] private GameObject _robotBomberPrefab;

        [SerializeField] private Transform _robotMinerParentTransform;
        [SerializeField] private Transform _robotBomberParentTransform;
        
        [SerializeField] private List<RobotMiner> _robotMiners = new();
        [SerializeField] private List<RobotBomber> _robotBombers = new();
        
        [SerializeField] private FloatVariable _robotMinerCount;
        [SerializeField] private FloatVariable _robotBomberCount;

        #region Lifecycle Methods

        private void OnEnable()
        {
            _robotMinerCount.AddListener(HandleRobotMinerQuantityChanged);
            _robotBomberCount.AddListener(HandleRobotBomberQuantityChanged);
        }

        private void OnDisable()
        {
            _robotMinerCount.RemoveListener(HandleRobotMinerQuantityChanged);
            _robotBomberCount.RemoveListener(HandleRobotBomberQuantityChanged);
        }

        #endregion

        #region Event Handlers

        private void HandleRobotMinerQuantityChanged(EventData<float> arg0)
        {
            SpawnRobotMiner();
        }
        private void HandleRobotBomberQuantityChanged(EventData<float> arg0)
        {
            SpawnRobotBomber();
        }

        #endregion
        
        [ContextMenu("Spawn Robot Bomber")]
        public void SpawnRobotBomber()
        {
            RobotBomber robotBomber = Instantiate(_robotBomberPrefab, _robotBomberParentTransform).GetComponent<RobotBomber>();
            _robotBombers.Add(robotBomber);
        }
    
        [ContextMenu("Spawn Robot Miner")]
        public void SpawnRobotMiner()
        {
            RobotMiner robotMiner = Instantiate(_robotMinerPrefab, _robotMinerParentTransform).GetComponent<RobotMiner>();
            _robotMiners.Add(robotMiner);
        }

        /// <summary>
        /// Used by robots to get their next target to move to.
        /// Will not target a scale that is already targeted by other robots.
        /// </summary>
        /// <param name="robotPosition"></param>
        public Scale GetNextTarget(Vector2 robotPosition)
        {
            List<Scale> possibleTargets = ScaleManager.Instance.ScaleDistances(robotPosition);
            
            // Remove all scales that are already targeted by other robots.
            foreach (RobotMiner robotMiner in _robotMiners)
            {
                possibleTargets.RemoveAll(scale => scale == robotMiner.TargetScale);
                
                // This could also be done a different way, where we just go through the list until we find one that isn't targeted.
                // Which would probably be more efficient. 
            }

            // If all scales are targeted already, then return null so the robot idles and waits for new scales.
            // Otherwise the first scale in the list is the closest one available.
            return possibleTargets.Count == 0 ? null : possibleTargets[0];
        }
    }
}
