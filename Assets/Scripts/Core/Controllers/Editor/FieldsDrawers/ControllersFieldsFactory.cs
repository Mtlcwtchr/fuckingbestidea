using System;
using System.Collections;
using System.Reflection;

namespace Core.Controllers.Editor.FieldsDrawers
{
    public class ControllersFieldsFactory
    {
        public static FieldInfo[] GetFields(object target)
        {
            return target.GetType().GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static ControllersBaseFieldDrawer CreateFieldDrawer(FieldInfo fieldInfo, object target)
        {
            if (fieldInfo.IsStatic)
            {
                return new ControllersLabelFieldDrawer(fieldInfo, target);
            }

            var value = fieldInfo.GetValue(target);

            switch (value)
            {
                case string _:
                    return new ControllersStringFieldDrawer(fieldInfo, target);
                case int _:
                    return new ControllersIntegerFieldDrawer(fieldInfo, target);
                case bool _:
                    return new ControllersBooleanFieldDrawer(fieldInfo, target);
                case ICollection _:
                    return new ControllersCollectionFieldDrawer(fieldInfo, target);
                case Action _:
                    return new ControllersActionFieldDrawer(fieldInfo, target);
                case UnityEngine.Object _:
                    return new ControllersUnityObjectFieldDrawer(fieldInfo, target);
            }

            return new ControllersParentObjectFieldDrawer(fieldInfo, target);
        }
    }
}
