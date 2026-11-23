using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CSoundManager : CSingletonDontDstroyAtLoad<CSoundManager>
{ 
    [SerializeField] private AudioMixerGroup _master;
    [SerializeField] private AudioMixerGroup _backGround;
    [SerializeField] private AudioMixerGroup _sfx;
    [SerializeField] private AudioMixerGroup _ui;
   
    public AudioMixerGroup Master => _master;
    public AudioMixerGroup BackGround => _backGround;
    public AudioMixerGroup SFX => _sfx; 
    public AudioMixerGroup UI => _ui;

    Transform _cameraTr;

    protected override void Awake()
    {
        base.Awake();
        SceneManager.sceneLoaded += CSoundControllerBackground.PlayBackground;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        SceneManager.sceneLoaded -= CSoundControllerBackground.PlayBackground;
    }
    private void Start()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            _cameraTr = mainCamera.transform;
        }
    }

    private void Update()
    {
        if (_cameraTr == null)
        {
            Camera mainCamera = Camera.main;

            if (mainCamera == null)
            {
                return;
            }

            _cameraTr = mainCamera.transform;
        }

        transform.position = _cameraTr.position;
    }
}

