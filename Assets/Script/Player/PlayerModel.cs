using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace PlayerModel
{
    public class Hp
    {
        /// <summary>�v���C���[��HP�̕ω����Ǘ�����ϐ�</summary>
        ReactiveProperty<int> _playerHpProperty;
        int _maxHp = 0;

        public Hp(int maxHp, System.Action<int> action, GameObject gameObject)
        {
            _maxHp = maxHp;
            _playerHpProperty = new ReactiveProperty<int>(_maxHp);
            _playerHpProperty.Subscribe(action).AddTo(gameObject);
        }

        public void Damage(int damage)
        {
            //Debug.Log(_playerHpProperty.Value + "HP");
            _playerHpProperty.Value -= damage;
        }
    }

    public class PowerModel
    {
        /// <summary>�v���C���[�̍U���͂��Ǘ�����ϐ�</summary>
        ReactiveProperty<float> _playerPowerProperty;
        /// <summary>�v���C���[�̍U����</summary>
        float _power = 1;

        public PowerModel(int power, System.Action<float> action, GameObject gameObject)
        {
            _power = power;
            _playerPowerProperty = new ReactiveProperty<float>(_power);
            _playerPowerProperty.Subscribe(action).AddTo(gameObject);
        }

        public void ChangPower(float power)
        {
            _playerPowerProperty.Value = power;
        }
    }
}
