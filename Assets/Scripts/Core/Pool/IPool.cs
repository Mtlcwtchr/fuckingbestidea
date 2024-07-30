namespace Core.Pool
{
    public interface IPool<T> where T : IPooling
    {
        T Get();
        void Free(T t);
        void Clear();
    }
}