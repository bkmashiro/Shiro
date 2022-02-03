using System.Diagnostics;
using static Shiro.Triangles;
namespace Shiro
{
    class Triangle : Object
    {
        public Vector3f v0, v1, v2; // vertices A, B ,C , counter-clockwise order
        public Vector3f e1, e2;     // 2 edges v1-v0, v2-v0;
        public Vector2f? t0, t1, t2; // texture coords
        public Vector3f normal;
        public float area;
        public Material? m;
        int numTriangles;
        Bound3 bounding_box;
        Vector3f[] vertices;
        int[] vertexIndex;
        List<Vector2f[]> stCoordinates;
        List<Triangle> triangles;

        BVHAccel bvh;
        public Triangle(Vector3f v0, Vector3f v1, Vector3f v2, Material? m = null)
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

        public override void getSurfaceProperties(Vector3f P, Vector3f I, int index, Vector2f uv,out Vector3f N,out Vector2f st)
        {
            N = normal;
            throw new NotImplementedException();
        }

        public override bool hasEmit()
        {
            return m?.HasEmission() ?? false;
        }

        public override bool intersect(Ray ray)
        {
            return true;
        }

        public override bool intersect(ref Ray ray, ref float tnear, ref int index)
        {
            return false;
        }

        public override void Sample(Intersection pos, out float pdf)
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
        Bound3 bounding_box;
        Vector3f[] vertices;
        int numTriangles;
        int[] vertexIndex;
        Vector2f[] stCoordinates;

        List<Triangle> triangles = new List<Triangle>();

        BVHAccel bvh;
        float area;

        Material m;
        public MeshTriangle(string fileName, Material mt)
        {
            OBJReader loader = new(fileName);
            area = 0;
            m = mt;
            Debug.Assert(loader.LoadedMeshes == 1);
            var mesh = loader.mesh;

            Vector3f min_vert = new(float.PositiveInfinity);
            Vector3f max_vert = new(float.NegativeInfinity);
            for (int i = 0; i < mesh.Vertex.Count; i += 3)
            {
                var face_vertices = new Vector3f[3];

                for (int j = 0; j < 3; j++)
                {
                    Vector3f vert = new(mesh.Vertex[i + j].x,
                                         mesh.Vertex[i + j].y,
                                         mesh.Vertex[i + j].z);
                    face_vertices[j] = vert;

                    min_vert = new(Min(min_vert.x, vert.x),
                                        Min(min_vert.y, vert.y),
                                        Min(min_vert.z, vert.z));
                    max_vert = new(Max(max_vert.x, vert.x),
                                        Max(max_vert.y, vert.y),
                                        Max(max_vert.z, vert.z));
                }

                triangles.Add(new(face_vertices[0], face_vertices[1], face_vertices[2], mt));
            }

            bounding_box = new(min_vert, max_vert);

            List<Object> ptrs = new();
            foreach (var tri in triangles)
            {
                ptrs.Add(tri);
                area += tri.area;
            }
            bvh = new BVHAccel(ptrs);
        }
        public override Vector3f? evalDiffuseColor(Vector2f st)
        {
            //需要测试^号用途（在cpp文件中）
            float scale = 5;
            float pattern =
                (((st.x * scale) % 1) > 0.5) ^ (((st.y * scale) % 1) > 0.5) ? 0f : 1f;
            return Lerp(new(0.815f, 0.235f, 0.031f),
                        new(0.937f, 0.937f, 0.231f), pattern);
        }

        public override float getArea()
        {
            return area;
        }

        public override Bound3? getBounds()
        {
            return bounding_box;
        }

        public override Intersection getIntersection(Ray ray)
        {
            Intersection intersec=new();

            if (bvh!=null)
            {
                intersec = bvh.Intersect(ray);
            }

            return intersec;
        }

        public override void getSurfaceProperties(Vector3f P, Vector3f I, int index, Vector2f uv,out Vector3f N,out Vector2f st)
        {
            Vector3f v0 = vertices[vertexIndex[index * 3]];
            Vector3f v1 = vertices[vertexIndex[index * 3 + 1]];
            Vector3f v2 = vertices[vertexIndex[index * 3 + 2]];
            Vector3f e0 = Normalize(v1 - v0);
            Vector3f e1 = Normalize(v2 - v1);
            N = Normalize(CrossProduct(e0, e1));
            Vector2f st0 = stCoordinates[vertexIndex[index * 3]];
            Vector2f st1 = stCoordinates[vertexIndex[index * 3 + 1]];
            Vector2f st2 = stCoordinates[vertexIndex[index * 3 + 2]];
            st = st0 * (1 - uv.x - uv.y) + st1 * uv.x + st2 * uv.y;
        }

        public override bool hasEmit()
        {
            return m.HasEmission();
        }

        public override bool intersect(Ray ray)
        {
            return true;
            throw new NotImplementedException();
        }

        public override bool intersect(ref Ray ray, ref float tnear, ref int index)
        {
            bool intersect = false;
            for (int k = 0; k < numTriangles; ++k)
            {
                Vector3f v0 = vertices[vertexIndex[k * 3]];
                Vector3f v1 = vertices[vertexIndex[k * 3 + 1]];
                Vector3f v2 = vertices[vertexIndex[k * 3 + 2]];
                float t, u, v;
                if (RayTriangleIntersect(v0, v1, v2, ray.origin, ray.direction, out t,
                                        out u, out v) &&
                    t < tnear)
                {
                    tnear = t;
                    index = k;
                    intersect |= true;
                }
            }

            return intersect;
        }

        public override void Sample(Intersection pos, out float pdf)
        {
            bvh.Sample(pos,out pdf);
            pos.emit = m.getEmission();
        }
    }

    static class Triangles
    {
        public static bool RayTriangleIntersect(Vector3f v0, Vector3f v1,
                           Vector3f v2, Vector3f orig,
                           Vector3f dir, out float tnear, out float u, out float v)
        {
            tnear = float.NaN;
            u = float.NaN;
            v = float.NaN;
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
