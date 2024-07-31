using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public class ControllersIntegerFieldDrawer : ControllersBaseFieldDrawer
    {
        public ControllersIntegerFieldDrawer(
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
                    var value = FieldInfo.GetValue(Target) is int
                                                            ? (int) FieldInfo.GetValue(Target)
                                                            : 0;
                    value = EditorGUILayout.IntField(value);
                    if (scope.changed)
                    {
                        FieldInfo.SetValue(Target, value);
                    }
                }
            }
        }
    }
}
