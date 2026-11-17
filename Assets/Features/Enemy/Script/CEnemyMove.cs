using UnityEngine;
using UnityEngine.AI; 

public class CEnemyMove : CEnemyActionBase
{
    
    #region 내부변수 
    private string _move = "IMove";
    
    private float _time;
    private float _waitTime = 3f;
    private Vector3 _setPosition;  
    private Vector3 _prePlayerPosision;
    #endregion
     
    public CEnemyMove(CEnemySetting enemySetting, int num): base(enemySetting, num)
    {  
        _nav.speed = enemySetting._enemy.Enemydata.MovementSpeed;
        _setPosition = enemySetting.transform.position;
    }
    
    public override bool Condition(Transform PlayerTr)
    {
        if(PlayerTr == null)
        {
            return false;
        }
         
        if ((PlayerTr.position - _tr.position).sqrMagnitude > _enemy.Enemydata.AttackRange * _enemy.Enemydata.AttackRange)
        {
            return true;
        }

        return false;
    }
    public override void ActionEnter(Transform PlayerTr)
    {
        Debug.Log("Enter Move");

        _nav.stoppingDistance = _enemy.Enemydata.AttackRange;

        _ani.SetInteger(_move, 1);
    }
    public override void ActionUpdate(Transform PlayerTr)
    { 
        if (_nav.speed != 0f)
        {
            float speed = _nav.velocity.sqrMagnitude / (_nav.speed * _nav.speed);

            if (speed == 0)
            {
                _time += Time.deltaTime;
            }

            _ani.speed = speed == 0 ? 1f : speed;
        }

        if (_prePlayerPosision == PlayerTr.position)
        {
            if (_time <= _waitTime)
            {
                return;
            }
           
        }
         
        _prePlayerPosision = PlayerTr.position;


        if (!_nav.pathPending)
        {
            if (_time <= _waitTime)
            {
                _nav.destination = PlayerTr.position;

            }
            else
            {
                _time = 0f;
                _nav.stoppingDistance = 0;
                _nav.destination = _setPosition; 
            }
        }

        Flip(_nav.destination);
         
    }
  
    public override void ActionExit()
    {
        _ani.SetInteger(_move, 0);
        _ani.speed = 1f;
        _time = 0f;
        _nav.stoppingDistance = _enemy.Enemydata.AttackRange;
        Debug.Log("Exit Move");   
    } 
}
