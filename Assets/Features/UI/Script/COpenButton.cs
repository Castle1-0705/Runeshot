using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COpenButton : MonoBehaviour
{
    [SerializeField] GameObject _openObject;
    public void Open()
    {
        _openObject.SetActive(true);
    }

    public void Toggle()
    {
        if (_openObject.activeSelf)
        {
            _openObject.SetActive(false);
        }
        else
        {
            _openObject.SetActive(true);
        } 
    }
}
