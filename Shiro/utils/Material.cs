using static Shiro.GLOBAL;
using static Shiro.Vectors;
using static System.Math;
namespace Shiro
{
    enum MaterialType
    {
        DIFFUSE,
    }
    class Material
    {
        /// <summary>
        /// 反射
        /// </summary>
        /// <param name="I"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        Vector3f reflect(Vector3f I, Vector3f N)
        {
            return I - 2 * DotProduct(I, N) * N;
        }
        /// <summary>
        /// 折射
        /// </summary>
        /// <param name="I"></param>
        /// <param name="N"></param>
        /// <param name="ior"></param>
        /// <returns></returns>
        Vector3f refract(Vector3f I, Vector3f N, float ior)
        {

            float cosi = Clamp(DotProduct(I, N), -1f, 1f);
            float etai = 1, etat = ior;
            Vector3f n = N;
            if (cosi < 0) { cosi = -cosi; }
            else { Swap(ref etai, ref etat); n = -N; }
            float eta = etai / etat;
            float k = 1 - eta * eta * (1 - cosi * cosi);
            return k < 0 ? new Vector3f(0) : eta * I + (eta * cosi - (float)Sqrt(k)) * n;
        }
        void fresnel(Vector3f I, Vector3f N, float ior, out float kr)
        {

            float cosi = Clamp(-1, 1, DotProduct(I, N));
            float etai = 1, etat = ior;
            if (cosi > 0) { Swap(ref etai, ref etat); }
            // Compute sini using Snell's law
            float sint = (float)(etai / etat * Sqrt(Max(0f, 1 - cosi * cosi)));
            // Total internal reflection
            if (sint >= 1)
            {
                kr = 1;
            }

            else
            {
                float cost = (float)Sqrt(Max(0f, 1 - sint * sint));
                cosi = Abs(cosi);
                float Rs = ((etat * cosi) - (etai * cost)) / ((etat * cosi) + (etai * cost));
                float Rp = ((etai * cosi) - (etat * cost)) / ((etai * cosi) + (etat * cost));
                kr = (Rs * Rs + Rp * Rp) / 2;
            }
            // As a consequence of the conservation of energy, transmittance is given by:
            // kt = 1 - kr;
        }
        Vector3f toWorld(Vector3f a, Vector3f N)
        {
            Vector3f B, C;
            if (Abs(N.x) > Abs(N.y))
            {
                float invLen = (float)(1.0f / Sqrt(N.x * N.x + N.z * N.z));
                C = new Vector3f(N.z * invLen, 0.0f, -N.x * invLen);
            }
            else
            {
                float invLen = (float)(1.0f / Sqrt(N.y * N.y + N.z * N.z));
                C = new Vector3f(0.0f, N.z * invLen, -N.y * invLen);
            }
            B = CrossProduct(C, N);
            return a.x * B + a.y * C + a.z * N;
        }
        public MaterialType MaterialType;
        public Vector3f? Emission;
        public Vector3f? Ks=new(1);
        public Vector3f? Kd { get { return Kd; } set { Kd = value??new(0); } }
        public Material(MaterialType t, Vector3f? e)
        {
            MaterialType = t;
            Emission = e;
        }
        public MaterialType GetMaterialType() { return MaterialType; }
        public Vector3f? getEmission() { return Emission; }
        public bool HasEmission() { return Emission != null; }
        public Vector3f GetColorAt(float u, float v)
        {
            //TODO
            throw new NotImplementedException();
        }
        public Vector3f Sample(Vector3f wi, Vector3f N)
        {
            switch (MaterialType)
            {
                case MaterialType.DIFFUSE:
                    {
                        // uniform sample on the hemisphere
                        float x_1 = get_random_float(), x_2 = get_random_float();
                        float z = Abs(1.0f - 2.0f * x_1);
                        float r = (float)Sqrt(1.0f - z * z), phi = (float)(2 * PI * x_2);
                        Vector3f localRay = new(r * (float)Cos(phi), r * (float)Sin(phi), z);
                        return toWorld(localRay, N);
                    }
                default:
                    {
                        return Vector3f.ZERO;
                    }
            }
        }
        //wi:观测方向 wo入射方向的反向 n面的法线
        public float PDF(Vector3f wi, Vector3f wo, Vector3f N)
        {
            switch (MaterialType)
            {
                case MaterialType.DIFFUSE:
                    {
                        // uniform sample probability 1 / (2 * PI)
                        if (DotProduct(wo, N) > 0.0f)
                            return ((float)(0.5f / PI));
                        else
                            return 0.0f;
                    }
                default:
                    {
                        return 0f;
                    }
            }
        }
        //wi:观测方向 wo入射方向的反向 n面的法线
        public Vector3f eval(Vector3f wi, Vector3f wo, Vector3f N)
        {
            switch (MaterialType)
            {
                case MaterialType.DIFFUSE:
                    {
                        // calculate the contribution of diffuse model
                        float cosalpha = DotProduct(N, wo);
                        if (cosalpha > 0.0f)
                        {
                            Vector3f diffuse = Kd! / (float)PI;
                            return diffuse;
                        }
                        else
                            return Vector3f.ZERO;
                    }
                default:
                    {
                        return Vector3f.ZERO;
                    }
            }
        }
    }
}
