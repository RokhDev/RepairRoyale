using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magic
{
    // Custom math class
    public struct MathS
    {
        public static float AsyntotalGrowth(float x, float asyntote, float curvature = 5)
        {
            return -(curvature / (x + (curvature / asyntote))) + asyntote;
        }

        public static float ExponentialGrowth(float x, float innerAmplitude = 1, float outerAmplitude = 1)
        {
            return ((x * x) / innerAmplitude) * outerAmplitude;
        }

        public static float LinearGrowth(float x, float lowerAngle = 1, float upperAngle = 1)
        {
            return (x * lowerAngle) / upperAngle;
        }

        public static Vector3 GetIntersectionPoint(Vector3 targetPosition, Vector3 targetVelocity, Vector3 myPosition, float mySpeed)
        {
            Vector3 totarget = targetPosition - myPosition;

            float a = Vector3.Dot(targetVelocity, targetVelocity) - (mySpeed * mySpeed);
            float b = 2 * Vector3.Dot(targetVelocity, totarget);
            float c = Vector3.Dot(totarget, totarget);

            float p = -b / (2 * a);
            float q = (float)Mathf.Sqrt((b * b) - 4 * a * c) / (2 * a);

            float t1 = p - q;
            float t2 = p + q;
            float t;

            if (t1 > t2 && t2 > 0)
            {
                t = t2;
            }
            else
            {
                t = t1;
            }

            Vector3 aimSpot = targetPosition + targetVelocity * t;
            return aimSpot;
        }

        public static float Squish(float n, float max, float min)
        {
            return (n - min) / (max - min);
        }

        public static float InverseProportion(float val, float max)
        {
            return Mathf.Abs(val - max);
        }
		
        public static void LookAt2D(Transform origin, Transform target)
        {
            Vector3 trueTarget = new Vector3(target.position.x, target.position.y, origin.position.z);
            origin.right = trueTarget - origin.position;
        }

        public static void LookAt2D(Transform origin, Vector3 target)
        {
            Vector3 trueTarget = new Vector3(target.x, target.y, origin.position.z);
            origin.right = trueTarget - origin.position;
        }
		
		public static Vector3 DisplaceVector(Vector3 origin, Quaternion direction, float distance)
        {
            Vector3 dir = direction * Vector3.forward;
            dir.Normalize();
            return (dir * distance) + origin;
        }

        public static Vector3 DisplaceVector(Vector3 origin, Vector3 direction, float distance)
        {
            direction.Normalize();
            return (direction * distance) + origin;
        }

        public static Vector2 DisplaceVector2D(Vector2 origin, Quaternion direction, float distance)
        {
            Vector2 dir = direction * Vector2.right;
            dir.Normalize();
            return (dir * distance) + origin;
        }

        public static Vector2 DisplaceVector2D(Vector2 origin, Vector2 direction, float distance)
        {
            direction.Normalize();
            return (direction * distance) + origin;
        }
		
		
        public static Vector3 GetQuadrantToDirection(Vector3 origin, Vector3 target)
        {
            Vector3 movementDirection = target - origin;
            movementDirection.Normalize();
            float angle = Vector3.Angle(movementDirection, Vector3.up);

            if (angle >= 315.0f)
                return Vector3.up;
            else if (angle <= 45.0f)
                return Vector3.up;
            else if ((angle >= 45.0f && angle <= 135.0f) && origin.x < target.x)
                return Vector3.right;
            else if ((angle >= 45.0f && angle <= 135.0f) && origin.x > target.x)
                return Vector3.left;
            else if (angle >= 135.0f)
                return Vector3.down;
            else
                return Vector3.down;
        }
    }
}
