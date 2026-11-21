using UnityEngine;
using UnityEngine.SceneManagement;

public class CProjectilePool : CObjectPool<CProjectilePool, CProjectile>
{
    #region 프로퍼티
    public CProjectile Prefab => _prefab;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        if (!enabled || Instance != this)
        {
            return;
        }
         
        Prewarm();
        _poolRoot.SetParent(transform);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 이전 씬에서 발사한 탄환은 다음 씬으로 가져가지 않음
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Single)
        {
            return;
        }

        for (int i = _liveList.Count - 1; i >= 0; i--)
        {
            if (_liveList[i] == null)
            {
                _liveList.RemoveAt(i);
            }
            else
            {
                _liveList[i].ReturnToPool();
            }
        }
    }

    protected override void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        for (int i = 0; i < _liveList.Count; i++)
        {
            if (_liveList[i] != null)
            {
                Destroy(_liveList[i].gameObject);
            }
        }

        _liveList.Clear();

        while (_pool.Count > 0)
        {
            CProjectile projectile = _pool.Dequeue();

            if (projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }

        base.OnDestroy();
    }
}
