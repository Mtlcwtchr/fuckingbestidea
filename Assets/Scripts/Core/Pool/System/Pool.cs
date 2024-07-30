namespace Core.Pool.System
{
    public class Pool<T> : Core.Pool.Pool<T> where T : IPooling, new()
    {
        public Pool(int step, int max) : base(step, max)
        {
        }

        protected override T CreateInstance()
        {
            return new T();
        }
    }
}