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
    [SerializeField] private float _splineRange = 4f; 
    #endregion

    #region 내부 변수 
    private float _tangentRange; 
    #endregion

    private void Awake()
    {
        _tangentRange = _splineRange * 0.55228f;

        SetControll(_border);
        SetControll(_fill);

        _border.BakeMesh();
        _border.BakeCollider();
        _border.gameObject.SetActive(false);
    }
    private void SetControll(SpriteShapeController controller)
    {
        for(int i = 0; i < controller.spline.GetPointCount(); i++)
        {
            switch (i)
            {
                case 0:
                    controller.spline.SetPosition(i, Vector2.up* _splineRange);
                    controller.spline.SetLeftTangent(i, Vector2.right* _tangentRange);
                    controller.spline.SetRightTangent(i, Vector2.left * _tangentRange); 
                    break;
                case 1:
                    controller.spline.SetPosition(i, Vector2.left * _splineRange);
                    controller.spline.SetLeftTangent(i, Vector2.up * _tangentRange);
                    controller.spline.SetRightTangent(i, Vector2.down * _tangentRange); 
                    break;
                case 2:
                    controller.spline.SetPosition(i, Vector2.down * _splineRange);
                    controller.spline.SetLeftTangent(i, Vector2.left * _tangentRange);
                    controller.spline.SetRightTangent(i, Vector2.right * _tangentRange); 
                    break;
                case 3:
                    controller.spline.SetPosition(i, Vector2.right * _splineRange);
                    controller.spline.SetLeftTangent(i, Vector2.down * _tangentRange);
                    controller.spline.SetRightTangent(i, Vector2.up * _tangentRange); 
                    break;
            }

        }
        
    }
}
