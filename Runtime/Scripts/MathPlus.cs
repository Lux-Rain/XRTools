using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.TeoDiaz.Math
{
    public static class MathPlus
    {
        public static Vector3 MultiplyVector3D(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.x * vector2.x, vector1.y * vector2.y, vector1.z * vector2.z);
        }

        public static Vector3 MultiplyVector3D(Vector2 vector1, Vector2 vector2)
        {
            return new Vector3(vector1.x * vector2.x, vector1.y * vector2.y);
        }

        public static Vector2 MultiplyVector2D(Vector2 vector1, Vector2 vector2)
        {
            return new Vector3(vector1.x * vector2.x, vector1.y * vector2.y);
        }

        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            Vector3 mid = Vector3.Lerp(start, end, t);
            return new Vector3(mid.x, f(t, height) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
        {
            Vector2 mid = Vector2.Lerp(start, end, t);
            return new Vector2(mid.x, f(t, height) + Mathf.Lerp(start.y, end.y, t));
        }

        private static float f(float x, float height) => (-4 * height * x * x) + (4 * height * x);

    }
}
