using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

public class CHouseEntrance : MonoBehaviour
{
    [Header("씬 설정")]
    [SerializeField] private string _sceneName;
    [SerializeField] private string _houseID;

    private bool _isPlayerNear;
    private bool _isLoading;

    private void Update()
    {
        if (!_isPlayerNear || _isLoading)
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
        if (!Application.CanStreamedLevelBeLoaded(_sceneName))
        {
            return;
        }

        CMapReturnData.ReturnPosition = transform.position;
        CMapReturnData.HasReturnPosition = true;

        CHouseLootData.SetCurrentHouse(_houseID);

        _isLoading = true;

        SceneManager.LoadScene(_sceneName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            _isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            _isPlayerNear = false;
        }
    }
}
