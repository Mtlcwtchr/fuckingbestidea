using System;
using System.Threading;
using Core.Controllers.Core.Interfaces;
using Core.Controllers.Core.States;
using Cysharp.Threading.Tasks;

namespace Core.Controllers.Core.Controllers
{
    public abstract partial class ControllerWithResultBase<TArg, TResult> : ControllerBase<TArg>, IControllerWithResult<TResult>
    {
        private readonly UniTaskCompletionSource<TResult> _resultSource = new UniTaskCompletionSource<TResult>();
        private ControllerWithResultState _withResultState;

        protected ControllerWithResultBase(IControllerFactory controllerFactory)
            : base(controllerFactory)
        {
        }

        async UniTask IControllerWithResult<TResult>.FlowAsync(CancellationToken cancellationToken)
        {
            switch (_withResultState)
            {
                case ControllerWithResultState.None:
                {
                    _withResultState = ControllerWithResultState.WaitFlowAsync;
                    await OnFlowAsync(cancellationToken);
                    break;
                }
                case ControllerWithResultState.Completed:
                case ControllerWithResultState.Failed:
                    break;
                default:
                    throw new InvalidOperationException(
                        $"{ControllerName} Flow async called from incorrect state. Current state: {_withResultState}");
            }
        }

        async UniTask<TResult> IControllerWithResult<TResult>.GetResult(CancellationToken token)
        {
            switch (_withResultState)
            {
                case ControllerWithResultState.None:
                case ControllerWithResultState.WaitForResultAsync:
                    throw new InvalidOperationException(
                        $"{ControllerName} ControllerWithResult awaited from incorrect state: {_withResultState}");
                case ControllerWithResultState.WaitFlowAsync:
                    _withResultState = ControllerWithResultState.WaitForResultAsync;
                    break;
                case ControllerWithResultState.Completed:
                case ControllerWithResultState.Failed:
                    break;
            }

            using (token.Register(Cancel, true))
            {
                _withResultState = ControllerWithResultState.WaitForResultAsync;
                return await _resultSource.Task;
            }
        }
 
        /// <summary>
        /// Override this for controller async flow.
        /// Started automatically after execute controller.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        protected virtual UniTask OnFlowAsync(CancellationToken cancellationToken)
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Complete controller with result.
        /// </summary>
        /// <param name="result">Controller result.</param>
        protected void Complete(TResult result)
        {
            _withResultState = ControllerWithResultState.Completed;
            _resultSource.TrySetResult(result);
        }

        /// <summary>
        /// Complete controller with result.
        /// </summary>
        /// <param name="exception">Exception that caused the controller to stop operating.</param>
        protected void Fail(Exception exception)
        {
            _withResultState = ControllerWithResultState.Failed;
            _resultSource.TrySetException(exception);
        }

        private void Cancel()
        {
            _withResultState = ControllerWithResultState.Failed;
            _resultSource.TrySetCanceled();
        }

        public override string ToString()
        {
            switch (_withResultState)
            {
                case ControllerWithResultState.WaitFlowAsync:
                case ControllerWithResultState.WaitForResultAsync:
                    return $"{ControllerName} : {_withResultState}";
                default:
                    return base.ToString();
            }
        }
    }
}
