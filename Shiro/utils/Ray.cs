namespace Shiro
{
    struct Ray
    {
        public Vector3f origin;
        public Vector3f direction;
        public Vector3f direction_inv { get; }
        public double t;
        public double t_min, t_max;
        Ray(Vector3f ori,
            Vector3f dir,
            double time = 0.0)
        {
            origin = ori;
            t = time;
            t_min = 0;
            t_max = double.MaxValue;
            direction = dir;
            direction_inv = new Vector3f(1f / direction.x, 1f / direction.y, 1f / direction.z);
        }

        public Vector3f this[float t] => origin + (direction * t);
    }
}
