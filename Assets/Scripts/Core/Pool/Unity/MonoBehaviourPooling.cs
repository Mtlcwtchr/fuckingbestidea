namespace Core.Pool.Unity
{
    public class MonoBehaviourPooling : MonoBehaviourCachedVisibility, IPooling
    {
        public virtual void Take()
        {
            SetActive(true);
        }

        public virtual void Free()
        {
            SetActive(false);
        }

        public void Dispose()
        {
            if (_gameObject == null)
                return;
            
            Destroy(_gameObject);
        }
    }
}