using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class CEnemySetting : MonoBehaviour
{
    [SerializeField] public CEnemy _enemy;
    [SerializeField] public NavMeshAgent _navAgent;
    [SerializeField] public Animator _ani;
    [SerializeField] public Collider2D _col;
    [SerializeField] public CEnemyController _controll; 

    [SerializeField] public SpriteRenderer _meleeAttack;
    [SerializeField] public SpriteRenderer _rangeAttack;
     
}
