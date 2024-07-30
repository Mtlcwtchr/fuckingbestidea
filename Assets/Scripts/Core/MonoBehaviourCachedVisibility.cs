using System;
using UnityEngine;

namespace Core
{
    public class MonoBehaviourCachedVisibility : MonoBehaviour
    {
        public event Action<bool> OnVisibilityChanged;
        
        protected GameObject _gameObject;
        protected bool _isActive;
        
        protected virtual void Awake()
        {
            _gameObject = gameObject;
            _isActive = _gameObject.activeSelf;
        }

        public void SetActive(bool isActive)
        {
            if (_isActive == isActive)
                return;

            _isActive = isActive;
            _gameObject.SetActive(isActive);
            OnVisibilityChanged?.Invoke(_isActive);
        }
    }
}