using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace Shiro
{
    class Bound3
    {
        public Vector3f pMin, pMax;
        public Bound3(Vector3f p) { pMin = pMax = p; }
        public Bound3(Vector3f p1, Vector3f p2)
        {
            pMin = new Vector3f(Min(p1.x, p2.x), Min(p1.y, p2.y), Min(p1.z, p2.z));
            pMax = new Vector3f(Max(p1.x, p2.x), Max(p1.y, p2.y), Max(p1.z, p2.z));
        }
        public Vector3f Diagonal { get { return pMax - pMin; } }
        public int MaxElement
        {
            get
            {
                Vector3f d = Diagonal;
                if (d.x > d.y && d.x > d.z)
                    return 0;
                else if (d.y > d.z)
                    return 1;
                else
                    return 2;
            }
        }
        public double SurfaceArea
        {
            get
            {
                Vector3f d = Diagonal;
                return 2 * (d.x * d.y + d.x * d.z + d.y * d.z);
            }
        }
        public Vector3f Centroid { get { return 0.5f * pMin + 0.5f * pMax; } }
        public Bound3 Intersect(Bound3 b)
        {
            return new Bound3(
                     new Vector3f(Max(pMin.x, b.pMin.x), Max(pMin.y, b.pMin.y), Max(pMin.z, b.pMin.z)),
                     new Vector3f(Min(pMax.x, b.pMax.x), Min(pMax.y, b.pMax.y), Min(pMax.z, b.pMax.z))
                );
        }
        public Vector3f Offset(Vector3f p)
        {
            Vector3f o = p - pMin;
            if (pMax.x > pMin.x)
                o.x /= pMax.x - pMin.x;
            if (pMax.y > pMin.y)
                o.y /= pMax.y - pMin.y;
            if (pMax.z > pMin.z)
                o.z /= pMax.z - pMin.z;
            return o;
        }
        public static bool Overlaps(Bound3 b1, Bound3 b2)
        {
            bool x = (b1.pMax.x >= b2.pMin.x) && (b1.pMin.x <= b2.pMax.x);
            bool y = (b1.pMax.y >= b2.pMin.y) && (b1.pMin.y <= b2.pMax.y);
            bool z = (b1.pMax.z >= b2.pMin.z) && (b1.pMin.z <= b2.pMax.z);
            return (x && y && z);
        }
        public static bool Inside(Vector3f p, Bound3 b)
        {
            return (p.x >= b.pMin.x && p.x <= b.pMax.x && p.y >= b.pMin.y &&
               p.y <= b.pMax.y && p.z >= b.pMin.z && p.z <= b.pMax.z);
        }
        public Vector3f this[int index]
        {
            get { return (index == 0) ? pMin : pMax; }
            set { if (index == 0) pMin = value; else pMax = value; }
        }
        public bool IntersectP(Ray ray)
        {
            float tEnter = float.NegativeInfinity;
            float tExit = float.PositiveInfinity;
            for (int i = 0; i < 3; i++)
            {
                float min = (pMin[i] - ray.origin[i]) * ray.direction_inv[i];
                float max = (pMax[i] - ray.origin[i]) * ray.direction_inv[i];
                if (ray.direction[i] < 0) { float t = min; min = max; max = t; }
                tEnter = Max(min, tEnter);
                tExit = Min(max, tExit);
            }
            return tEnter < tExit && tExit >= 0;
        }
        public static Bound3 Union(Bound3 b1, Bound3 b2) => new Bound3(Vector3f.Min(b1.pMin, b2.pMin), Vector3f.Max(b1.pMax, b2.pMax));
        public static Bound3 Union(Bound3 b1, Vector3f p) => new Bound3(Vector3f.Min(b1.pMin, p), Vector3f.Max(b1.pMax, p));

    }
}
