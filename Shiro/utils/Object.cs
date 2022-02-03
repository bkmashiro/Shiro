namespace Shiro
{
    abstract class Object
    {
        public Object() { }

        public abstract bool intersect(Ray ray);
        public abstract bool intersect(ref Ray ray, ref float tnear, ref int index);
        public abstract Intersection getIntersection(Ray _ray);
        public abstract void getSurfaceProperties(Vector3f P, Vector3f I, int index, Vector2f uv,out Vector3f N,out Vector2f st);
        public abstract Vector3f? evalDiffuseColor(Vector2f v);
        public abstract Bound3? getBounds();
        public abstract float getArea();
        public abstract void Sample(Intersection pos, out float pdf);
        public abstract bool hasEmit();
    }
}
