using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>�X�|�[�������G���Ǘ�����X�N���v�g</summary>
public class EnemyList : MonoBehaviour
{
    /// <summary>�G�L�������Ǘ����郊�X�g</summary>
    [SerializeField] List<GameObject> _enemys = new List<GameObject>();

    /// <summary>
    /// �G�I�u�W�F�N�g�����X�g�ɓ���邽�߂̊֐�
    /// </summary>
    /// <param name="enemy">�G�I�u�W�F�N�g</param>
    public void Enemy(GameObject enemy)
    {
        _enemys.Add(enemy);
    }

    public void EnemyDestroy(GameObject destroyEnemy)
    {
        for (var i = 0; i < _enemys.Count; i++)
        {
            if (_enemys[i] == destroyEnemy)
            {
                Debug.Log("�j�󂷂�G������");

                EnemyMove _enemyMove = _enemys[i].GetComponent<EnemyMove>();
                _enemys.Remove(destroyEnemy);
                _enemyMove.DeleteEnemy();

            }
        }
    }

    public void EnemyReset()
    {
        for (var i = 0; i < _enemys.Count; i++)
        {
            var _enemyMove = _enemys[i].GetComponent<EnemyMove>();
            _enemyMove.DeleteEnemy();

            if (i == _enemys.Count - 1)
            {
                _enemys.Clear();
            }
        }
    }

    /// <summary>
    /// ���X�g�̒��ɓG���c���Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool CheckEnemy()
    {
        bool _clearEnemy;

        if (_enemys == null)
        {
            _clearEnemy = true;
        }
        else
        {
            _clearEnemy = false;
        }
        return _clearEnemy;
    }

    /// <summary>
    /// �G�𓮂������߂̊֐�
    /// </summary>
    public void GoEnemyMove()
    {
        Debug.Log("GoEnemyMove�@���������Ă���");
        for (var i = 0; i < _enemys.Count; i++)
        {
            Debug.Log("��������Ă���G�ɖ��߂����Ă���");
            EnemyMove _enemyMove = _enemys[i].GetComponent<EnemyMove>();
            _enemyMove.MoveEnemy();
        }
    }

    /// <summary>
    /// �v���C���[�̍U�������ɂǂ̓G������̂��m�F�����̓G�Ƀ_���[�W��^����
    /// </summary>
    /// <param name="posx">�v���C���[��X���W</param>
    /// <param name="posz">�v���C���[��Z���W</param>
    /// <param name="pPower">�v���C���[�̍U����</param>
    public void CheckEnemy(int posx, int posz, int pPower)
    {
        Debug.Log("�Ă΂�Ă���");
        for (var j = 0; j < _enemys.Count; j++)
        {
            Debug.Log("���ׂĂ���");
            var _enemyMove = _enemys[j].GetComponent<EnemyMove>();

            if (_enemyMove._pointX == posx && _enemyMove._pointZ == posz)
            {
                var _enemyPresenter = _enemys[j].GetComponent<EnemyPresenter>();
                _enemyPresenter.Damage(pPower);
            }
        }
    }
}