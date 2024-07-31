using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public class ControllersUnityObjectFieldDrawer: ControllersBaseFieldDrawer
    {
        public ControllersUnityObjectFieldDrawer(
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
                var obj = FieldInfo.GetValue(Target);
                if (obj == null)
                {
                    GUILayout.Label(NULL_VALUE);
                }
                else
                {
                    EditorGUILayout.ObjectField((Object) obj, typeof(Object), true);
                }
            }
        }
    }
}
