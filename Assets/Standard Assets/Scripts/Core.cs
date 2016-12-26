using UnityEngine;
using System.Collections;

public static class Core 
{
    public static float Angle(this Vector3 v)
    {
        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }
    public static float Squared(this float v)
    {
        return v * v;
    }

    public static double Squared(this double v)
    {
        return v * v;
    }

    public static Vector4 Mask(this Vector4 value, Vector4 mask)
    {
        return new Vector4(value.x * mask.x, value.y * mask.y, value.z * mask.z, value.w * mask.w);
    }

    public static Vector4 InverseMask(this Vector4 value, Vector4 mask)
    {
        mask.x += (0.5f - mask.x) * 2;
        mask.y += (0.5f - mask.y) * 2;
        mask.z += (0.5f - mask.z) * 2;
        mask.w += (0.5f - mask.w) * 2;
        return new Vector4(value.x * mask.x, value.y * mask.y, value.z * mask.z, value.w * mask.w);
    }
    public static float MaskSum(this Vector4 value, Vector4 mask)
    {
        return value.x * mask.x + value.y * mask.y + value.z * mask.z + value.w * mask.w;
    }
}
