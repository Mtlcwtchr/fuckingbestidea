using Core.Controllers.Core.Interfaces;
using System;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Features.AppStarter.Scripts.EntryPoints
{
    public class AppStarter : IStartable, IDisposable
    {
        private readonly IObjectResolver _scope;
        private AppRootController _rootController;
        private CancellationTokenSource _tokenSource;

        public AppStarter(IObjectResolver scope)
        {
            _scope = scope;
        }

        public void Start()
        {
            Application.quitting += Clear;

            var factory = _scope.Resolve<IControllerFactory>();
            _tokenSource = new CancellationTokenSource();
            try
            {
                _rootController = GetRootController(factory);
                _rootController.LaunchTree(_tokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception exception)
            {
                Debug.LogError($"Can't start app root controller {exception}");
            }
        }


        private AppRootController GetRootController(IControllerFactory factory)
        {
            return new AppRootController(factory);
        }

        void IDisposable.Dispose()
        {
            Application.quitting -= Clear;
            Clear();
        }

        private void CancelTokenAndDispose()
        {
            if (_tokenSource == null)
            {
                return;
            }

            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = null;
        }

        private void Clear()
        {
            CancelTokenAndDispose();
        }
    }
}
