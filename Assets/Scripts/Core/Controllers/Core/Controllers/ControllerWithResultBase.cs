using Core.Controllers.Core.DTO;
using Core.Controllers.Core.Interfaces;

namespace Core.Controllers.Core.Controllers
{
    public abstract class ControllerWithResultBase : ControllerWithResultBase<EmptyControllerArg, EmptyControllerResult>, IControllerWithResult
    {
        protected ControllerWithResultBase(IControllerFactory controllerFactory)
            : base(controllerFactory)
        {
        }

        /// <summary>
        /// Complete controller with default result.
        /// </summary>
        protected void Complete()
        {
            Complete(default);
        }
    }
}
