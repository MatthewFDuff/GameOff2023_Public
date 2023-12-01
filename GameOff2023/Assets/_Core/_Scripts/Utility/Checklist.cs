using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Core._Scripts.Utility
{
    /// <summary>
    /// A really scuffed to-do list. 
    /// </summary>
    public class Checklist : MonoBehaviour
    {
        [SerializeField] private List<ChecklistTask> _tasks;
    }

    [Serializable]
    public struct ChecklistTask
    {
        public bool IsComplete;
        public string TaskDescription;
    }
}
