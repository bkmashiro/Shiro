using static System.Math;
namespace Shiro
{
    class Vector3f
    {
        /// <summary>
        /// 存储向量内容，不建议直接直接使用；用方法存取
        /// </summary>
        public float[] val = { 0f, 0f, 0f };
        public Vector3f() { x = y = z = 0f; }
        public Vector3f(float v) { x = y = z = v; }
        public Vector3f(float xx, float yy, float zz) { x = xx; y = yy; z = zz; }
        public static Vector3f operator *(Vector3f v, float f) { return new Vector3f(v.x * f, v.y * f, v.z * f); }
        public static Vector3f operator /(Vector3f v, float f) { return new Vector3f(v.x / f, v.y / f, v.z / f); }
        public static Vector3f operator *(float f, Vector3f v) { return new Vector3f(v.x * f, v.y * f, v.z * f); }
        public static Vector3f operator /(float f, Vector3f v) { return new Vector3f(v.x / f, v.y / f, v.z / f); }
        /// <summary>
        /// 2-范数（模长）
        /// </summary>
        public float Norm => (float)Sqrt((x * x) + (y * y) + (z * z));
        /// <summary>
        /// 归一化（转换为该方向上的单位向量）
        /// </summary>
        public Vector3f Normalixzed => this / Norm;
        /// <summary>
        /// 向量点乘
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3f operator *(Vector3f v1, Vector3f v2) { return new Vector3f(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z); }
        public static Vector3f operator -(Vector3f v1, Vector3f v2) { return new Vector3f(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z); }
        public static Vector3f operator +(Vector3f v1, Vector3f v2) { return new Vector3f(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z); }
        public static Vector3f operator -(Vector3f v1) { return new Vector3f(-v1.x, -v1.y, -v1.z); }
        /// <summary>
        /// 向量叉乘
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector3f operator ^(Vector3f a,Vector3f b)
        {
            return new Vector3f(
                (a.y * b.z) - (a.z * b.y),
                (a.z * b.x) - (a.x * b.z),
                (a.x * b.y) - (a.y * b.x)
            );
        }
        public float this[int index] { get { return val[index]; } set { val[index] = value; } }
        public float x { get { return val[0]; } set { val[0] = value; } }
        public float y { get { return val[1]; } set { val[1] = value; } }
        public float z { get { return val[2]; } set { val[2] = value; } }
        public static Vector3f Min(Vector3f p1, Vector3f p2) => new Vector3f(Math.Min(p1.x, p2.x), Math.Min(p1.y, p2.y), Math.Min(p1.z, p2.z));
        public static Vector3f Max(Vector3f p1, Vector3f p2) => new Vector3f(Math.Max(p1.x, p2.x), Math.Max(p1.y, p2.y), Math.Max(p1.z, p2.z));
        public static readonly Vector3f ZERO = new Vector3f(0,0,0);


    }
    class Vector2f
    {
        public float[] val = { 0f, 0f };
        public Vector2f() { x = y = 0; }
        public Vector2f(float v) { x = y = v; }
        public Vector2f(float xx, float yy) { x = xx; y = yy; }
        public static Vector2f operator *(Vector2f v, float r) => new Vector2f(v.x * r, v.y * r);
        public static Vector2f operator +(Vector2f v, Vector2f b) => new Vector2f(v.x + b.x, v.y + b.y);

        public float x { get { return val[0]; } set { val[0] = value; } }
        public float y { get { return val[1]; } set { val[1] = value; } }

    }
    static class Vectors
    {
        public static float DotProduct(Vector3f a, Vector3f b) => (a.x * b.x) + (a.y * b.y) + (a.z * b.z);

        public static Vector3f CrossProduct(Vector3f a, Vector3f b)
        {
            return new Vector3f(
                (a.y * b.z) - (a.z * b.y),
                (a.z * b.x) - (a.x * b.z),
                (a.x * b.y) - (a.y * b.x)
            );
        }
        /// <summary>
        /// 线性插值
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3f Lerp(Vector3f a, Vector3f b, float t)
        {
            return a * (1 - t) + b * t;
        }
        public static Vector3f Normalize(Vector3f v)
        {
            float mag2 = v.x * v.x + v.y * v.y + v.z * v.z;
            if (mag2 > 0)
            {
                float invMag = (float)(1f / Sqrt(mag2));
                return new Vector3f(v.x * invMag, v.y * invMag, v.z * invMag);
            }
            return v;
        }

    }
}