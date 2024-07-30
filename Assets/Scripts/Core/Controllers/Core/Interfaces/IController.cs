using System;
using System.Collections.Generic;
using System.Threading;

namespace Core.Controllers.Core.Interfaces
{
    public interface IController : IDisposable
    {
        internal void Initialize(
            CancellationToken externalCancellationToken,
            CancellationToken parentCancellationToken);
        internal void Start();
        internal void Stop();
        internal void Stop(Exception rootCauseException);
        internal void ScanTree(List<string> controllersTree, string prefix);
    }

    public interface IController<in TArg>
    {
        internal void SetArgInternal(TArg arg);
    }
}
