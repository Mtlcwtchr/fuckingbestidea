using System.Threading;
using Core.Controllers.Core.DTO;
using Cysharp.Threading.Tasks;

namespace Core.Controllers.Core.Interfaces
{
    public interface IControllerWithResult : IControllerWithResult<EmptyControllerResult>
    {
    }

    public interface IControllerWithResult<TResult> : IController
    {
        internal UniTask FlowAsync(CancellationToken cancellationToken);
        internal UniTask<TResult> GetResult(CancellationToken token);
    }
}
