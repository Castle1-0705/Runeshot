using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class CEnemyPhaseRange : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private SpriteShapeController _border;
    [SerializeField] private SpriteShapeController _fill;
    [SerializeField] private PolygonCollider2D _col;
    [SerializeField] private CEnemyAttackBossPhase2SO _phase; 
    #endregion

    #region 내부 변수 
    private float _tangentRange; 
    #endregion

    
    private void Awake()
    {
        _tangentRange = _phase._range * 0.55228f;

        SetControll(_border, _phase._range);
        SetControll(_fill, _phase._range);

        _border.BakeMesh();
        _border.BakeCollider();
        _border.gameObject.SetActive(false);
    }
    private void SetControll(SpriteShapeController controller, float range)
    {
        for(int i = 0; i < controller.spline.GetPointCount(); i++)
        {
            switch (i)
            {
                case 0:
                    controller.spline.SetPosition(i, Vector2.up* range);
                    controller.spline.SetLeftTangent(i, Vector2.right* _tangentRange);
                    controller.spline.SetRightTangent(i, Vector2.left * _tangentRange); 
                    break;
                case 1:
                    controller.spline.SetPosition(i, Vector2.left * range);
                    controller.spline.SetLeftTangent(i, Vector2.up * _tangentRange);
                    controller.spline.SetRightTangent(i, Vector2.down * _tangentRange); 
                    break;
                case 2:
                    controller.spline.SetPosition(i, Vector2.down * range);
                    controller.spline.SetLeftTangent(i, Vector2.left * _tangentRange);
                    controller.spline.SetRightTangent(i, Vector2.right * _tangentRange); 
                    break;
                case 3:
                    controller.spline.SetPosition(i, Vector2.right * range);
                    controller.spline.SetLeftTangent(i, Vector2.down * _tangentRange);
                    controller.spline.SetRightTangent(i, Vector2.up * _tangentRange); 
                    break;
            }

        }
        
    }
}
