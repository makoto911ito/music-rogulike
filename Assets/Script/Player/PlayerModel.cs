using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerModel
{
    /// <summary>�v���C���[��HP�̕ω����Ǘ�����ϐ�</summary>
    ReactiveProperty<int> _playerHpProperty;
    int _maxHp = 0;

    /// <summary>�v���C���[�̍U����</summary>
    public int _pPower = 1;

    public PlayerModel(int maxHp, System.Action<int> action, GameObject gameObject)
    {
        _maxHp = maxHp;
        _playerHpProperty = new ReactiveProperty<int>(maxHp);
        _playerHpProperty.Subscribe(action).AddTo(gameObject);
    }

    public void Damage(int damage)
    {
        Debug.Log("�v�Z��");
        _playerHpProperty.Value -= damage;
    }
}