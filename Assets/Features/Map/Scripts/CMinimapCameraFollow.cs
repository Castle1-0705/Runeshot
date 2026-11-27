using UnityEngine;

public class CMinimapCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private void LateUpdate()
    {
        if (_player == null)
        {
            return;
        }

        Vector3 position = transform.position;

        position.x = _player.position.x;
        position.y = _player.position.y;

        transform.position = position;
    }
}