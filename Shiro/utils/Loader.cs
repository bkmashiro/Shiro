namespace Shiro.utils
{
    class OBJReader
    {
        struct Vector2
        {
            // Default Constructor
            public Vector2()
            {
                X = 0.0f;
                Y = 0.0f;
            }
            // Variable Set Constructor
            Vector2(float X_, float Y_)
            {
                X = X_;
                Y = Y_;
            }
            // Bool Equals Operator Overload
            public static bool operator ==(Vector2 t, Vector2 other)
            {
                return (t.X == other.X && t.Y == other.Y);
            }
            // Bool Not Equals Operator Overload
            public static bool operator !=(Vector2 t, Vector2 other)
            {
                return !(t.X == other.X && t.Y == other.Y);
            }
            // Addition Operator Overload
            public static Vector2 operator +(Vector2 t, Vector2 right)
            {
                return new(t.X + right.X, t.Y + right.Y);
            }
            // Subtraction Operator Overload
            public static Vector2 operator -(Vector2 t, Vector2 right)
            {
                return new(t.X - right.X, t.Y - right.Y);
            }
            // Float Multiplication Operator Overload
            public static Vector2 operator *(Vector2 t, float other)
            {
                return new(t.X * other, t.Y * other);
            }

            // Positional Variables
            float X;
            float Y;

            public override bool Equals(object? obj)
            {
                throw new NotImplementedException();
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        };

        struct Vector3
        {
            // Default Constructor
            public Vector3()
            {
                X = 0.0f;
                Y = 0.0f;
                Z = 0.0f;
            }
            // Variable Set Constructor
            public Vector3(float X_, float Y_, float Z_)
            {
                X = X_;
                Y = Y_;
                Z = Z_;
            }
            // Bool Equals Operator Overload
           public static bool operator ==(Vector3 t, Vector3 other)
            {
                return (t.X == other.X && t.Y == other.Y && t.Z == other.Z);
            }
            // Bool Not Equals Operator Overload
            public static bool operator !=(Vector3 t, Vector3 other)
            {
                return !(t.X == other.X && t.Y == other.Y && t.Z == other.Z);
            }
            // Addition Operator Overload
            public static Vector3 operator +(Vector3 t, Vector3 right)
            {
                return new(t.X + right.X, t.Y + right.Y, t.Z + right.Z);
            }
            // Subtraction Operator Overload
            public static Vector3 operator -(Vector3 t, Vector3 right)
            {
                return new(t.X - right.X, t.Y - right.Y, t.Z - right.Z);
            }
            // Float Multiplication Operator Overload
            public static Vector3 operator *(Vector3 t, float other)
            {
                return new(t.X * other, t.Y * other, t.Z * other);
            }
            // Float Division Operator Overload
            public static Vector3 operator /(Vector3 t, float other)
            {
                return new(t.X / other, t.Y / other, t.Z / other);
            }

            // Positional Variables
            float X;
            float Y;
            float Z;

            public override bool Equals(object? obj)
            {
                throw new NotImplementedException();
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }
        };

        struct Mesh
        {
            // Default Constructor
            public Mesh()
            {

            }
            // Variable Set Constructor
            public Mesh(List<Vertex> _Vertices, List<uint> _Indices)
            {
                Vertices = _Vertices;
                Indices = _Indices;
                MeshMaterial = null;
            }
            // Mesh Name
            string MeshName="";
            // Vertex List
            List<Vertex> Vertices=new();
            // Index List
            List<uint> Indices = new();

            // Material
            Material? MeshMaterial = null;
        };

        struct Vertex
        {
            // Position Vector
            Vector3 Position;

            // Normal Vector
            Vector3 Normal;

            // Texture Coordinate Vector
            Vector2 TextureCoordinate;
        };
    }
}
