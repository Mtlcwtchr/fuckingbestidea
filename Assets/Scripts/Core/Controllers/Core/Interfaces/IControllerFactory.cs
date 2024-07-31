namespace Core.Controllers.Core.Interfaces
{
    public interface IControllerFactory
    {
        IController Create<T>() where T : class, IController;
    }
}