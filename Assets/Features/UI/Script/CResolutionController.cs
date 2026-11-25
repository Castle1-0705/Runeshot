using System;
using System.Collections;
using System.Collections.Generic;
using TMPro; 
using UnityEngine;
using UnityEngine.UI;

public class CResoultionController : MonoBehaviour
{
    #region 인스펙터  
    [SerializeField] private TMP_Dropdown _dropdown;
    [SerializeField] private TMP_Dropdown _dropdownFullScreen; 
    [SerializeField] private Button _resolutionButton;
    #endregion

    #region 내부 변수 
    private bool _fullscreen;
    private int _num = 0;
    #endregion

    private void Awake()
    {
        if (_dropdown == null || _dropdownFullScreen == null || _resolutionButton == null)
        {
            Debug.Log("해상도 옵션 인스펙터 확인");
            return;
        }

        _dropdown.onValueChanged.AddListener(ResolutionNumChange);

        _dropdownFullScreen.onValueChanged.AddListener(IsFullScreen);
          
        _resolutionButton.onClick.AddListener(ResolutionChange);

        gameObject.SetActive(false);
    }

    public void IsFullScreen(int num)
    {
        switch (num)
        {
            case 0:
                _fullscreen = false;
                break;
            case 1:
                _fullscreen = true;
                break;
        }

        ResolutionChange();
    }

    public void ToggleFullscreen()
    {
        _fullscreen = !_fullscreen;

        ResolutionChange();
    }

    public void ResolutionNumChange(int num)
    {
        _num = num;
    }

    public void ResolutionChange()
    {
        switch (_num)
        {
            case 0:
                Screen.SetResolution(1280,720, _fullscreen);
                break;
            case 1:
                Screen.SetResolution(1600, 900, _fullscreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, _fullscreen);
                break;
            case 3:
                Screen.SetResolution(2560, 1440, _fullscreen);
                break;
            case 4:
                Screen.SetResolution(3840, 2160, _fullscreen);
                break;
        }

    }
}
