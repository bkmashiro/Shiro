using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shiro
{
    class Triangle : Object
    {
        Vector3f v0, v1, v2; // vertices A, B ,C , counter-clockwise order
        Vector3f e1, e2;     // 2 edges v1-v0, v2-v0;
        Vector3f? t0, t1, t2; // texture coords
        Vector3f normal;
        float area;
        Material m;

        public Triangle(Vector3f v0, Vector3f v1, Vector3f v2, Material m)
        {
            this.v0 = v0;
            this.v1 = v1;
            this.v2 = v2;
            this.m = m;
            e1 = v1 - v0;
            e2 = v2 - v0;
            normal = Normalize(CrossProduct(e1, e2));
            area = CrossProduct(e1, e2).Norm * 0.5f;
        }


        public override Vector3f? evalDiffuseColor(Vector2f v)
        {
            return new(0.5f);
        }

        public override float getArea()
        {
            return area;
        }

        public override Bound3? getBounds()
        {
            return Bound3.Union(new(v0, v1), v2);
        }

        public override Intersection getIntersection(Ray ray)
        {
            Intersection inter = new Intersection();

            if (DotProduct(ray.direction, normal) > 0)
                return inter;
            double u, v, t_tmp = 0;
            Vector3f pvec = CrossProduct(ray.direction, e2);
            double det = DotProduct(e1, pvec);
            if (Abs(det) < EPSILON)
                return inter;

            double det_inv = 1F / det;
            Vector3f tvec = ray.origin - v0;
            u = DotProduct(tvec, pvec) * det_inv;
            if (u < 0 || u > 1)
                return inter;
            Vector3f qvec = CrossProduct(tvec, e1);
            v = DotProduct(ray.direction, qvec) * det_inv;
            if (v < 0 || u + v > 1)
                return inter;
            t_tmp = DotProduct(e2, qvec) * det_inv;

            if (t_tmp < 0)
            {
                return inter;
            }

            inter.distance = t_tmp;
            inter.coords = ray[(float)t_tmp];
            inter.happened = true;
            inter.m = m;
            inter.normal = normal;
            inter.obj = this;

            return inter;
        }

        public override void getSurfaceProperties(Vector3f P, Vector3f I, int index, Vector2f uv, Vector3f N, Vector2f st)
        {
            N = normal;
            throw new NotImplementedException();
        }

        public override bool hasEmit()
        {
            return m.HasEmission();
        }

        public override bool intersect(Ray ray)
        {
            throw new NotImplementedException();
        }

        public override bool intersect(Ray ray, float tnear, int index)
        {
            throw new NotImplementedException();
        }

        public override void Sample(Intersection pos,out float pdf)
        {
            float x = (float)Sqrt(get_random_float()), y = get_random_float();
            pos.coords = v0 * (1.0f - x) + v1 * (x * (1.0f - y)) + v2 * (x * y);
            pos.normal = this.normal;
            pdf = 1.0f / area;
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
    class MeshTriangle : Object
    {
        
       public MeshTriangle()
        {

        }
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override Vector3f? evalDiffuseColor(Vector2f v)
        {
            return base.evalDiffuseColor(v);
        }

        public override float getArea()
        {
            return base.getArea();
        }

        public override Bound3? getBounds()
        {
            return base.getBounds();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override Intersection getIntersection(Ray _ray)
        {
            return base.getIntersection(_ray);
        }

        public override void getSurfaceProperties(Vector3f P, Vector3f I, int index, Vector2f uv, Vector3f N, Vector2f st)
        {
            base.getSurfaceProperties(P, I, index, uv, N, st);
        }

        public override bool hasEmit()
        {
            return base.hasEmit();
        }

        public override bool intersect(Ray ray)
        {
            return base.intersect(ray);
        }

        public override bool intersect(Ray ray, float tnear, int index)
        {
            return base.intersect(ray, tnear, index);
        }

        public override void Sample(Intersection pos, out float pdf)
        {
            base.Sample(pos, out pdf);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }

    static class Triangles
    {
        public static bool rayTriangleIntersect(Vector3f v0, Vector3f v1,
                              Vector3f v2, Vector3f orig,
                              Vector3f dir, out float tnear, out float u, out float v)
        {
            tnear = 0.0f;
            u = 0.0f;
            v = 0.0f;
            Vector3f edge1 = v1 - v0;
            Vector3f edge2 = v2 - v0;
            Vector3f pvec = CrossProduct(dir, edge2);
            float det = DotProduct(edge1, pvec);
            if (det == 0 || det < 0)
                return false;

            Vector3f tvec = orig - v0;
            u = DotProduct(tvec, pvec);
            if (u < 0 || u > det)
                return false;

            Vector3f qvec = CrossProduct(tvec, edge1);
            v = DotProduct(dir, qvec);
            if (v < 0 || u + v > det)
                return false;

            float invDet = 1 / det;

            tnear = DotProduct(edge2, qvec) * invDet;
            u *= invDet;
            v *= invDet;

            return true;
        }


    }
}
