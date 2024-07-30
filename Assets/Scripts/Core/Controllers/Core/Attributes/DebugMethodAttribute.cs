using System;
using UnityEngine.Scripting;

namespace Core.Controllers.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class DebugMethodAttribute :
#if UNITY_EDITOR || DEVELOPMENT_BUILD || UNITY_BUILDTYPE_DEV
        PreserveAttribute
#else
        Attribute
#endif
    {
        public readonly string Name;
        public DebugMethodAttribute()
        {
        }

        public DebugMethodAttribute(string name)
        {
            Name = name;
        }
    }
}
