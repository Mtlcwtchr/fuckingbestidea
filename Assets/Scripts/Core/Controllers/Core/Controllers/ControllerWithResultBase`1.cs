using Core.Controllers.Core.DTO;
using Core.Controllers.Core.Interfaces;

namespace Core.Controllers.Core.Controllers
{
    public abstract class ControllerWithResultBase<TResult> : ControllerWithResultBase<EmptyControllerArg, TResult>, IControllerWithResult<TResult>
    {
        protected ControllerWithResultBase(IControllerFactory controllerFactory)
            : base(controllerFactory)
        {
        }
    }
}