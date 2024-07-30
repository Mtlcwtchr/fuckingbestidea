using System;
using System.Diagnostics;
using System.Threading;
using Core.Controllers.Core.Interfaces;
using Core.Controllers.Core.States;

namespace Core.Controllers.Core.Controllers
{
    public abstract partial class ControllerBase
    {
        /// <summary>
        /// Executes a controller of type T.
        /// This method creates an instance of the controller and starts it.
        /// </summary>
        /// <typeparam name="T">The type of the controller to execute. Must implement IController.</typeparam>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        protected void Execute<T>(
            CancellationToken token)
            where T : class, IController
        {
            ThrowIfControllerWithResult<T>();
            ExecuteInternal<T>(null, token);
        }

        /// <summary>
        /// Executes a controller of type T that requires an argument of type TArg.
        /// This method creates an instance of the controller, sets the argument, and starts the controller.
        /// </summary>
        /// <typeparam name="T">The type of the controller to execute. Must implement IController and IController&lt;TArg&gt;.</typeparam>
        /// <typeparam name="TArg">The type of the argument required by the controller.</typeparam>
        /// <param name="arg">The argument to pass to the controller.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        protected void Execute<T, TArg>(
            TArg arg,
            CancellationToken token)
            where T : class, IController, IController<TArg>
        {
            ThrowIfControllerWithResult<T>();
            ExecuteInternal<T, TArg>(arg, null, token);
        }

        /// <summary>
        /// Executes a controller of type T using a specified controller factory.
        /// This method creates an instance of the controller and starts it.
        /// </summary>
        /// <typeparam name="T">The type of the controller to execute. Must implement IController.</typeparam>
        /// <param name="factory">The factory to use when creating the controller.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        protected void Execute<T>(
            IControllerFactory factory,
            CancellationToken token)
            where T : class, IController
        {
            ThrowIfControllerWithResult<T>();
            ExecuteInternal<T>(factory, token);
        }

        /// <summary>
        /// Executes a controller of type T that requires an argument of type TArg using a specified controller factory.
        /// This method creates an instance of the controller, sets the argument, and starts the controller.
        /// </summary>
        /// <typeparam name="T">The type of the controller to execute. Must implement IController and IController&lt;TArg&gt;.</typeparam>
        /// <typeparam name="TArg">The type of the argument required by the controller.</typeparam>
        /// <param name="arg">The argument to pass to the controller.</param>
        /// <param name="factory">The factory to use when creating the controller.</param>
        /// <param name="token">A cancellation token that can be used to cancel the operation.</param>
        protected void Execute<T, TArg>(
            TArg arg,
            IControllerFactory factory,
            CancellationToken token)
            where T : class, IController, IController<TArg>
        {
            ThrowIfControllerWithResult<T>();
            ExecuteInternal<T, TArg>(arg, factory, token);
        }

        private IController ExecuteInternal<T>(
            IControllerFactory factory,
            CancellationToken cancellationToken)
            where T : class, IController
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfControllerHasIncorrectState();
            var controller = Create<T>(factory);
            Start(controller, cancellationToken);
            return controller;
        }

        private IController ExecuteInternal<T, TArg>(
            TArg arg,
            IControllerFactory factory,
            CancellationToken cancellationToken)
            where T : class, IController, IController<TArg>
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfControllerHasIncorrectState();
            var controller = Create<T>(factory);
            ((IController<TArg>) controller).SetArgInternal(arg);
            Start(controller, cancellationToken);
            return controller;
        }

        private IController Create<T>(IControllerFactory factory)
            where T : class, IController
        {
            factory ??= _controllerFactory;
            var controller = factory.Create<T>();
            return controller;
        }

        private void Start(IController controller, CancellationToken token)
        {
            controller.Initialize(token, token);

            AddChild(controller);

            try
            {
                controller.Start();
            }
            catch (Exception exception)
            {
                using (controller)
                {
                    controller.Stop(exception);
                    RemoveChild(controller);
                }

                throw;
            }
        }

        [Conditional("UNITY_EDITOR")]
        private static void ThrowIfControllerWithResult<T>()
        {
            var controllerType = typeof(T);
            var isControllerWithResultBase = controllerType.IsSubclassOf(typeof(ControllerWithResultBase<,>));
            if (isControllerWithResultBase)
            {
                throw new Exception($"{controllerType.Name}: ControllerWithResult cannot be started with {nameof(Execute)}");
            }
        }

        [Conditional("UNITY_EDITOR")]
        private void ThrowIfControllerHasIncorrectState()
        {
            if (_state != ControllerState.Running)
            {
                throw new InvalidOperationException($"{ControllerName} Can't Execute from {_state} state.");
            }
        }
    }
}
