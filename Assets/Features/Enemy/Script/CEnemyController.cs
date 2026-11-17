using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CEnemyController : MonoBehaviour
{
    #region 인스펙터 
    [SerializeField] private CEnemy _enemy;
    [SerializeField] private CEnemyFSM _enemyFSM;
    [SerializeField] private CEnemySetting _enemySetting;
    [SerializeField] List<CEnemyActionBaseSO> _enemyActionSOList;
    #endregion

    #region 내부변수   
    private Dictionary<int, CEnemyActionBase> _enemyActionDictionary = new Dictionary<int, CEnemyActionBase>(10); 
    #endregion
    private void Awake()
    {
        if (_enemyActionSOList.Count == 0)
        {
            Debug.Log("적 행동이 없음");
            return;
        }
         
       IniDictionary(); 
    }
    private void OnDisable()
    {
        foreach (var dic in _enemyActionDictionary)
        {
            _enemyActionDictionary[dic.Key].OnDisable();
        }
    }

    private void Update()
    {
        if (!_enemy.IsAlive)
        {
            return;
        }
         
        SetControll();

       _enemyFSM.Action(); 
    }

    private void IniDictionary()
    { 
        for (int i = 0; i < _enemyActionSOList.Count; i++)
        {
            if (_enemyActionSOList[i] == null)
            {
                Debug.Log($"_enemyActionSOList{i}가 null");
                continue;
            }

            CEnemyActionBase enemyActionBase = _enemyActionSOList[i].Create(_enemySetting);
            _enemyActionDictionary.Add(_enemyActionSOList[i]._num, enemyActionBase);
        }
    }
    public void SetControll(int action)
    {
        if(_enemyActionDictionary.ContainsKey(action))
        {
            _enemyFSM.SetControll(_enemyActionDictionary[action]);
        } 
    }

    private void SetControll()
    { 
        int num = -1;

        foreach (var dic in _enemyActionDictionary)
        {
            if (_enemyActionDictionary[dic.Key].Condition(_enemy.PlayerTr))
            { 
                if (num < dic.Key)
                {
                    num = dic.Key;
                } 
            }
        }
         
        if(num < 0)
        { 
            return;
        }
         
        SetControll(num);
    }

    
    
}
