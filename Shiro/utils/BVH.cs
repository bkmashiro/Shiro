using System.Diagnostics;
using static Shiro.Bound3;
namespace Shiro
{
    enum SplitMethod { NAIVE, SAH };
    class BVHBuildNode
    {
        public Bound3? bounds;
        public BVHBuildNode? left;
        public BVHBuildNode? right;
        public Object? obj;
        public float? area;

        public int splitAxis = 0, firstPrimOffset = 0, nPrimitives = 0;
        // BVHBuildNode Public Methods
        public BVHBuildNode()
        {
            bounds = null;
            left = null; right = null;
            obj = null;
        }

        public override string ToString()
        {
            return $"{bounds}" +
                $"\n  {(left == null ? "NULL" : left)}" +
                $"\n  {(right == null ? "NULL" : right)}";
        }
    }
    class BVHAccel
    {
        public BVHBuildNode? root;
        public BVHAccel(List<Object> p, int maxPrimsInNode = 1, SplitMethod splitMethod = SplitMethod.NAIVE)
        {
            root = recursiveBuild(p);
            Console.WriteLine(root);
        }

        BVHBuildNode recursiveBuild(List<Object> obj)
        {
            BVHBuildNode node = new BVHBuildNode();
            // Compute bounds of all primitives in BVH node
            Bound3 bounds = new();
            for (int i = 0; i < obj.Count; ++i)
                bounds = Union(bounds, obj[i].getBounds()!);
            if (obj.Count == 1)
            {
                // Create leaf _BVHBuildNode_
                node.bounds = obj[0].getBounds();
                node.obj = obj[0];
                node.left = null;
                node.right = null;
                node.area = obj[0].getArea();
                return node;
            }
            else if (obj.Count == 2)
            {
                node.left = recursiveBuild(obj.GetRange(0, 1));
                node.right = recursiveBuild(obj.GetRange(1, 1));

                node.bounds = Union(node.left.bounds!, node.right.bounds!);
                node.area = node.left.area + node.right.area;
                return node;
            }
            else
            {
                Bound3 centroidBounds = new(new());
                for (int i = 0; i < obj.Count; ++i)
                    centroidBounds = Union(centroidBounds, obj[i].getBounds()!.Centroid);
                int dim = centroidBounds.MaxExtent;
                switch (dim)
                {
                    case 0:
                        obj.Sort((o1, o2) => { return o1.getBounds()!.Centroid.x.CompareTo(o2.getBounds()!.Centroid.x); });
                        break;
                    case 1:
                        obj.Sort((o1, o2) => { return o1.getBounds()!.Centroid.y.CompareTo(o2.getBounds()!.Centroid.y); });
                        break;
                    case 2:
                        obj.Sort((o1, o2) => { return o1.getBounds()!.Centroid.z.CompareTo(o2.getBounds()!.Centroid.z); });
                        break;
                }

                var beginning = 0;
                var middling = 0 + (obj.Count / 2);
                var ending = obj.Count - 1;

                var leftshapes = obj.GetRange(beginning, middling - beginning);
                var rightshapes = obj.GetRange(middling, ending - middling + 1);

                Debug.Assert(obj.Count == (leftshapes.Count + rightshapes.Count));
                node.left = recursiveBuild(leftshapes);
                node.right = recursiveBuild(rightshapes);

                node.bounds = Union(node.left.bounds!, node.right.bounds!);
                node.area = node.left.area + node.right.area;
            }
            return node;
        }
        public Intersection Intersect(Ray ray)
        {
            Intersection isect = new();
            if (root == null)
            {
                return isect;
                throw new InvalidOperationException();
            }
            isect = GetIntersection(root!, ray);
            //Debug.Assert(!isect.happened);
            return isect;
        }

        public Intersection GetIntersection(BVHBuildNode node, Ray ray)
        {

            Intersection inter = new();

            // 判断结点的包围盒与光线是否相交
            if (node.bounds?.IntersectP(ray) == false) return inter;

            if (node.left == null && node.right == null)
            {
                inter = node.obj!.getIntersection(ray);
                return inter;
            }

            // 递归判断子节点是否存在与光线相交的情况
            var hit1 = GetIntersection(node.left!, ray);
            var hit2 = GetIntersection(node.right!, ray);

            if (hit1.distance < hit2.distance)
                return hit1;
            return hit2;
        }

        public void GetSample(BVHBuildNode node, float p, ref Intersection pos, ref float pdf)
        {
            if (node.left == null || node.right == null)
            {
                node.obj!.Sample(ref pos, ref pdf);
                pdf *= node.area ?? 0;
                return;
            }
            if (p < node.left.area) GetSample(node.left, p, ref pos, ref pdf);
            else GetSample(node.right, p - node.left.area ?? 0, ref pos, ref pdf);
        }

        public void Sample(ref Intersection pos, ref float pdf)
        {
            float p = (float)(Sqrt(get_random_float()) * root!.area ?? 1);
            GetSample(root, p, ref pos, ref pdf);
            pdf /= root.area ?? 1;
        }

        public override string ToString()
        {
            return root.ToString();
        }
    }
}
