using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Pool.Unity
{
    public class MonoBehaviourPool : MonoBehaviourSingleton<MonoBehaviourPool>
    {
        [Serializable]
        private class PoolingObjectProperties
        {
            public MonoBehaviourPooling Template;
            public int CacheIncrementStep;
            public int MaxCacheSize;
        }
        
        [SerializeField] private List<PoolingObjectProperties> _templates;

        private Dictionary<MonoBehaviourPooling, Pool<MonoBehaviourPooling>> _pools;

        protected override void Awake()
        {
            base.Awake();

            _pools = new Dictionary<MonoBehaviourPooling, Pool<MonoBehaviourPooling>>(_templates.Count);
            foreach (var record in _templates)
            {
                var pool = new Pool<MonoBehaviourPooling>(record.Template, record.CacheIncrementStep, record.MaxCacheSize);
                _pools.Add(record.Template, pool);
            }
        }

        protected override void OnDestroy()
        {
            foreach (var (_, pool) in _pools)
            {
                pool.Clear();
            }
            _pools.Clear();
            
            base.OnDestroy();
        }
    }
}