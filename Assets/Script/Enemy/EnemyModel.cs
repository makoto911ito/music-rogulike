using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyModel
{
    /// <summary>敵キャラクターのHPを管理する変数</summary>
    ReactiveProperty<int> _enemyHpPropety;
    int _maxHp = 0;

    public EnemyModel(int maxHp, System.Action<int> action, GameObject gameObject)
    {
        _maxHp = maxHp;
        _enemyHpPropety = new ReactiveProperty<int>(maxHp);
        _enemyHpPropety.Subscribe(action).AddTo(gameObject);
    }

    public void Damage(int damage)
    {
        Debug.Log("計算している");
        _enemyHpPropety.Value -= damage;
    }

}
