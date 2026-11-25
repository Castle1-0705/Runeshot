using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCloseButton : MonoBehaviour
{
    [SerializeField] GameObject _closeObject;
     
    public void CloseTimeScale1()
    {
        Close();
    }

    public void Close()
    {
        if (_closeObject == null)
        {
            return;
        }

        CLootChestUIController chestUI = _closeObject.GetComponent<CLootChestUIController>();
        if (chestUI != null)
        {
            chestUI.CloseCurrentChest();
            return;
        }

        _closeObject.SetActive(false);
        TimeScale();
    }

    public void TimeScale()
    {
        if (_closeObject == null)
        {
            return;
        }

        COptionPenel option = _closeObject.GetComponentInParent<COptionPenel>(true);
        if (option != null)
        {
            option.UpdatePauseRequest();
        }
    }
}
