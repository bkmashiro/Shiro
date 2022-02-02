namespace Shiro
{
    class Scene
    {
        public static int width = 1280;
        public static int height = 960;
        public static double fov = 40;
        public static Vector3f backgroundColor = new(0.235294f, 0.67451f, 0.843137f);
        public static int maxDepth = 1;
        public static float RussianRoulette = 0.8F;
        List<Object> objects = new List<Object>();
        List<Light> lights = new List<Light>();
        BVHAccel BVHAccel;

        public Scene(int w, int h)
        {
            width = w;
            height = h;
        }
        Vector3f reflect(Vector3f I, Vector3f N)
        {
            return I - 2 * DotProduct(I, N) * N;
        }
        // Compute refraction direction using Snell's law
        //
        // We need to handle with care the two possible situations:
        //
        //    - When the ray is inside the object
        //
        //    - When the ray is outside.
        //
        // If the ray is outside, you need to make cosi positive cosi = -N.I
        //
        // If the ray is inside, you need to invert the refractive indices and negate the normal N
        Vector3f refract(Vector3f I, Vector3f N, float ior)
        {
            float cosi = Clamp(-1, 1, DotProduct(I, N));
            float etai = 1, etat = ior;
            Vector3f n = N;
            if (cosi < 0) { cosi = -cosi; } else { Swap(ref etai, ref etat); n = -N; }
            float eta = etai / etat;
            float k = 1 - eta * eta * (1 - cosi * cosi);
            return k < 0 ? Vector3f.ZERO : (eta * I) + ((float)((eta * cosi) - Sqrt(k)) * n);
        }

        // Compute Fresnel equation
        //
        // \param I is the incident view direction
        //
        // \param N is the normal at the intersection point
        //
        // \param ior is the material refractive index
        //
        // \param[out] kr is the amount of light reflected
        public void fresnel(Vector3f I, Vector3f N, float ior, out float kr)
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

        public void BUlidBVH()
        {
            Console.WriteLine("Building BVH");
            BVHAccel = new BVHAccel(objects, 1, SplitMethod.NAIVE);
        }

        public Intersection intersect(Ray ray) { return BVHAccel.Intersect(ray); }

        public void sampleLight(Intersection pos, out float pdf)
        {

            float emit_area_sum = 0;
            for (int k = 0; k < objects.Count; ++k)
            {
                if (objects[k].hasEmit())
                {
                    emit_area_sum += objects[k].getArea();
                }
            }
            float p = get_random_float() * emit_area_sum;
            emit_area_sum = 0;
            for (int k = 0; k < objects.Count; ++k)
            {
                if (objects[k].hasEmit())
                {
                    emit_area_sum += objects[k].getArea();
                    if (p <= emit_area_sum)
                    {
                        objects[k].Sample(pos, out pdf);
                        break;
                    }
                }
            }
            pdf = 0;
            throw new Exception("pdf error");
        }

        public bool trace(
            Ray ray, List<Object> objects, ref float tNear, ref int index, ref Object? hitObject)
        {
            hitObject = null;
            for (int k = 0; k < objects.Count; ++k)
            {
                float tNearK = float.PositiveInfinity;
                int indexK=0;
                Vector2f uvK;
                if (objects[k].intersect(ref ray, ref tNearK, ref indexK) && tNearK < tNear)
                {
                    hitObject = objects[k];
                    tNear = tNearK;
                    index = indexK;
                }
            }
            return (hitObject != null);
        }
    }
}
