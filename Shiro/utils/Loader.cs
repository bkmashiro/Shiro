namespace Shiro
{
    struct Mesh
    {
        public Mesh()
        {
        }
        public string MeshName = "";
        public List<Vector3f> Vertex = new();
        public List<Vector2f> Texure = new();
        public List<Vector3f> Normal = new();
        public List<Triangle> Faces = new();

        // Material
        public Material? MeshMaterial = null;
    };
    class OBJReader
    {
        public int LoadedMeshes;
        public int VertexCount => mesh.Vertex.Count;
        public int UVCount => mesh.Texure.Count;
        public int FaceCount => mesh.Faces.Count;
        public int NormalCount => mesh.Normal.Count;
        public Mesh mesh;

        public OBJReader(string path)
        {
            mesh = new Mesh();
            if (!File.Exists(path))
                return;
            StreamReader sr = new StreamReader(path);
            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                if (line == null)
                    continue;
                string[] parts = line.Split(' ');
                switch (parts[0])
                {
                    case "v":
                        mesh.Vertex.Add(new(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;
                    case "vt":
                        mesh.Texure.Add(new(float.Parse(parts[1]), float.Parse(parts[2])));
                        break;
                    case "vn":
                        mesh.Normal.Add(new(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])));
                        break;
                    case "f":
                        var p1 = parts[1].Split(' ').ToList().ConvertAll(int.Parse);
                        var p2 = parts[2].Split(' ').ToList().ConvertAll(int.Parse);
                        var p3 = parts[3].Split(' ').ToList().ConvertAll(int.Parse);
                        Triangle triangle = new(
                            mesh.Vertex[p1[0]],
                            mesh.Vertex[p2[0]],
                            mesh.Vertex[p3[0]]);
                        triangle.t0 = mesh.Texure[p1[1]];
                        triangle.t1 = mesh.Texure[p2[1]];
                        triangle.t2 = mesh.Texure[p3[1]];
                        triangle.normal = mesh.Normal[p1[2]];//法线三个顶点都相同
                        mesh.Faces.Add(triangle);
                        break;
                    default:
                        throw new Exception("unable to resolve obj file.");
                }
            }
            ++LoadedMeshes;
        }
        public Mesh GetMesh()
        {
            return this.mesh;
        }
    }
}
