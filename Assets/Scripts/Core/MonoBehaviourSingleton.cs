using UnityEngine;

namespace Core
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
    {
        private static T _instance;
        
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Debug.LogWarning($"Attempt to create multiple singleton instances {name}.");
                return;
            }
            
            _instance = (T)this;
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}