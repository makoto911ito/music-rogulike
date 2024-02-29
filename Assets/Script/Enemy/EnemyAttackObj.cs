using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackObj : MonoBehaviour
{
    /// <summary>�X�e�[�W�̏����Ǘ����Ă���X�N���v�g���Q�Ƃ��邽�߂̕ϐ�</summary>
    AreaController areaController;

    PlayerPresenter _playerPresenter;

    EnemyAttackObjController _enemyAttackObjController;

    /// <summary>�i��ł�������(������)</summary>
    string _direction;
    /// <summary>�i��ł�������({true = + }{false = - }����)</summary>
    bool _directionJudg;

    int _posX;
    int _posZ;

    public void ThisInit(string direction,bool judg, int posx ,int posz , PlayerPresenter playerPresenter,EnemyAttackObjController enemyAttackObjController)
    {
        _direction = direction;
        _directionJudg = judg;
        _posX = posx;
        _posZ = posz;
        _playerPresenter = playerPresenter;
        _enemyAttackObjController = enemyAttackObjController;
    }

    public void GoObj()
    {
        if (_direction == "x")
        {
            if (_directionJudg == true)
            {
                areaController = MapManager._areas[_posX + 1, _posZ].GetComponent<AreaController>();
                //�ړ���̏��ɂ���čs�������߂�
                if (areaController._onWall == true)
                {
                    //���g��j�󂷂�
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else if (areaController._onPlayer == true)
                {
                    //�U��������
                    _playerPresenter.EnemyAttack(1);
                    //�j�󂷂�
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else
                {
                    transform.position = new Vector3(_posX + 1, transform.position.y, _posZ);
                    _posX = _posX + 1;
                }
            }
            else
            {
                areaController = MapManager._areas[_posX - 1, _posZ].GetComponent<AreaController>();
                //�ړ���̏��ɂ���čs�������߂�
                if (areaController._onWall == true)
                {
                    //�j�󂷂�
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else if (areaController._onPlayer == true)
                {
                    //Debug.Log("�v���C���[�ɍU��");
                    //�U��������
                    _playerPresenter.EnemyAttack(1);
                    //�j�󂷂�
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else
                {
                    transform.position = new Vector3(_posX + 1, transform.position.y, _posZ);
                    _posX = _posX - 1;
                }
            }
        }
        else
        {
            if (_directionJudg == true)
            {
                areaController = MapManager._areas[_posX, _posZ + 1].GetComponent<AreaController>();
                //�ړ���̏��ɂ���čs�������߂�
                if (areaController._onWall == true)
                {
                    //�j�󂷂�
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else if (areaController._onPlayer == true)
                {
                    //Debug.Log("�v���C���[�ɍU��");
                    //�U��������
                    _playerPresenter.EnemyAttack(1);
                    //�j�󂷂�
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else
                {
                    transform.position = new Vector3(_posX, transform.position.y, _posZ + 1);
                    _posZ = _posZ + 1;
                }
            }
            else
            {
                areaController = MapManager._areas[_posX, _posZ - 1].GetComponent<AreaController>();
                //�ړ���̏��ɂ���čs�������߂�
                if (areaController._onWall == true)
                {
                    //�j�󂷂�
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else if (areaController._onPlayer == true)
                {
                    //Debug.Log("�v���C���[�ɍU��");
                    //�U��������
                    _playerPresenter.EnemyAttack(1);
                    //�j�󂷂�
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else
                {
                    transform.position = new Vector3(_posX, transform.position.y, _posZ - 1);
                    _posZ = _posZ - 1;
                }
            }
        }
    }
}
