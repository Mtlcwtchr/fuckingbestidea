using System.Collections.Generic;
using Core.Pool;

namespace Core.Shared
{
    public static class SharedList<T>
    {
        private class PoolingList : List<T>, IPooling
        {
            private List<T> _list = new();

            public List<T> List => _list;

            public void Take() { }

            public void Free() { _list.Clear(); }

            public void Dispose()
            {
                _list.Clear();
                _list = null;
            }
        }

        private static Pool.System.Pool<PoolingList> _listPool = new (4, 16);
        private static List<PoolingList> _busy = new (16);

        public static List<T> Get()
        {
            var list = _listPool.Get();
            _busy.Add(list);
            return list?.List;
        }
        
        public static void Free(List<T> list)
        {
            for (var i = 0; i < _busy.Count; i++)
            {
                var e = _busy[i];
                if (e.List == list)
                {
                    _listPool.Free(e);
                    _busy.RemoveAt(i);
                    return;
                }
            }
        }
    }
}