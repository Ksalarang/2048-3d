using UnityEngine;

namespace Utils {
public static class MathUtils {
    public static float min(float a, float b, float c) {
        var min = Mathf.Min(a, b);
        return min < c ? min : c;
    }

    public static float vector2ToAngle(Vector2 direction) => Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    public static Vector3 clamp(Vector3 value, Vector3 min, Vector3 max) {
        value.x = Mathf.Clamp(value.x, min.x, max.x);
        value.y = Mathf.Clamp(value.y, min.y, max.y);
        value.z = Mathf.Clamp(value.z, min.z, max.z);
        return value;
    }
}
}