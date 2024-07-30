using System;
using System.Reflection;
using UnityEngine;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public class ControllersActionFieldDrawer : ControllersBaseFieldDrawer
    {
        public ControllersActionFieldDrawer(
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
                if (GUILayout.Button("Invoke"))
                {
                    if (FieldInfo.GetValue(Target) is Action action)
                    {
                        action?.Invoke();
                    }
                }
            }
        }
    }
}
