using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPresenter : MonoBehaviour
{
    /// <summary>エネミーのデータに関してのクラス</summary>
    EnemyModel _enemyModel = null;

    /// <summary>エネミーの表示に関してのクラス</summary>
    [SerializeField] EnemyView _enemyView = null;

    [SerializeField] int _enemyHp = 1;

    EnemyList _enemyList = null;

    public void GetLisut()
    {
        var gameObject = GameObject.Find("EnemyList");

        _enemyList = gameObject.GetComponent<EnemyList>();
        if(_enemyList == null)
        {
            Debug.Log("EnemyListが取得できていません");
        }
    }

    public void Init()
    {
        _enemyModel = new EnemyModel(
            _enemyHp,
            x =>
            {
                _enemyView.ChangeSliderValue(_enemyHp, x);
                if(x <= 0)
                {
                    _enemyList.EnemyDestroy(this.gameObject);
                }
            },
            _enemyView.gameObject);


    }

    public void Damage(float pPower)
    {
        if (_enemyModel == null)
        {
            Debug.Log("EnemyModelはnullです");
        }

        Debug.Log("ダメージ与えている");
        _enemyModel.Damage(pPower);
    }
}
