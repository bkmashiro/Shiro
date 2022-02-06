using System.Text;

namespace Shiro
{
    struct hit_payload
    {
        float tNear;
        int index;
        Vector2f uv;
        Object hit_obj;
    };
    class Renderer
    {
        //parallelized needed
        static float deg2rad(float deg)
        {
            return deg * (float)PI / 180.0f;
        }

        const float EPSILON = 0.00001f;
        public static void Render(Scene scene)
        {
            Vector3f[] frameBuffer = new Vector3f[scene.width * scene.height];
            for(int i = 0;i < frameBuffer.Length;i++) frameBuffer[i] = new Vector3f(0);
            float scale = (float)Tan(deg2rad((float)(scene.fov * 0.5f)));
            float imageAspectRatio = scene.width / (float)scene.height;
            Vector3f eye_pos = new(278, 273, -800);
            int m = 0;

            // 射线数量
            int spp = 1;
            for (int j = 0; j < scene.height; ++j)
            {
                for (int i = 0; i < scene.width; ++i)
                {
                    // generate primary ray direction
                    float x = (float)((2 * (i + 0.5) / (float)scene.width - 1) *
                              imageAspectRatio * scale);
                    float y = (float)((1 - 2 * (j + 0.5) / (float)scene.height) * scale);

                    Vector3f dir = Normalize(new(-x, y, 1));
                    for (int k = 0; k < spp; k++)
                    {
                        frameBuffer[m] += scene.castRay(new(eye_pos, dir), 0) / spp;
                    }
                    m++;
                }
                ProgressHandler.UpdateProgress(j / (float)scene.height);              
            }
            ProgressHandler.UpdateProgress(1f);
            var fs = File.Create("binary.ppm");
            StringBuilder sb = new();
            sb.Append($"P6\n{scene.width} {scene.height}\n255\n");
            fs.Write(Encoding.UTF8.GetBytes(sb.ToString()));

            for (int i = 0; i < scene.width*scene.height; ++i)
            {
                byte[] b = new byte[3];
                b[0] = (byte)(255f * Pow(Clamp(frameBuffer[i].x, 0f, 1f), 0.6f));
                b[1] = (byte)(255f * Pow(Clamp(frameBuffer[i].y, 0f, 1f), 0.6f));
                b[2] = (byte)(255f * Pow(Clamp(frameBuffer[i].z, 0f, 1f), 0.6f));
                fs.Write(b);
            }
            fs.Flush();
            fs.Close();
            // save framebuffer to file
            //FILE* fp = fopen("binary.ppm", "wb");
            //(void)fprintf(fp, "P6\n%d %d\n255\n", scene.width, scene.height);
            //for (auto i = 0; i < scene.height * scene.width; ++i)
            //{
            //    static unsigned char color[3];
            //    color[0] = (unsigned char)(255 * std::pow(clamp(0, 1, framebuffer[i].x), 0.6f));
            //color[1] = (unsigned char)(255 * std::pow(clamp(0, 1, framebuffer[i].y), 0.6f));
            //color[2] = (unsigned char)(255 * std::pow(clamp(0, 1, framebuffer[i].z), 0.6f));
            //fwrite(color, 1, 3, fp);
            //}
            //fclose(fp);
        }
    }

    public static class ProgressHandler
    {
        static int maxProgress = 0;
        static int progress;

        public static void UpdateProgress(float progress)
        {
            int barWidth = 70;
            progress = Clamp(progress, 0f, 1f);
            Console.Write('[');
            int pos = (int)(barWidth * progress);
            for (int i = 0; i < barWidth; ++i)
            {
                if (i < pos) Console.Write('=');
                else if (i == pos) Console.Write('>');
                else Console.Write(' ');
            }
            Console.Write($"]{progress * 100.0} %\r");
        }
    }
}
