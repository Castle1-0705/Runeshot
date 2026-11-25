using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCloseButton : MonoBehaviour
{
    [SerializeField] GameObject _closeObject;
     
    public void CloseTimeScale1()
    { 
        _closeObject.SetActive(false);

        TimeScale();
    }
    public void Close()
    {
        _closeObject.SetActive(false);
    }

    public void TimeScale()
    {
        Time.timeScale = 1.0f;
    }
}
