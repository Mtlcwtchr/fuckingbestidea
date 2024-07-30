using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Core.Utilities
{
    public static class MathUtils
    {
        private static readonly float[] Factorial =
        {
            1.0f,
            1.0f,
            2.0f,
            6.0f,
            24.0f,
            120.0f,
            720.0f,
            5040.0f,
            40320.0f,
            362880.0f,
            3628800.0f,
            39916800.0f,
            479001600.0f,
            6227020800.0f,
            87178291200.0f,
            1307674368000.0f,
            20922789888000.0f,
        };

        private const int MAX_BEZIER_CP = 16;

        public static int Range(int min, int max, int seed)
        {
            var old = UnityEngine.Random.state;
            UnityEngine.Random.InitState(seed);
            var value = UnityEngine.Random.Range(min, max);
            UnityEngine.Random.state = old;
            return value;
        }
        
        public static bool LineIntersection(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2, out Vector2 intersection)
        {
            intersection = new Vector2();

            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.x * s.y - r.y * s.x;

            if (Mathf.Abs(rxs) <= float.Epsilon)
                return false;

            var qpxs = (q.x - p.x) * s.y - (q.y - p.y) * s.x;

            var t = qpxs / rxs;

            intersection = p + t * r;
            return true;
        }
        
        public static void Bezier(List<Vector2> controlPoints, int pointsCount)
        {
            int n = controlPoints.Count;
            
            Span<Vector2> points = stackalloc Vector2[n];
            for (var i = 0; i < n - 1 && i < MAX_BEZIER_CP; ++i)
            {
                points[i] = controlPoints.PopFront();
            }
            points[n - 1] = controlPoints.PopBack();
            controlPoints.Clear();
            
            for (int t = 0; t <= pointsCount; ++t)
            {
                Vector2 p = new Vector2();
                for (int i = 0; i < n; ++i)
                {
                    Vector2 bn = Bernstein(n, i, t/(float)pointsCount) * points[i];
                    p += bn;
                }
                controlPoints.Add(p);
            }
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Bernstein(int n, int i, float t)
        {
            float t_i = Mathf.Pow(t, i);
            float t_n_minus_i = Mathf.Pow((1 - t), (n - i));

            float basis = Binomial(n, i) * t_i * t_n_minus_i;
            return basis;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Binomial(int n, int i)
        {
            float ni;
            float a1 = Factorial[n];
            float a2 = Factorial[i];
            float a3 = Factorial[n - i];
            ni = a1 / (a2 * a3);
            return ni;
        }
    }
}