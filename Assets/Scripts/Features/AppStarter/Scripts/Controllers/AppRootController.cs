using System.Threading;
using Core.Controllers.Core.Controllers;
using Core.Controllers.Core.Interfaces;
using Cysharp.Threading.Tasks;
using Features.AppStarter.Scripts.Controllers;
using UnityEngine;

public class AppRootController : RootController
{
    public AppRootController(IControllerFactory controllerFactory) : base(controllerFactory)
    {
    }

    protected override void OnStart()
    {
        base.OnStart();

        Debug.Log("Flow started");
        StartFlowAsync(CancellationToken).Forget();
    }

    private async UniTask StartFlowAsync(CancellationToken cancellationToken)
    {
        Execute<LaunchAppController>(cancellationToken);
    }
}