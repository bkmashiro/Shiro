namespace Shiro.utils
{
    class AreaLight : Light
    {
        public float length;
        public Vector3f normal;
        public Vector3f u;
        public Vector3f v;
        public AreaLight(Vector3f _position, Vector3f _intensity) : base(_position, _intensity)
        {
            normal = new(0, -1, 0);
            u = new(1, 0, 0);
            v = new(0, 0, 1);
            length = 100;
        }
        public Vector3f SamplePoint()
        {
            var random_u = get_random_float();
            var random_v = get_random_float();
            return position + random_u * u + random_v * v;
        }
    }
}
