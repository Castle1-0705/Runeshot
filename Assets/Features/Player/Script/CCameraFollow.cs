using UnityEngine;

public class CCameraFollow : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private Transform _target;
    #endregion

    // 씬 로드 후 유지된 Player를 추적 대상으로 연결
    public void SetTarget(Transform target)
    {
        _target = target;
        enabled = _target != null;
    }

    private void Start()
    {
        if (_target == null)
        {
            Debug.LogError("카메라 타겟이 비어있음");
            enabled = false;
            return;
        }
    }

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
    }
}
