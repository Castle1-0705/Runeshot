using UnityEngine;
using UnityEngine.AI;

public class CEnemySpawnManager : MonoBehaviour
{
    [Header("적 설정")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _maxEnemyCount = 8;

    [Header("플레이어 설정")]
    [SerializeField] private Transform _player;
    [SerializeField] private Camera _mainCamera;

    [Header("스폰 시간")]
    [SerializeField] private float _spawnIntervalMin = 3f;
    [SerializeField] private float _spawnIntervalMax = 5f;

    [Header("스폰 위치")]
    [SerializeField] private float _spawnOutsideDistance = 1.5f;
    [SerializeField] private float _minPlayerDistance = 4f;

    [Header("적 제거 설정")]
    [SerializeField] private float _enemyRemoveDistance = 20f;

    [Header("내비메쉬 설정")]
    [SerializeField] private float _navMeshSearchRadius = 2f;
    [SerializeField] private int _spawnTryCount = 15;

    [Header("설정")]
    [SerializeField] private CProjectilePool _projectilePool;

    private GameObject[] _enemyPool;

    private float _spawnTimer;
    private float _nextSpawnTime;

    private Vector3 _spawnPosition;


    private void Start()
    {
        if (_maxEnemyCount < 1)
        {
            _maxEnemyCount = 1;
        }

        _enemyPool = new GameObject[_maxEnemyCount];

        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        if (_player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                _player = playerObject.transform;
            }
        }

        SetNextSpawnTime();
    }

    private void Update()
    {
        if (_enemyPrefab == null || _player == null || _mainCamera == null)
        {
            return;
        }

        RemoveFarEnemies();

        int enemyCount = CountEnemy();

        if (enemyCount >= _maxEnemyCount)
        {
            return;
        }

        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= _nextSpawnTime)
        {
            _spawnTimer = 0f;

            SetNextSpawnTime();

            if (FindSpawnPosition())
            {
                CreateEnemy();
            }
        }
    }

    private void SetNextSpawnTime()
    {
        _nextSpawnTime = Random.Range(
            _spawnIntervalMin,
            _spawnIntervalMax
        );
    }

    private void RemoveFarEnemies()
    {
        for (int i = 0; i < _enemyPool.Length; i++)
        {
            if (_enemyPool[i] == null || !_enemyPool[i].activeSelf)
            {
                continue;
            }

            float distance = Vector2.Distance(
                _player.position,
                _enemyPool[i].transform.position
            );

            if (distance > _enemyRemoveDistance)
            {
                _enemyPool[i].SetActive(false);
            }
        }
    }

    private int CountEnemy()
    {
        int enemyCount = 0;

        for (int i = 0; i < _enemyPool.Length; i++)
        {
            if (_enemyPool[i] != null && _enemyPool[i].activeSelf)
            {
                enemyCount++;
            }
        }

        return enemyCount;
    }

    private bool FindSpawnPosition()
    {
        float halfHeight = _mainCamera.orthographicSize;
        float halfWidth = halfHeight * _mainCamera.aspect;

        float cameraX = _mainCamera.transform.position.x;
        float cameraY = _mainCamera.transform.position.y;

        for (int i = 0; i < _spawnTryCount; i++)
        {
            float randomX = Random.Range(-halfWidth, halfWidth);
            float randomY = Random.Range(-halfHeight, halfHeight);

            int side = Random.Range(0, 4);

            Vector3 candidate = Vector3.zero;

            if (side == 0)
            {
                candidate.x = cameraX - halfWidth - _spawnOutsideDistance;
                candidate.y = cameraY + randomY;
            }
            else if (side == 1)
            {
                candidate.x = cameraX + halfWidth + _spawnOutsideDistance;
                candidate.y = cameraY + randomY;
            }
            else if (side == 2)
            {
                candidate.x = cameraX + randomX;
                candidate.y = cameraY - halfHeight - _spawnOutsideDistance;
            }
            else
            {
                candidate.x = cameraX + randomX;
                candidate.y = cameraY + halfHeight + _spawnOutsideDistance;
            }

            candidate.z = _player.position.z;

            NavMeshHit hit;

            if (!NavMesh.SamplePosition(
                candidate,
                out hit,
                _navMeshSearchRadius,
                NavMesh.AllAreas))
            {
                continue;
            }

            Vector3 checkPosition = hit.position;

            if (checkPosition.x >= cameraX - halfWidth &&
                checkPosition.x <= cameraX + halfWidth &&
                checkPosition.y >= cameraY - halfHeight &&
                checkPosition.y <= cameraY + halfHeight)
            {
                continue;
            }

            float distance = Vector2.Distance(
                _player.position,
                checkPosition
            );

            if (distance < _minPlayerDistance)
            {
                continue;
            }

            _spawnPosition = checkPosition;
            _spawnPosition.z = _player.position.z;

            return true;
        }

        return false;
    }

    private void CreateEnemy()
    {
        for (int i = 0; i < _enemyPool.Length; i++)
        {

            if (_enemyPool[i] == null)
            {
                CWeapon weapon = _enemyPool[i].GetComponentInChildren<CWeapon>(true);

                if (weapon != null)
                {
                    weapon.SetProjectilePool(_projectilePool);
                }
            }

            if (!_enemyPool[i].activeSelf)
            {
                _enemyPool[i].transform.position = _spawnPosition;
                _enemyPool[i].transform.rotation = Quaternion.identity;

                _enemyPool[i].SetActive(true);

                return;
            }
        }
    }
}
