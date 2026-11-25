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
    private bool _isChestPaused;
    private bool _isOptionPaused;
    #endregion

    #region 프로퍼티
    public static CGameManager Instance => _instance;
    public EGameState CurrentState => _currentState;
    public bool IsPlaying => _currentState == EGameState.Playing;
    public bool IsGameOver => _currentState == EGameState.GameOver;
    public bool CanRunCombat => IsPlaying && !_isChestPaused && !_isOptionPaused && Time.timeScale > 0.0f;
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
        _isChestPaused = false;
        _isOptionPaused = false;
        UpdateTimeScale();
        OnStateChanged?.Invoke(_currentState);
    }

    public void GameOver()
    {
        if (_instance != this || !IsPlaying)
        {
            return;
        }

        _currentState = EGameState.GameOver;
        UpdateTimeScale();
        OnStateChanged?.Invoke(_currentState);
        OnGameOver?.Invoke();
    }

    public void SetChestPaused(bool isPaused)
    {
        if (_instance != this)
        {
            return;
        }

        _isChestPaused = isPaused;
        UpdateTimeScale();
    }

    public void SetOptionPaused(bool isPaused)
    {
        if (_instance != this)
        {
            return;
        }

        _isOptionPaused = isPaused;
        UpdateTimeScale();
    }

    private void UpdateTimeScale()
    {
        if (!IsPlaying || _isChestPaused || _isOptionPaused)
        {
            Time.timeScale = 0.0f;
            return;
        }

        Time.timeScale = 1.0f;
    }
}
