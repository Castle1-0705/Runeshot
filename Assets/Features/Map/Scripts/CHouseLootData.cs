using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHouseLootData
{
    private static List<string> _collectedItems = new List<string>();

    private static string _currentHouseID;

    public static void SetCurrentHouse(string houseID)
    {
        _currentHouseID = houseID;
    }

    public static bool IsCollected(string itemID)
    {
        string uniqueID = _currentHouseID + "_" + itemID;

        return _collectedItems.Contains(uniqueID);
    }

    public static void Collect(string itemID)
    {
        string uniqueID = _currentHouseID + "_" + itemID;

        if (!_collectedItems.Contains(uniqueID))
        {
            _collectedItems.Add(uniqueID);
        }
    }
}
