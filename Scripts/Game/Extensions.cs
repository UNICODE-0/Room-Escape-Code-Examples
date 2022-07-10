using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 Abs(this Vector3 Vec)
    {
        return new Vector3(Mathf.Abs(Vec.x),Mathf.Abs(Vec.y),Mathf.Abs(Vec.z));
    }
    public static Vector3 MultiplyBy(this Vector3 FirstVector , Vector3 SecondVector)
    {
        return new Vector3(FirstVector.x * SecondVector.x, FirstVector.y * SecondVector.y, FirstVector.z * SecondVector.z);
    }
        public static Vector3 MultiplyBy(this Vector3 FirstVector , float Multiplier)
    {
        return new Vector3(FirstVector.x * Multiplier, FirstVector.y * Multiplier, FirstVector.z * Multiplier);
    }
    public static Vector3 DivideBy(this Vector3 Vec, float Divider)
    {
        return new Vector3(Vec.x / Divider,Vec.y / Divider,Vec.z / Divider );
    }
}
