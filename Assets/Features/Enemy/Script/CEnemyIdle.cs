using UnityEngine;

public class CEnemyIdle : CEnemyActionBase
{ 
    #region 내부변수 
    private string _move = "IMove";
    private Vector3 _setPosition;
    #endregion
     
    public CEnemyIdle(CEnemySetting enemySetting, int num) : base(enemySetting, num)
    {  
        _setPosition = enemySetting.transform.position;
    }
      
    public override bool Condition(Transform PlayerTr)
    {
        if (PlayerTr == null)
        {
            float distance = (_setPosition - _tr.position).sqrMagnitude;

            if (distance < 0.1f)
            {
                return true;
            }

        }

        return false;
    }
    public override void ActionEnter(Transform PlayerTr)
    {
        Debug.Log("Enter Idle");
         
        _ani.SetInteger(_move, 0);
    }
 
    public override void ActionExit()
    {
        Debug.Log("Exit Idle"); 
    }
     
}
