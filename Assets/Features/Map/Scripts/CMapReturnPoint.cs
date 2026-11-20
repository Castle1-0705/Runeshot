using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMapReturnPoint : MonoBehaviour
{
    private void Start()
    {
        if (CMapReturnData.HasReturnPosition)
        {
            transform.position = CMapReturnData.ReturnPosition;

            CMapReturnData.HasReturnPosition = false;
        }
    }
}
