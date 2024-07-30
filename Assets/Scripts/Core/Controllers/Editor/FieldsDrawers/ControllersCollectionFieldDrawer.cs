using System.Collections;
using System.Reflection;
using UnityEngine;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public class ControllersCollectionFieldDrawer : ControllersBaseFieldDrawer
    {
        private bool _foldout;

        public ControllersCollectionFieldDrawer(
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
                GUILayout.Label(FieldInfo.GetValue(Target)?.ToString() ?? NULL_VALUE);

                if (GUILayout.Button(_foldout ? "-" : "+"))
                {
                    _foldout = !_foldout;
                }
            }

            if (_foldout && FieldInfo.GetValue(Target) is ICollection collection)
            {
                using (new GUILayout.VerticalScope())
                {
                    var index = 0;
                    foreach (var value in collection)
                    {
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.Label(index.ToString("D3"));
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(value?.ToString() ?? NULL_VALUE);
                            ++index;
                        }
                    }
                }
            }
        }
    }
}
