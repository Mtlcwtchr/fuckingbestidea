#if UNITY_EDITOR
using Core.Controllers.Core.Interfaces;
using Core.Controllers.Core.States;
using UnityEngine;

namespace Core.Controllers.Core.Controllers
{
    public partial class ControllerWithResultBase<TArg, TResult> : ControllerBase<TArg>, IControllerDebugInfo
    {
        protected override string ControllerTypeInternal() =>
            nameof(ControllerWithResultBase);

        protected override string StateNameInternal() =>
            _withResultState switch
            {
                ControllerWithResultState.WaitFlowAsync => _withResultState.ToString(),
                ControllerWithResultState.WaitForResultAsync => _withResultState.ToString(),
                _ => base.StateNameInternal()
            };

        protected override Color StateColorInternal() =>
            _withResultState switch
            {
                ControllerWithResultState.None => Color.white,
                ControllerWithResultState.WaitFlowAsync => new Color(1.0f, 0.647f, 0.0f, 1.0f),
                ControllerWithResultState.WaitForResultAsync => new Color(0.25f, 0.65f, 0.75f, 1.0f),
                ControllerWithResultState.Completed => new Color(0.13f, 0.9f, 0f),
                ControllerWithResultState.Failed => Color.red,
                _ => Color.magenta
            };
    }
}
#endif