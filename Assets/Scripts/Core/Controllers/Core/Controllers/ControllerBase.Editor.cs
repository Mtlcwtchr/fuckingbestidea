#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Core.Controllers.Core.Interfaces;
using Core.Controllers.Core.States;
using UnityEngine;

namespace Core.Controllers.Core.Controllers
{
    public partial class ControllerBase : IControllerDebugInfo
    {
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IControllerDebugInfo>)this).GetEnumerator();
        }
 
        IEnumerator<IControllerDebugInfo> IEnumerable<IControllerDebugInfo>.GetEnumerator()
        {
            foreach (var childController in _childControllers)
            {
                yield return (IControllerDebugInfo)childController;
            }
        }

        string IControllerDebugInfo.ControllerType => ControllerTypeInternal();
        string IControllerDebugInfo.StateName => StateNameInternal();
        Color IControllerDebugInfo.StateColor => StateColorInternal();
        string IControllerDebugInfo.ScopeName => _controllerFactory?.ToString();
        CancellationToken IControllerDebugInfo.CancellationToken => _lifetimeToken;

        protected virtual string ControllerTypeInternal() =>
            nameof(global::Core.Controllers.Core.Controllers.ControllerBase);

        protected virtual string StateNameInternal() =>
            _state.ToString();

        protected virtual Color StateColorInternal() =>
            _state switch
            {
                ControllerState.Created => new Color(0.09f, 0.52f, 0.87f),
                ControllerState.Initialized => new Color(0.07f, 0.84f, 0.85f),
                ControllerState.Running => new Color(0.13f, 0.8f, 0f),
                ControllerState.Stopped => new Color(1f, 0.86f, 0f),
                ControllerState.Disposed => new Color(1f, 0.26f, 0.12f),
                _ => Color.magenta
            };
    }
}
#endif
