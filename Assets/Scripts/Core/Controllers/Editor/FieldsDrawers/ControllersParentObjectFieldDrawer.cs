using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public class ControllersParentObjectFieldDrawer : ControllersBaseFieldDrawer
    {
        private List<ControllersBaseFieldDrawer> _staticChildren;
        private List<ControllersBaseFieldDrawer> _simpleChildren;
        private object _childTarget;
        private string _value;

        public ControllersParentObjectFieldDrawer(
            FieldInfo fieldInfo,
            object target)
            : base(
                fieldInfo,
                target)
        {
        }

        protected override void OnDraw()
        {
            using (new GUILayout.HorizontalScope())
            {
                _value = FieldInfo.GetValue(Target)?.ToString() ?? NULL_VALUE;

                GUILayout.Label(FieldInfo.Name);
                GUILayout.FlexibleSpace();
                GUILayout.Label(_value);

                using (new EditorGUI.DisabledScope(_value == NULL_VALUE))
                {
                    if (_simpleChildren == null && _staticChildren == null)
                    {
                        if (GUILayout.Button("+"))
                        {
                            InitializeChildren();
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("-"))
                        {
                            RemoveChildren();
                        }
                    }
                }
            }

            TryDrawChildrenFields();
        }

        private void TryDrawChildrenFields()
        {
            if (_staticChildren != null)
            {
                DrawFieldsPart(_staticChildren, "Static or Const fields", Color.red);
            }

            if (_simpleChildren != null)
            {
                DrawFieldsPart(_simpleChildren, "Simple fields", Color.yellow);
            }
        }

        private void DrawFieldsPart(
            List<ControllersBaseFieldDrawer> drawers,
            string label,
            Color color)
        {
            var backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            using (new GUILayout.VerticalScope(GUI.skin.box))
            {
                GUI.backgroundColor = backgroundColor;

                GUILayout.Label(label);
                if (drawers.Count > 0)
                {
                    drawers.ForEach(f => f.Draw());
                }
                else
                {
                    EditorGUILayout.HelpBox("Fields not found", MessageType.None);
                }
            }
        }

        private void InitializeChildren()
        {
            _childTarget = FieldInfo.GetValue(Target);
            var childInfos = ControllersFieldsFactory.GetFields(_childTarget);
            if (childInfos != null)
            {
                _simpleChildren = new List<ControllersBaseFieldDrawer>();
                _staticChildren = new List<ControllersBaseFieldDrawer>();
                foreach (var item in childInfos)
                {
                    var fieldInfo = ControllersFieldsFactory.CreateFieldDrawer(item, _childTarget);
                    if (fieldInfo != null)
                    {
                        if (item.IsStatic)
                        {
                            _staticChildren.Add(fieldInfo);
                        }
                        else
                        {
                            _simpleChildren.Add(fieldInfo);
                        }
                    }
                }
            }
        }

        private void RemoveChildren()
        {
            _childTarget = null;
            _simpleChildren = null;
            _staticChildren = null;
        }
    }
}
