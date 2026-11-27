using UnityEngine;

public class CHouseInteriorController : MonoBehaviour
{
    #region 인스펙터
    [Header("상자 설정")]
    [SerializeField] private GameObject _chestPrefab;
    [SerializeField] private Transform[] _lootSpawnPoints;
    #endregion

    #region 내부 변수
    private GameObject _spawnedChest;
    #endregion

    public bool SpawnChest()
    {
        if (_spawnedChest != null)
        {
            return false;
        }

        if (_chestPrefab == null ||
            _lootSpawnPoints == null ||
            _lootSpawnPoints.Length == 0)
        {
            return false;
        }

        int index = Random.Range(0, _lootSpawnPoints.Length);
        Transform spawnPoint = _lootSpawnPoints[index];

        if (spawnPoint == null)
        {
            return false;
        }

        _spawnedChest = Instantiate(
            _chestPrefab,
            spawnPoint.position,
            Quaternion.identity,
            transform);

        return true;
    }

    public void ClearInterior()
    {
        if (_spawnedChest != null)
        {
            _spawnedChest.SetActive(false);
            Destroy(_spawnedChest);
            _spawnedChest = null;
        }
    }
}