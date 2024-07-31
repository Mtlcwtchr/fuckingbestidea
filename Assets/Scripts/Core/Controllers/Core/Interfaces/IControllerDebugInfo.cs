#if UNITY_EDITOR
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Core.Controllers.Core.Interfaces
{
    public interface IControllerDebugInfo : IEnumerable<IControllerDebugInfo>
    {
        string ControllerType { get; }
        string StateName { get; }
        Color StateColor { get; }
        string ScopeName { get; }
        CancellationToken CancellationToken { get; }
    }
}
#endif
