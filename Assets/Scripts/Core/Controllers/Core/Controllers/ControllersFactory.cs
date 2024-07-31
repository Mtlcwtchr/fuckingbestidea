using Core.Controllers.Core.Interfaces;
using VContainer;
using VContainer.Unity;

namespace Core.Controllers.Core.Controllers
{
    public class ControllerFactory : IControllerFactory
    {
        private readonly IObjectResolver _container;

        public ControllerFactory(IObjectResolver container)
        {
            _container = container;
        }

        public IController Create<T>() where T : class, IController
        {
            var controller = _container.Resolve<T>();
            return controller;
        }

#if  UNITY_EDITOR
        public override string ToString()
        {
            const string errorMessage = "Cannot determine the LifeTime scope of the origin.";
            var lifeTimeScope = _container.ApplicationOrigin as LifetimeScope;
            if (lifeTimeScope == null)
            {
                return errorMessage;
            }

            return lifeTimeScope.GetType().Name;
        }
#endif
    }
}