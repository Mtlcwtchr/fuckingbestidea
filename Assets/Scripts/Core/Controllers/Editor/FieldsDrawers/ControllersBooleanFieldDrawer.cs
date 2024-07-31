using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public class ControllersBooleanFieldDrawer : ControllersBaseFieldDrawer
    {
        public ControllersBooleanFieldDrawer(
            FieldInfo fieldInfo,
            object target)
            : base(fieldInfo, target)
        {
        }

        protected override void OnDraw()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label(FieldInfo.Name);
                GUILayout.FlexibleSpace();
                using (var scope = new EditorGUI.ChangeCheckScope())
                {
                    if (FieldInfo.GetValue(Target) is bool value)
                    {
                        value = GUILayout.Toggle(value, "");
                        if (scope.changed)
                        {
                            FieldInfo.SetValue(Target, value);
                        }
                    }
                }
            }
        }
    }
}
