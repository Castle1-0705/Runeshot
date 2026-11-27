using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

public class CHouseExit : MonoBehaviour
{
    private Transform _player;
    private bool _isPlayerNear;

    private void Update()
    {
        if (!_isPlayerNear)
        {
            return;
        }

        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            ExitHouse();
        }
    }

    private void ExitHouse()
    {
        CGameManager gameManager = CGameManager.Instance;

        if (gameManager == null || !gameManager.CanRunCombat)
        {
            return;
        }

        if (_player == null || !CMapReturnData.HasReturnPosition)
        {
            return;
        }

        if (CMapReturnData.CurrentInterior != null)
        {
            CMapReturnData.CurrentInterior.ClearInterior();
        }

        _player.position = CMapReturnData.ReturnPosition;

        CMapReturnData.Reset();

        _isPlayerNear = false;
        _player = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            _isPlayerNear = true;
            _player = other.transform.root;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            _isPlayerNear = false;
            _player = null;
        }
    }
}
