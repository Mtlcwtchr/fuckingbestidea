using System;
using System.Collections.Generic;
using Core.Utilities;

namespace Core.Pool
{
    public abstract class Pool<T> : IPool<T> where T : IPooling
    {
        private readonly List<T> _items;

        private readonly int _step;
        private readonly int _max;
        
        private int _currentSize;

        protected Pool(int step, int max)
        {
            _step = step;
            _max = max;
            _currentSize = Math.Min(_step, _max);
            _items = new List<T>(_currentSize);
        }
        
        public T Get()
        {
            if (_items.Count < 1)
            {
                CreateInstances(_step);
            }

            var item = _items.PopBack();
            item.Take();
            return item;
        }

        public void Free(T t)
        {
            t.Free();
            _items.Add(t);
        }

        private void CreateInstances(int count)
        {
            while (--count >= 0)
            {
                if (_currentSize >= _max)
                    return;
                
                _items.Add(CreateInstance());
                ++_currentSize;
            }
        }

        public void Clear()
        {
            foreach (var item in _items)
            {
                item.Dispose();
            }
            _items.Clear();
        }

        protected abstract T CreateInstance();
    }
}