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
    };

    class BVHAccel
    {
        BVHBuildNode? root;
        public BVHAccel(List<Object> p, int maxPrimsInNode = 1, SplitMethod splitMethod = SplitMethod.NAIVE)
        {
            recursiveBuild(p);
        }

        BVHBuildNode recursiveBuild(List<Object> obj)
        {
            BVHBuildNode node = new BVHBuildNode();
            // Compute bounds of all primitives in BVH node
            Bound3 bounds = new(new());
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
                node.left = recursiveBuild(obj.GetRange(0,0));
                node.right = recursiveBuild(obj.GetRange(1, 1));

                node.bounds = Union(node.left.bounds!, node.right.bounds!);
                node.area = node.left.area + node.right.area;
                return node;
            }
            else
            {
                Bound3 centroidBounds=new(new());
                for (int i = 0; i < obj.Count; ++i)
                    centroidBounds = Union(centroidBounds, obj[i].getBounds()!.Centroid);
                int dim = centroidBounds.MaxExtent;
                switch (dim)
                {
                    case 0:
                        obj.Sort((o1, o2) => { return o1.getBounds()!.Centroid.x.CompareTo(o1.getBounds()!.Centroid.x); });
                        break;
                    case 1:
                        obj.Sort((o1, o2) => { return o1.getBounds()!.Centroid.y.CompareTo(o1.getBounds()!.Centroid.y); });
                        break;
                    case 2:
                        obj.Sort((o1, o2) => { return o1.getBounds()!.Centroid.z.CompareTo(o1.getBounds()!.Centroid.z); });
                        break;
                }

                var beginning = 0;
                var middling = 0 + (obj.Count / 2);
                var ending = obj.Count;

                var leftshapes = obj.GetRange(beginning,middling);
                var rightshapes = obj.GetRange(middling, ending);

                Debug.Assert(obj.Count == (leftshapes.Count + rightshapes.Count));
                node.left = recursiveBuild(leftshapes);
                node.right = recursiveBuild(rightshapes);

                node.bounds = Union(node.left.bounds!, node.right.bounds!);
                node.area = node.left.area + node.right.area;
            }

            return node;
        }

    }
}
