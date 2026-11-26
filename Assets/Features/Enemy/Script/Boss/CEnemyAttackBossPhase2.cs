 using UnityEngine; 
 
public class CEnemyAttackBossPhase2 : CEnemyAttack
{
    private CEnemyHPHandler _enemyHP;
    private Collider2D _rangeCol;
    private Transform _colTr;
     
    private Collider2D[] collider2Ds = new Collider2D[1];
    ContactFilter2D contactFilter;
    private LayerMask _layer;
    private string _stringPlayer = "Player";
     
    private float _phaseHPPercent = 0.6f;
    private float _sqrRange;

    private bool _isPhaseInverval = false;
    private float _phaseInterval = 10f; 
    private bool _isPhaseAttack = false;
    private float _chargingTime = 3f;
      
    public CEnemyAttackBossPhase2(CEnemySetting enemySetting, int num, float range) : base(enemySetting, num)
    {
        _enemyHP = enemySetting.GetComponent<CEnemyHPHandler>();
        _rangeCol = enemySetting._poCol;
        _colTr = enemySetting._coltr;

        _sqrRange = range*range;

        InitObject(); 
    }

    private void InitObject()
    {
        _layer = 1 << LayerMask.NameToLayer(_stringPlayer);
        contactFilter.layerMask = _layer;
        contactFilter.useLayerMask = true;
         
        _colTr.localScale = Vector3.zero;
    }
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
          
        if ((PlayerTr.position - _tr.position).sqrMagnitude < _sqrRange)
        {
            if(_enemyHP.HP < _enemyHP.MaxHP* _phaseHPPercent)
            {  
                if(_isPhaseInverval)
                {  
                    _time += Time.deltaTime;

                    if (_time < _phaseInterval)
                    { 
                        return false;
                    }
                    else
                    {
                        _time = 0;
                        _isPhaseInverval = false;
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
                _playerHP.OnHit(_damage);
                collider2Ds[0] = null;
            } 
        }

        _ani.SetTrigger(_attack);

        _isPhaseAttack = false;
        _isPhaseInverval = true;
        _time = 0; 
    }
    
    public override void ActionExit()
    {
        base.ActionExit();

        _colTr.localScale = Vector3.zero;
        _rangeCol.gameObject.SetActive(false); 
    }
}
