using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;
using static Unity.VisualScripting.Member;

public class CEnemySetting : MonoBehaviour
{
    //기본
    [SerializeField] public CEnemy _enemy;
    [SerializeField] public NavMeshAgent _navAgent;
    [SerializeField] public Animator _ani;
    [SerializeField] public Collider2D _col;
    [SerializeField] public CEnemyController _controll; 
     
    //원거리
    [SerializeField] public CWeapon _weapon; 
    [SerializeField] public Transform _muzzle;

    //보스
    [SerializeField] public SpriteRenderer _meleeAttack;
    [SerializeField] public SpriteRenderer _rangeAttack;

    //보스 패턴
    [SerializeField] public PolygonCollider2D _poCol;
    [SerializeField] public Transform _coltr;
    [SerializeField] public Transform _point;

    public CancellationTokenSource _source;
     
    public CancellationTokenSource TokenGen()
    {
        _source = new CancellationTokenSource();

        return _source;
    }
     
    public void TokenCancel()
    {
        if (_source != null)
        {
            _source.Cancel();
            _source.Dispose();
            _source = null;
        }
    }
     
    private void OnDestroy()
    {
        TokenCancel();
    }
}
