using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CEnemyActionBaseSO : ScriptableObject
{
    [SerializeField] public string _id;
    [SerializeField] public int _priority;
    public abstract CEnemyActionBase Create(CEnemySetting enemySetting);

}
