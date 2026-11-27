using UnityEngine;

public static class CMapReturnData
{
    public static Vector3 ReturnPosition;
    public static bool HasReturnPosition;

    public static CHouseInteriorController CurrentInterior;

    public static bool IsInsideHouse => HasReturnPosition;

    public static void Reset()
    {
        ReturnPosition = Vector3.zero;
        HasReturnPosition = false;
        CurrentInterior = null;
    }
}
