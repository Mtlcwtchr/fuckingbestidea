using System;
using System.Diagnostics;
using System.Threading;
using Core.Controllers.Core.Interfaces;

namespace Core.Controllers.Core.Controllers
{
    public abstract class RootController : ControllerBase
    {
        protected RootController(IControllerFactory controllerFactory) : base(controllerFactory)
        {
        }

        /// <summary>
        /// Launches the execution of the controller tree.
        /// This method initializes the root controller with the provided cancellation token, starts it, and registers a callback to stop the root controller when the cancellation token is cancelled.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        public void LaunchTree(CancellationToken cancellationToken)
        {
            var registration = cancellationToken.Register(StopRootController, true);
            AddDisposable(registration);
            ((IController)this).Initialize(cancellationToken, CancellationToken.None);
            ((IController)this).Start();
        }

        private void StopRootController()
        {
            ((IDisposable)this).Dispose();
        }

        protected override void OnStart()
        {
            SetRootController(this);
        }

        protected override void OnStop()
        {
            SetRootController(null);
        }

        public string DumpControllersTree()
        {
            return Dump();
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_BUILDTYPE_DEV")]
        private static void SetRootController(global::Core.Controllers.Core.Controllers.ControllerBase controller)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD || UNITY_BUILDTYPE_DEV
            Instance = controller;
#endif
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD || UNITY_BUILDTYPE_DEV
        public static global::Core.Controllers.Core.Controllers.ControllerBase Instance { get; private set; }
#endif
    }
}
