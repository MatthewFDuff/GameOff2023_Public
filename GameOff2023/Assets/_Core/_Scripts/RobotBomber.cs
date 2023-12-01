using System.Collections.Generic;
using System.Linq;
using HeathenEngineering;
using UnityEngine;

namespace _Core._Scripts
{
    public class RobotBomber : RobotMiner
    {
        [SerializeField] protected FloatVariable BombSize;
        
        protected override void MineScale()
        {
            MiningTimer += Time.deltaTime;

            // Check if enough time has passed to mine ore
            if (MiningTimer >= MiningSpeed.Value)
            {
                var scalesToMine = new List<Scale>();
                var scalesToSearch = new List<Scale> {TargetScale};

                for (int i = 0; i < BombSize.Value; i++)
                {
                    var newScalesToSearch = new List<Scale>();
                    foreach (var scale in scalesToSearch)
                    {
                        if(!scalesToMine.Contains(scale)) scalesToMine.Add(scale);
                        
                        Debug.Log(scale);
                        foreach (var adjScale in scale.AdjacentScales)
                        {
                            if(!newScalesToSearch.Contains(adjScale)) newScalesToSearch.Add(adjScale);
                        }
                    }

                    scalesToSearch = newScalesToSearch;
                }

                foreach (var scale in scalesToMine)
                {
                    scale.MineScale(MiningDamage, Tools.RobotBomber);
                }
                
                // Reset the timer
                MiningTimer = 0f;

                if (TargetScale.IsCleaned)
                {
                    // Go back to idling and search for a new node to mine. 
                    CurrentState = RobotState.Idle;
                }
            }
        }

        protected override void CancelActivity()
        {
            var cancel = TargetScale.AdjacentScales.Count(s => s.IsCleaned) == 4 && TargetScale.IsCleaned;
            if (cancel)
            {
                // Reset to idle so it can find a new target.
                CurrentState = RobotState.Idle;
            }
        }

        /// <summary>
        /// We get scale that is dirtiest
        /// </summary>
        /// <returns>Dirtiest Scale</returns>
        protected override Scale FindAvailableScale()
        {
            var scale = base.FindAvailableScale();
            if (BombSize.Value == 1) return scale;
            var retScale = scale;
            foreach (var adjacentScale in scale.AdjacentScales)
            {
                if(adjacentScale is null) continue;
                if (adjacentScale.AdjacentDirtyScaleCount > retScale.AdjacentDirtyScaleCount)
                {
                    retScale = adjacentScale;
                }
            }
            return retScale;
        }
    }
}
