using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public class ControllersStringFieldDrawer : ControllersBaseFieldDrawer
    {
        public ControllersStringFieldDrawer(
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
                    var text = GUILayout.TextArea(FieldInfo.GetValue(Target)?.ToString() ?? NULL_VALUE);
                    if (scope.changed)
                    {
                        FieldInfo.SetValue(Target, text);
                    }
                }
            }
        }
    }
}
