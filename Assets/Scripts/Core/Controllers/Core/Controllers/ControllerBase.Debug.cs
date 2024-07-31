using System.Collections.Generic;
using Core.Controllers.Core.Interfaces;
using UnityEngine.Pool;

namespace Core.Controllers.Core.Controllers
{
    public partial class ControllerBase
    {
        /// <summary>
        /// Generates a string representation of the controller tree.
        /// Used for debug or error handling purposes.
        /// </summary>
        /// <returns>A string representation of the controller tree.</returns>
        protected string Dump()
        {
            using var controllersTreePooledObject = ListPool<string>.Get(out var controllersTree);
            ((IController)this).ScanTree(controllersTree, string.Empty);
            return string.Join("\n", controllersTree);
        }

        void IController.ScanTree(List<string> controllersTree, string prefix)
        {
            controllersTree.Add($"{prefix}{this}");
            foreach (var child in _childControllers)
            {
                child.ScanTree(controllersTree, $"  {prefix}");
            }
        }
    }
}
