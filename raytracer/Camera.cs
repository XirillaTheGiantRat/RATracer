using System.Numerics;

namespace INFOGR2024Template
{
    internal class Camera
    {
        public Vector3 position;
        Vector3 lookAtDirection;
        Vector3 UpDirection;
        float p0, p1, p2, p3;
        public Camera(Vector3 pos, Vector3 lookAt, Vector3 Updir ) 
        {
            position = new Vector3(0,0,0);
            lookAtDirection = new Vector3(0,0,1);
            UpDirection = new Vector3(0,1,0);
        }
    }
}
