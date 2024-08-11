using UnityEngine;

namespace Utils
{
    public static class MathUtils
    {
        public static bool TriangleContainsPoint(Vector2 point, Vector2 t1, Vector2 t2, Vector2 t3)
        {
            float n1 = (t1.x - point.x) * (t2.y - t1.y) - (t2.x - t1.x) * (t1.y - point.y);
            float n2 = (t2.x - point.x) * (t3.y - t2.y) - (t3.x - t2.x) * (t2.y - point.y);
            float n3 = (t3.x - point.x) * (t1.y - t3.y) - (t1.x - t3.x) * (t3.y - point.y);
            
            if ((n1 >= 0 && n2 >= 0 && n3 >= 0)
                || (n1 <= 0 && n2 <= 0 && n3 <= 0))
                return true;
            return false;
        }
        
        public static float	RelativeAngle(this Vector3 forwardDirection, Vector3 targetDirection, Vector3 upDirection)
        {
            var	angle = Vector3.Angle(forwardDirection, targetDirection);

            if (forwardDirection.AngleDirection(targetDirection, upDirection) == -1)
                return -angle;
            else
                return angle;
        }
        
        public static int AngleDirection(this Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3	perp = Vector3.Cross(fwd, targetDir);
            float	dir = Vector3.Dot(perp, up);

            if (dir > 0)
                return 1;
            else if (dir < 0)
                return -1;
            else
                return 0;
        }
    }
}