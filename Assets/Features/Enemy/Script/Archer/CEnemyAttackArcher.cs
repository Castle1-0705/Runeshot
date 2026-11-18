using Cysharp.Threading.Tasks; 
using System.Threading; 
using UnityEngine;
using UnityEngine.AI; 

public class CEnemyAttackArcher : CEnemyAttack
{
    CWeapon _rangeAttack;
    public CEnemyAttackArcher(CEnemySetting enemySetting, int num) : base(enemySetting, num)
    {
        _rangeAttack = enemySetting._weapon;
    }
  
    public override void ActionUpdate(Transform PlayerTr)
    {

        if (PlayerTr == null || _playerHP == null)
        {
            return;
        }
         
        Flip(PlayerTr.position);

         _time += Time.deltaTime;

        if( _time < _attackInterval )
        {
            return;
        }
       
        _rangeAttack.TryFire(PlayerTr.position);
        //_playerHP.OnHit(_damage);

        _ani.SetTrigger(_attack);
        _time = 0;

    }
    public override void ActionExit()
    {
        
        Debug.Log("Exit Attack");
        _time = 0;
    }
     
     
}
 
