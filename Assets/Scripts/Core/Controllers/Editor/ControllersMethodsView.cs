using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.Controllers.Core.Attributes;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor
{
    internal class ControllersMethodsView
    {
        private readonly ControllersTreeViewModel _model;

        private List<MethodInfo> _debugMethods;
        private List<MethodInfo> _simpleMethods;
        private List<MethodInfo> _disabledMethods;

        private Vector2 _scrollPosition;
        private object _invokeObject;

        internal ControllersMethodsView(ControllersTreeViewModel model)
        {
            _model = model;
            _debugMethods = new List<MethodInfo>();
            _simpleMethods = new List<MethodInfo>();
            _disabledMethods = new List<MethodInfo>();
        }

        internal void SelectedController(object invokeObject)
        {
            _debugMethods.Clear();
            _simpleMethods.Clear();
            _disabledMethods.Clear();
            _invokeObject = invokeObject;

            if (_invokeObject == null)
            {
                return;
            }

            var methods = invokeObject.GetType().GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttributes(typeof(DebugMethodAttribute), true).SingleOrDefault();
                if (attribute != null)
                {
                    _debugMethods.Add(method);
                }
                else if (method.GetParameters().Length == 0 && !((IList) _model.IgnoredMethods).Contains(method.Name))
                {
                    _simpleMethods.Add(method);
                }
                else
                {
                    _disabledMethods.Add(method);
                }
            }
        }

        internal void DrawMethods()
        {
            if (_model.IsRightPanelVisible)
            {
                using (new GUILayout.VerticalScope(GUI.skin.box, GUILayout.Width(_model.RightPanelWidth)))
                {
                    using (var scrollScope = new GUILayout.ScrollViewScope(_scrollPosition))
                    {
                        _scrollPosition = scrollScope.scrollPosition;
                        GUILayout.Space(4);
                        DrawMethodsBlock(_debugMethods, "Debug methods:", Color.green);
                        DrawMethodsBlock(_simpleMethods, "Available methods:", Color.yellow);
                        DrawMethodsBlock(_disabledMethods, "Disabled methods:", Color.red, true);
                    }
                }
            }
        }

        private void DrawMethodsBlock(
            List<MethodInfo> methods,
            string label,
            Color color,
            bool disabled = false)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            using (new EditorGUI.DisabledScope(disabled))
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUI.backgroundColor = backgroundColor;

                GUILayout.Label(label);
                if (methods.Count <= 0)
                {
                    EditorGUILayout.HelpBox("Methods not found", MessageType.None);
                    return;
                }

                foreach (var method in methods)
                {
                    if (GUILayout.Button(method.Name))
                    {
                        method.Invoke(_invokeObject, null);
                    }
                }
            }
        }
    }
}
