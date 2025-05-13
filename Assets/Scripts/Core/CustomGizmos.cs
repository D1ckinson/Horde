#if UNITY_EDITOR
using UnityEngine;

public sealed class CustomGizmos
{
    public static void DrawCircle(Vector3 center, float radius, Color? color = null)
    {
        center.ThrowIfNull();
        radius.ThrowIfZeroOrLess();

        Gizmos.color = color == null ? Color.yellow : (Color)color;

        float segmentsCount = 32;
        float angleStep = Constants.FullCircleDegrees / segmentsCount;

        for (int i = 0; i < segmentsCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float nextAngle = (i + 1) * angleStep * Mathf.Deg2Rad;

            Vector3 start = center + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 end = center + new Vector3(Mathf.Cos(nextAngle), 0, Mathf.Sin(nextAngle)) * radius;

            Gizmos.DrawLine(start, end);
        }
    }

    public static void DrawCone(Vector3 center, Vector3 direction, float radius, float angle)
    {
        center.ThrowIfNull();
        direction.ThrowIfDefault();
        direction.ThrowIfNotNormalize();
        radius.ThrowIfZeroOrLess();
        angle.ThrowIfZeroOrLess();

        Gizmos.color = Color.red;

        float halfAngle = angle * 0.5f;

        Vector3 rightDirection = Quaternion.Euler(0, halfAngle, 0) * direction;
        Vector3 leftDirection = Quaternion.Euler(0, -halfAngle, 0) * direction;

        Gizmos.DrawLine(center, center + rightDirection * radius);
        Gizmos.DrawLine(center, center + leftDirection * radius);

        int segments = 20;
        float angleStep = angle / segments;
        Vector3 previousPoint = center + leftDirection * radius;

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -halfAngle + angleStep * i;
            Vector3 nextPoint = center + Quaternion.Euler(0, currentAngle, 0) * direction * radius;

            Gizmos.DrawLine(previousPoint, nextPoint);

            previousPoint = nextPoint;
        }
    }
}
#endif
