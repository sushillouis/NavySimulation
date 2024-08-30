using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {


    public static float EPSILON = 0.01f;
    public static bool ApproximatelyEqual(float a, float b)
    {
        return (Mathf.Abs(a - b) < EPSILON);
    }

    public static float Clamp(float val, float min, float max)
    {
        if (val < min)
            val = min;
        if (val > max)
            val = max;
        return val;
    }

    public static float AngleDiffPosNeg(float a, float b)
    {
        float diff = a - b;
        if (diff > 180)
            return diff - 360;
        if (diff < -180)
            return diff + 360;
        return diff;
    }

    public static float Degrees360(float angleDegrees)
    {
        while (angleDegrees >= 360)
            angleDegrees -= 360;
        while (angleDegrees < 0)
            angleDegrees += 360;
        return angleDegrees;

    }

    public static float VectorToHeadingDegrees(Vector3 v)
    {
        return Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
    }

    public static bool AngleBetween(float a, float low, float high)
    {
        float a360 = Degrees360(a);
        float low360 = Degrees360(low);
        float high360 = Degrees360(high);

        if (high360 > low360)
        {
            return (a360 > low360) && (a360 < high360);
        }
        else
        {
            return (a360 > low360) || (a360 < high360);
        }
    }

    public static float TCPA(Entity e1, Entity e2)
    {
        Vector3 p = e2.position - e1.position;
        Vector3 v = e2.velocity - e1.velocity;
        float t = Mathf.Acos(Vector3.Dot(-p, v) / (p.magnitude * v.magnitude));

        float tcpa = p.magnitude * Mathf.Cos(t) / v.magnitude;
        return tcpa;
    }
}
