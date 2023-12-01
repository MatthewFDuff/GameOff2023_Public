using UnityEngine;

namespace _Core._Scripts.Utility
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = FindObjectOfType<T>();

                if (_instance != null) return _instance;
                    
                GameObject obj = new();
                _instance = obj.AddComponent<T>();
                obj.name = typeof(T).Name;
                
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            // If the instance is null, we need to assign it.
            if (_instance == null)
            {
                // Assign the instance.
                _instance = this as T;
            }
            // If the instance is not null and it is not this, we need to destroy it.
            else if (_instance != this)
            {
                // Destroy the object.
                Destroy(gameObject);
            }
        }
    }
}
