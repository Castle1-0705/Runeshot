using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T _instance = null;  
    private static bool _isDestroyed = false;

    #region 프로퍼티
    public static bool IsDestroyed => _isDestroyed;
    #endregion

    public static T Instance
    {
        get
        {
            if(_isDestroyed)
            {
                return null;
            }
            if (_instance == null)
            {
                var go = new GameObject(typeof(T).ToString());
                _instance = go.AddComponent<T>();
                Debug.Log($"[CSingleton] {typeof(T).ToString()} 인스턴스가 존재하지 않아 새로 생성되었습니다.");
            }

            return _instance;
        }
    }

    public static T Create()
    {
        return CSingleton<T>.Instance;
    }

    protected virtual void Awake()
    {
        if (_instance == null && _instance != this)
        {
            _instance = this as T;
            // 실제 새 인스턴스가 등록되면 다시 정상 조회 가능
            _isDestroyed = false;
        }

        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if(_instance == this)
        {
            _instance = null;
            _isDestroyed = true;
        }
    }
}
