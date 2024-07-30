using System.Threading;
using Core.Controllers.Core.Controllers;
using Core.Controllers.Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.AppStarter.Scripts.Controllers
{
    public class LaunchAppController : ControllerBase
    {
        public LaunchAppController(IControllerFactory controllerFactory) : base(controllerFactory)
        {
        }

        protected override void OnStart()
        {
            LaunchFlowAsync(CancellationToken).Forget(e => Debug.LogError($"Loading flow error- {e.Message}"));
        }

        private async UniTask LaunchFlowAsync(CancellationToken cancellationToken)
        {
            var startViewControllersTask = StartSplashScreenController(cancellationToken);
            var initializeNonLoginServicesTask = InitializeNonLoginServices(cancellationToken);
            var startServicesWithResourcesLoading = StartServicesWithResourcesLoading(cancellationToken);

            await UniTask.WhenAll(startViewControllersTask, initializeNonLoginServicesTask, startServicesWithResourcesLoading);
            cancellationToken.ThrowIfCancellationRequested();

            //Execute<LaunchGameController>(cancellationToken);
        }

        private UniTask StartSplashScreenController(CancellationToken cancellationToken)
        {

            // s

            return UniTask.CompletedTask;
        }

        private UniTask InitializeNonLoginServices(CancellationToken cancellationToken)
        {
            //Execute<ThirdPartyInitializeController>(cancellationToken);
            return UniTask.CompletedTask;
        }

        private UniTask StartServicesWithResourcesLoading(CancellationToken cancellationToken)
        {
            // load all resources
            return UniTask.CompletedTask;
        }
    }
}