using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOGR2024Template
{
    public class Light
    {
        public Vector3d Position { get; set; }
        public Color4 Intensity { get; set; }

        public Light(Vector3d position, Color4 intensity)
        {
            Position = position;
            Intensity = intensity;
        }

    }
}
