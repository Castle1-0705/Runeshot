using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyHand : MonoBehaviour
{
    [SerializeField] private CEnemy _enemy;

    private void Awake()
    {
        if( _enemy == null )
        {
            Debug.Log("CEnemyHand의 Enemy가 비어있다");
            _enemy = transform.parent.GetComponent<CEnemy>();
        }
    }
    void Update()
    {
        if( _enemy != null )
        {
            if(_enemy.PlayerTr != null)
            {
                transform.LookAt(_enemy.PlayerTr);
            }
        }
    }
}
