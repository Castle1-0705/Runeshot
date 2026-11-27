using UnityEngine;

public static class CMapReturnData
{
    public static Vector3 ReturnPosition;
    public static bool HasReturnPosition;

    public static bool IsInsideHouse => HasReturnPosition;
}
