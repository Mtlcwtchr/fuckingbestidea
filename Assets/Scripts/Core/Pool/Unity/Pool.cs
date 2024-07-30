using UnityEngine;

namespace Core.Pool.Unity
{
    public class Pool<T> : Core.Pool.Pool<T> where T : Object, IPooling
    {
        private readonly T _template;
        
        public Pool(T template, int step, int max) : base(step, max)
        {
            _template = template;
        }

        protected override T CreateInstance()
        {
            return Object.Instantiate(_template);
        }
    }
}