using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CCloseButton : MonoBehaviour
{
    [SerializeField] Button _closeButton;
     
    public void CloseTimeScale1()
    {
        transform.parent.gameObject.SetActive(false);

        TimeScale();
    }
    public void Close()
    {
        transform.parent.gameObject.SetActive(false); 
    }

    public void TimeScale()
    {
        Time.timeScale = 1.0f;
    }
}
