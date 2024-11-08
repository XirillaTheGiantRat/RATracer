using OpenTK.Mathematics;

namespace INFOGR2024Template
{
    public partial class Surface
    {
        Vector3d camera2;
        Color4 spawnColour = new Color4(1.0f, 0.5f, 0.31f, 1.0f);
        Vector3d ray;

        public void Render(Scene scene)
        {
            int width2 = width / 2;
            camera2 = new Vector3d(0f, 4f, 4f);
            scene.primitives.Add(new Primitive.Sphere(new Vector3d(35f, -3f, 30f), 10f, spawnColour));
            scene.primitives.Add(new Primitive.Sphere(new Vector3d(33f, 10f, 25f), 10f, spawnColour));
            scene.primitives.Add(new Primitive.Sphere(new Vector3d(19f, 10f, 10f), 5f, spawnColour));

            scene.primitives.Add(new Primitive.Plane(new Vector3d(0f, -6f, 0f), new Vector3d(0f, 1f, 0f), spawnColour));
            scene.lightsources.Add(new Light(new Vector3d(-3, 5, 0), new Color4(0.3f, 0.3f, 0.3f, 1)));
            scene.lightsources.Add(new Light(new Vector3d(3, 5, 0), new Color4(1, 1, 1, 1)));

            float aspectratio = (float)width2 / height;

            Bar(width / 4, height / 2, width / 4 + 4, height / 2 + 4, 345851);

            foreach (var primitive in scene.primitives)
            {
                if (primitive is Primitive.Sphere)
                {
                    DrawCircle((int)primitive.position.X, (int)primitive.position.Z * -1, (int)primitive.radius, primitive.color);
                }
            }

            for (int i = width2; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float ndcX = (i + 0.5f) / width2 * 2 - 1;
                    float ndcY = 1 - (j + 0.5f) / height * 2;
                    ndcX *= aspectratio;

                    Vector3d dir = new Vector3d(ndcX, ndcY, 1);
                    dir = Vector3d.Normalize(dir);
                    GenerateRay(camera2, dir);

                    double closestT = double.MaxValue;
                    Primitive closestprim = null;
                    Vector3d closesthp = new Vector3d();
                    Vector3d closestnorm = new Vector3d();

                        foreach (var primitive in scene.primitives)
                    {
                        if (primitive is Primitive.Plane)
                        {
                            double T = Intersectionplane(camera2, dir, primitive);

                            if (T > 0)
                            {
                                Vector3d hitpointPlane = camera2 + T * dir;
                                Vector3d normalPlane = Vector3d.Normalize(hitpointPlane - primitive.position);
                                Color4 color = new Color4((float)normalPlane.X + 1, (float)normalPlane.Y + 1, (float)normalPlane.Z + 1, (float)1);
                                Plot(i, j, color);
                            }
                        }

                        if (primitive is Primitive.Sphere)
                        {
                            double t = IntersectionSphere(camera2, dir, primitive);
                            if (t > 0 && t < closestT)
                            {
                                closestT = t;
                                closestprim = primitive;
                                closesthp = camera2 + t * dir;
                                closestnorm = Vector3d.Normalize(closesthp - primitive.position);
                            }
                        }
                    }
                    if (closestprim != null)
                    {
                        Color4 color = lightdiff(closesthp, closestnorm, scene.lightsources, closestprim, scene);
                        Plot(i, j, color);
                    }
                    else
                    {
                        Plot(i, j, new Color4(0.33f, 0.42f, 0.18f, 1.0f));
                    }
                }
            }
        }


        void DrawCircle(int x, int y, int r, Color4 color)
        {
            double x1, y1;
            x = x + width / 4;
            y = y + height / 2;

            for (double angle = 0; angle <= 360; angle += 0.5)
            {
                x1 = r * Math.Cos(angle);
                y1 = r * Math.Sin(angle);
                Plot((int)(x + x1), (int)(y + y1), color);
            }
        }

            public void GenerateRay(Vector3d origin, Vector3d direction)
            {
                ray = origin + direction;
            }

            public double Intersectionplane(Vector3d origin, Vector3d dir, Primitive plane)
            {

                double t = -((origin.Y - plane.position.Y) / dir.Y);
                //double t = (plane.position.Y - origin.Y) / dir.Y;
                if (t > 0 && Double.IsFinite(t))
                {
                    return t;
                }
                return -1.0;
            }


            public double IntersectionSphere(Vector3d Origin, Vector3d dir, Primitive primitive)
            {
                Vector3d oc = primitive.position - Origin;
                double a = dir.Length * dir.Length;
                double h = Vector3d.Dot(dir, oc);
                double c = oc.Length * oc.Length - (primitive.radius * primitive.radius);
                double D = h * h - a * c;

                if (D < 0)
                {
                    return -1.0;
                }
                else
                {
                    return (h - Math.Sqrt(D)) / a;
                }
            }

            public Color4 CalculateColor(Vector3d normal)
            {
                float r = (float)((normal.X + 1) * 0.5f);
                float g = (float)((normal.Y + 1) * 0.5f);
                float b = (float)((normal.Z + 1) * 0.5f);

                r = r < 0 ? 0 : (r > 1 ? 1 : r);
                g = g < 0 ? 0 : (g > 1 ? 1 : g);
                b = b < 0 ? 0 : (b > 1 ? 1 : b);

                return new Color4(r, g, b, 1.0f);
            }

            public Color4 lightdiff(Vector3d hp, Vector3d normal, List<Light> lights, Primitive primitive, Scene scene)
            {
                float ambient = 0.1f;
                float maxdiff = 1.0f;
                float r = 0, g = 0, b = 0;

                foreach (var light in lights)
                {
                    Vector3d ldir = Vector3d.Normalize(light.Position - hp);
                    float diffuse = MathF.Max((float)Vector3d.Dot(normal, ldir), 0) * maxdiff;
                    Vector3d origin = hp + normal * 0.001f;
                    Vector3d sdir = ldir;
                    bool shadow = false;

                    foreach (var prim in scene.primitives)
                    {
                        if (prim != primitive)
                        {
                            double t = IntersectionSphere(origin, sdir, prim);
                            if (t > 0 && t < Vector3d.Distance(hp, light.Position))
                            {
                                shadow = true;
                                break;
                            }
                        }
                    }

                    if (shadow)
                    {
                        r += primitive.color.R * ambient;
                        g += primitive.color.G * ambient;
                        b += primitive.color.B * ambient;
                    }
                    else
                    {
                        r += primitive.color.R * (ambient + diffuse);
                        g += primitive.color.G * (ambient + diffuse);
                        b += primitive.color.B * (ambient + diffuse);
                    }
                }

                int lightcount = lights.Count;
                r /= lightcount;
                g /= lightcount;
                b /= lightcount;

                r = r < 0 ? 0 : (r > 1 ? 1 : r);
                g = g < 0 ? 0 : (g > 1 ? 1 : g);
                b = b < 0 ? 0 : (b > 1 ? 1 : b);

                return new Color4(r, g, b, primitive.color.A);
            }
    }
} 