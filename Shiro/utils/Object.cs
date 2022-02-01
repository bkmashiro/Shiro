namespace Shiro
{
    class Object
    {
        public Object() { }

        public virtual bool intersect(Ray ray) { return false; }
        public virtual bool intersect(Ray ray, float tnear, int index) { return false; }
        public virtual Intersection getIntersection(Ray _ray) { return new Intersection(); }
        public virtual void getSurfaceProperties(Vector3f P, Vector3f I, int index, Vector2f uv, Vector3f N, Vector2f st) { }
        public virtual Vector3f? evalDiffuseColor(Vector2f v) { return null;}
        public virtual Bound3? getBounds() { return null; }
        public virtual float getArea() { return 0; }
        public virtual void Sample(Intersection pos,out float pdf) { pdf = 0; }
        public virtual bool hasEmit() { return false; }
    }
}
