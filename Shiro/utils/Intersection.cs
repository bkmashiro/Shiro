namespace Shiro
{
    struct Intersection
    {
        public Intersection()
        {
            happened = false;
            coords = null;
            normal = null;
            emit = null;
            tcoords = null;
            distance = null;
            obj = null;
            m = null;
        }
        public bool happened;
        public Vector3f? coords;
        public Vector3f? tcoords;
        public Vector3f? normal;
        public Vector3f? emit;
        public double? distance;
        public Object? obj;
        public Material? m;
    };
}
