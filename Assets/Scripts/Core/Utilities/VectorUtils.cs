using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace Core.Utilities
{
    public static class VectorUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ToV3(this Vector2 obj, float y = 0)
        {
            return new Vector3(obj.x, y, obj.y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToV2(this Vector3 obj)
        {
            return new Vector2(obj.x, obj.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSq(this Vector2 obj, Vector2 other)
        {
            return math.distancesq(obj, other);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSq(this Vector3 obj, Vector3 other)
        {
            return math.distancesq(obj.ToV2(), other.ToV2());
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Right(this Vector2 obj)
        {
            return new Vector2(obj.y, -obj.x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Left(this Vector2 obj)
        {
            return new Vector2(-obj.y, obj.x);
        }
    }
}