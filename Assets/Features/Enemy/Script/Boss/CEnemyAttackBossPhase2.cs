 using UnityEngine; 
 
public class CEnemyAttackBossPhase2 : CEnemyAttackBossPhaseBase
{ 
    private Collider2D _rangeCol;
    private Transform _colTr;
     
    private Collider2D[] collider2Ds = new Collider2D[1];
    ContactFilter2D contactFilter;
    private LayerMask _layer;
    private string _stringPlayer = "Player";
      
    public CEnemyAttackBossPhase2(CEnemySetting enemySetting, int num, float range, float damageModifier) : base(enemySetting, num)
    {
        
        _rangeCol = enemySetting._poCol;
        _colTr = enemySetting._coltr;

        _sqrRange = range*range;

        _damageModifier = damageModifier;

        InitObject(); 
    }

    private void InitObject()
    {
        _phaseInterval = 11f;
        _chargingTime = 3f;
        _phaseHPPercent = 0.8f;

        _layer = 1 << LayerMask.NameToLayer(_stringPlayer);
        contactFilter.layerMask = _layer;
        contactFilter.useLayerMask = true;
         
        _colTr.localScale = Vector3.zero;
    }
    
    public override bool Condition(Transform PlayerTr)
    { 
        if (PlayerTr == null)
        {
            return false;
        }

        if(_isPhaseAttack)
        {
            return true;
        }
         
        if (_isOtherPhaseAttacking)
        {
            return false;
        }

        if ((PlayerTr.position - _tr.position).sqrMagnitude < _sqrRange)
        {
            if(_enemyHP.HP < _enemyHP.MaxHP* _phaseHPPercent)
            {  
                if(_isPhaseInterval)
                {  
                    _time += Time.deltaTime;

                    if (_time < _phaseInterval)
                    { 
                        return false;
                    }
                    else
                    {
                        _time = 0;
                        _isPhaseInterval = false;
                    }
                    
                }

                _isPhaseAttack = true;
                return true;
            } 
        }

        return false;
    }

    public override void ActionEnter(Transform PlayerTr)
    {
        base.ActionEnter(PlayerTr);

        _rangeCol.gameObject.SetActive(true);

        _ani.SetInteger("IMove", 0);

        _isOtherPhaseAttacking = _isPhaseAttack;
    }

    public override void ActionUpdate(Transform PlayerTr)
    {
        if (PlayerTr == null || _playerHP == null)
        {
            return;
        }
         
        if(_chargingTime != 0f)
        {
            _colTr.localScale = new Vector2(_time / _chargingTime, _time / _chargingTime);
        }
          
        _time += Time.deltaTime;
         
        if (_time < _chargingTime)
        {
            return;
        }
        
        Physics2D.OverlapCollider(_rangeCol, contactFilter, collider2Ds); 

        if (collider2Ds[0] != null)
        {
            if(collider2Ds[0].tag == _stringPlayer)
            {
                _playerHP.OnHit(_damage* _damageModifier);
                collider2Ds[0] = null;
            } 
        }

        _phaseInterval = _phaseInterval + Random.Range(-2f, 2f);
        _chargingTime = _chargingTime + Random.Range(-1f, 1f);
        _phaseInterval = Mathf.Clamp(_phaseInterval, 4f, 18f);
        _chargingTime = Mathf.Clamp(_chargingTime, 2f, 4f);

        _ani.SetTrigger(_attack);

        _isPhaseAttack = false;
        _isPhaseInterval = true;
        _time = 0;

        _isOtherPhaseAttacking = _isPhaseAttack;
    }
    
    public override void ActionExit()
    {
        base.ActionExit();
         
        _colTr.localScale = Vector3.zero;
        _rangeCol.gameObject.SetActive(false); 
    }

    //안쓰는 것
    public void SetRange(Collider2D col)
    {
        if (col.bounds.extents.y < col.bounds.extents.x)
        {
            _sqrRange = col.bounds.extents.y * col.bounds.extents.y;
        }
        else
        {
            _sqrRange = col.bounds.extents.x * col.bounds.extents.x;
        }
    }
}
