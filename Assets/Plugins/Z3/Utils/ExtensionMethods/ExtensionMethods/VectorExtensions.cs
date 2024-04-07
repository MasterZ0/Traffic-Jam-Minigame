using UnityEngine;
using Random = UnityEngine.Random;

namespace Z3.Utils.ExtensionMethods
{

    public static class VectorExtensions
    {
        public static int RandomRange(this Vector2Int range)
        {
            return Random.Range(range.x, range.y);
        }

        public static float RandomRange(this Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }

        public static float HalfTerm(this Vector2 range)
        {
            return range.x + (range.y - range.x) / 2f;
        }

        public static Vector3 Multiply(this Vector3 a, Vector3 b)
        {
            return new Vector3()
            {
                x = a.x * b.x,
                y = a.y * b.y,
                z = a.z * b.z,
            };
        }

        public static int RandomRangeAround(this Vector2 range)
        {
            float value = Random.Range(range.x, range.y);
            return Mathf.RoundToInt(value);
        }

        public static Vector2Int Abs(this Vector2Int original)
        {
            return Vector2Int.RoundToInt(((Vector2)original).Abs());
        }

        public static Vector2 Abs(this Vector2 original)
        {
            original.x = Mathf.Abs(original.x);
            original.y = Mathf.Abs(original.y);
            return original;
        }

        public static bool IsOutRange(this Vector2 range, int value)
        {
            return value < range.x || value > range.y;
        }

        public static bool IsOutRange(this Vector2 range, float value)
        {
            return value < range.x || value > range.y;
        }

        public static bool InsideRange(this Vector2 range, float value)
        {
            return !range.IsOutRange(value);
        }

        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0f);
        }

        public static Vector2 InvertXY(this Vector2 range)
        {
            float aux = range.x;
            range.x = range.y;
            range.y = aux;
            return range;
        }
    }
}