using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public abstract class ControllersBaseFieldDrawer
    {
        protected const string NULL_VALUE = "NULL";

        protected readonly FieldInfo FieldInfo;
        protected readonly object Target;

        protected ControllersBaseFieldDrawer(FieldInfo fieldInfo, object target)
        {
            FieldInfo = fieldInfo;
            Target = target;
        }

        public void Draw()
        {
            try
            {
                EditorGUI.indentLevel += 1;
                var backgroundColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.black;
                using (new GUILayout.VerticalScope(GUI.skin.box))
                {
                    GUI.backgroundColor = backgroundColor;
                    OnDraw();
                }
            }
            finally
            {
                EditorGUI.indentLevel -= 1;
            }
        }

        protected abstract void OnDraw();
    }
}
