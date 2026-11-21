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
        if (_player == null || !CMapReturnData.HasReturnPosition)
        {
            return;
        }

        _player.position = CMapReturnData.ReturnPosition;

        CMapReturnData.HasReturnPosition = false;

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
