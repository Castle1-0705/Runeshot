using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CSoundController<T> : MonoBehaviour
{
    private static CSoundController<T> _instance;

    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private CSoundSO _soundSO;

    private Action _soundAction;

    private static bool _isDestroyed = false;

    #region 프로퍼티
    public CSoundSO SoundSO => _soundSO;
    public AudioSource AudioSource { get { return _audioSource; } set { _audioSource = value; } } 
    public static bool IsDestroyed => _isDestroyed;
    #endregion

    public static CSoundController<T> Instance
    {
        get
        {
            if (_isDestroyed)
            {
                return null;
            }
            if (_instance == null)
            {
                var go = new GameObject(typeof(T).ToString());
                _instance = go.AddComponent<CSoundController<T>>();
                Debug.Log($"[CSingleton] {typeof(T).ToString()} 인스턴스가 존재하지 않아 새로 생성되었습니다.");
            }

            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance == null && _instance != this)
        {
            _instance = this as CSoundController<T>; 

            _isDestroyed = false;

            _audioSource = GetComponent<AudioSource>();
        }

        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
            _isDestroyed = true;
        }
    }
     
    public static void RegisterSound(AudioClip clip)
    {
        Instance._soundAction = null;

        Instance._soundAction = () => Instance.AudioSource.PlayOneShot(clip);
    }

    public static void PlaySound()
    {
        Instance._soundAction?.Invoke();
    }
    public static void PlaySound(AudioClip clip)
    {
        Instance.AudioSource.PlayOneShot(clip);
    }

    public static void PlaySound(string name)
    {
        AudioClip clip;

        if(Instance.SoundSO.Clips.TryGetValue(name, out clip))
        {
            Instance.AudioSource.PlayOneShot(clip);
        }
    }
    public static void PlayOtherBackGround(AudioClip clip)
    {
        Instance.AudioSource.clip = clip;

        Instance.AudioSource.Play();
    }
}
