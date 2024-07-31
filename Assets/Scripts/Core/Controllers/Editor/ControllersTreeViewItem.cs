using System;
using Core.Controllers.Core.Interfaces;
using UnityEditor.IMGUI.Controls;

namespace Core.Controllers.Editor
{
    internal class ControllersTreeViewItem : TreeViewItem
    {
        private readonly WeakReference _controllerWeakReference;

        internal IControllerDebugInfo ControllerDebugInfo
        {
            get
            {
                if (_controllerWeakReference.IsAlive)
                {
                    return (IControllerDebugInfo)_controllerWeakReference.Target;
                }

                return null;
            }
        }

        internal ControllersTreeViewItem(
            int id,
            int depth,
            IControllerDebugInfo controllerDebugInfo)
            : base(id, depth, controllerDebugInfo.ToString())
        {
            _controllerWeakReference = new WeakReference(controllerDebugInfo);
        }
    }
}
