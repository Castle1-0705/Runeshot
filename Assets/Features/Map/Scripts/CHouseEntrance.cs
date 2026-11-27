using UnityEngine.InputSystem;
using UnityEngine;

public class CHouseEntrance : MonoBehaviour
{
    [Header("집 설정")]
    [SerializeField] private Transform _interiorSpawnPoint;

    private Transform _player;
    private bool _isPlayerNear;

    private void Update()
    {
        if (!_isPlayerNear)
        {
            return;
        }

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            EnterHouse();
        }
    }

    private void EnterHouse()
    {
        CGameManager gameManager = CGameManager.Instance;

        if (gameManager == null || !gameManager.CanRunCombat)
        {
            return;
        }

        if (_player == null || _interiorSpawnPoint == null)
        {
            return;
        }

        CMapReturnData.ReturnPosition = transform.position;
        CMapReturnData.HasReturnPosition = true;

        _player.position = _interiorSpawnPoint.position;

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