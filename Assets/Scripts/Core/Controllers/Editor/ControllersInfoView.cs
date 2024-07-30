using System.Collections.Generic;
using Core.Controllers.Editor.FieldsDrawers;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor
{
    internal class ControllersInfoView
    {
        private readonly ControllersTreeViewModel _model;

        private List<ControllersBaseFieldDrawer> _staticFields;
        private List<ControllersBaseFieldDrawer> _otherFields;
        private object _target;
        private Vector2 _scrollPosition;

        internal ControllersInfoView(ControllersTreeViewModel model)
        {
            _model = model;
        }

        internal void SelectedController(object target)
        {
            _target = target;
            if (target == null)
            {
                _staticFields = null;
                _otherFields = null;
                return;
            }

            _staticFields = new List<ControllersBaseFieldDrawer>();
            _otherFields = new List<ControllersBaseFieldDrawer>();

            var fieldsInfo = ControllersFieldsFactory.GetFields(target);
            foreach (var info in fieldsInfo)
            {
                if (info.IsStatic)
                {
                    _staticFields.Add(ControllersFieldsFactory.CreateFieldDrawer(info, _target));
                }
                else
                {
                    _otherFields.Add(ControllersFieldsFactory.CreateFieldDrawer(info, _target));
                }
            }
        }

        internal void DrawInfos()
        {
            if (!_model.IsBottomPanelVisible)
            {
                return;
            }

            using var verticalScope = new GUILayout.VerticalScope(GUI.skin.box, GUILayout.Height(_model.BottomPanelHeight));
            using var scrollScope = new GUILayout.ScrollViewScope(_scrollPosition);
            _scrollPosition = scrollScope.scrollPosition;
            GUILayout.Space(4);
            if (_target != null)
            {
                TryDrawFieldsPart(_staticFields, "Static or Const fields", Color.red, true);
                TryDrawFieldsPart(_otherFields, "Simple fields", Color.yellow);
            }
            else
            {
                EditorGUILayout.HelpBox("Controller not selected", MessageType.None);
            }
        }

        private void TryDrawFieldsPart(
            List<ControllersBaseFieldDrawer> drawers,
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
                if (drawers != null && drawers.Count > 0)
                {
                    drawers.ForEach(f => f.Draw());
                }
                else
                {
                    EditorGUILayout.HelpBox("Fields not found", MessageType.None);
                }
            }
        }
    }
}
