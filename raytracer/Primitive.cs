using OpenTK.Mathematics;

namespace INFOGR2024Template
{
    public class Primitive
    {
        public Vector3d position, normal;
        public double radius, distance;
        public Color4 color;
        public Primitive() 
        {
        }

        public class Sphere : Primitive
        {
            public Sphere(Vector3d pos, float radius, Color4 color)
            {
                this.position = pos;
                this.radius = radius;
                this.color = color;

            }

        }

        public class Plane : Primitive
        {
            public Vector3d Position { get; set; }
            public Vector3d Normal { get; set; }
            public Color4 Color { get; set; }

            public Plane(Vector3d position, Vector3d normal, Color4 color)
            {
                Position = position;
                Normal = normal;
                Color = color;
            }
        }
    }

}
