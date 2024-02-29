using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyModel
{
    /// <summary>敵キャラクターのHPを管理する変数</summary>
    ReactiveProperty<float> _enemyHpPropety;
    float _maxHp = 0;

    public EnemyModel(float maxHp, System.Action<float> action, GameObject gameObject)
    {
        _maxHp = maxHp;
        _enemyHpPropety = new ReactiveProperty<float>(maxHp);
        _enemyHpPropety.Subscribe(action).AddTo(gameObject);
    }

    public void Damage(float damage)
    {
        Debug.Log("計算している");
        _enemyHpPropety.Value -= damage;
    }

}
