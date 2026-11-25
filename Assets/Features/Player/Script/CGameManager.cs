using System;
using UnityEngine;

public enum EGameState
{
    Playing,
    GameOver,
    Clear
}

public class CGameManager : MonoBehaviour
{
    #region 내부 변수
    private static CGameManager _instance;
    private EGameState _currentState = EGameState.Playing;
    #endregion

    #region 프로퍼티
    public static CGameManager Instance => _instance;
    public EGameState CurrentState => _currentState;
    public bool IsPlaying => _currentState == EGameState.Playing;
    public bool IsGameOver => _currentState == EGameState.GameOver;
    #endregion

    #region 이벤트
    public event Action<EGameState> OnStateChanged;
    public event Action OnGameOver;
    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        _instance = this;
        InitializeGame();
    }

    private void OnDestroy()
    {
        if (_instance != this)
        {
            return;
        }

        _instance = null;
        Time.timeScale = 1.0f;
    }

    // 월드 초기화가 끝난 뒤 호출. HP와 인벤토리 등은 초기화하지 않음
    public void InitializeGame()
    {
        if (_instance != this)
        {
            return;
        }

        _currentState = EGameState.Playing;
        Time.timeScale = 1.0f;
        OnStateChanged?.Invoke(_currentState);
    }

    public void GameOver()
    {
        if (_instance != this || !IsPlaying)
        {
            return;
        }

        _currentState = EGameState.GameOver;
        Time.timeScale = 0.0f;
        OnStateChanged?.Invoke(_currentState);
        OnGameOver?.Invoke();
    }
}
