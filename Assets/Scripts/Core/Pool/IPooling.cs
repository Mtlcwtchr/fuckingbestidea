namespace Core.Pool
{
    public interface IPooling
    {
        void Take();
        void Free();
        void Dispose();
    }
}