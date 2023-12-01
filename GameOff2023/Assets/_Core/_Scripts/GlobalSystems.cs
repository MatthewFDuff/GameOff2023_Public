using UnityEngine;

namespace _Core._Scripts
{
    public class GlobalSystems : PersistentSingleton<GlobalSystems>
    {
    }

    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                _instance = FindObjectOfType<T>();

                if (_instance != null) return _instance;
                
                GameObject newInstance = new GameObject();
                _instance = newInstance.AddComponent<T>();

                return _instance;
            }
        }

        /// <summary>
        /// On awake, we check if there's already a copy of the object in the scene. If there's one, we destroy it.
        /// </summary>
        protected virtual void Awake()
        {
            if (!Application.isPlaying) return;

            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(transform.gameObject);
            }
            else
            {
                if(this != _instance) Destroy(gameObject);
            }
        }
    }
}
