using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemySummonChest : MonoBehaviour
{
    #region ¿ŒΩ∫∆Â≈Õ 
    [SerializeField] private GameObject _chestPrifab;
    [SerializeField][Range(0,100)] private int _percentage;
    [SerializeField] private bool _togleIfDestroy;
    [SerializeField][Range(0f,100f)] private float _destroyTime; 
    #endregion 

    public void SummonChest(Transform transform)
    {
        int ranNum = Random.Range(0,100);
         
        if(ranNum < _percentage)
        {
            GameObject chestClone = Instantiate(_chestPrifab, transform.position,transform.rotation);
            DestroyChest(chestClone); 
        }
    }

    private void DestroyChest(GameObject chestClone)
    {
        if(_togleIfDestroy)
        {
            Destroy(chestClone, _destroyTime);
        }
    }
}
