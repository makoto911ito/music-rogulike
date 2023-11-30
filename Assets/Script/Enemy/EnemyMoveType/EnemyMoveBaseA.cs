using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>���̃X�N���v�g�ł̓G�̓����^�͓񔏂Ɉ��s������R�[�h�������Ă���</summary>
class EnemyMoveBaseA : MoveBase
{
    /// <summary>���S�ɂ��邩�ǂ������f���邽�߂̃J�E���g�ϐ��i�O���ƒ��S�i�����X�|�[���u�j�j</summary>
    int _count = 0;
    /// <summary>�܂�Ԃ����ǂ���</summary>
    bool _change = false;

    bool _canMove = false;

    private readonly EnemyMove _enemyMove;
    private readonly PlayerPresenter _playerPresenter;

    bool _once = true;

    int _initPosX;
    //�ړ���̏ꏊ���L�����邽�߂̕ϐ�
    int _enemyPosX;

    public EnemyMoveBaseA(EnemyMove enemyMove, PlayerPresenter playerPresenter)
    {
        _enemyMove = enemyMove;
        _playerPresenter = playerPresenter;
    }

    /// <summary>�s���̊֐�</summary>
    public override void Move()
    {
        if(_once != false)
        {
            _initPosX = _enemyMove._pointX;
            _enemyPosX = _enemyMove._pointX;
            Debug.Log(_enemyPosX + "�G�̈ʒuX");
            _once = false;
        }
        //Debug.Log(_enemyMove._pointX + "�G�̈ʒuX");

        //Debug.Log(_count + "��");

        if (_canMove == false)
        {
            _canMove = true;
        }
        else
        {
            if (_count > 0)
            {
                _count--;
                _change = true;
            }
            else if (_count < 0)
            {
                _count++;
                _change = false;
            }
            else//���S�ɂ���ꍇ
            {
                if (_change == true)
                {
                    Debug.Log(_count + "/" + _change + "�܂�Ԃ�");
                    _count--;
                }
                else
                {
                    Debug.Log(_count + "/" + _change + "�܂�Ԃ�");
                    _count++;
                }

            }


            if(_count == 0)
            {
                //�s�����������̏����m�F�������̂ňړ���̃X�N���v�g���擾����
                var areaController = MapManager._areas[_initPosX, _enemyMove._pointZ].GetComponent<AreaController>();

                //�ړ���̏��ɂ���čs�������߂�
                if (areaController._onWall == true)
                {
                    //���΂Ɉړ����邩������Ȃ�
                }
                else if (areaController._onEnemy == true) { }
                else if (areaController._onPlayer == true)
                {
                    //�U��������
                    _playerPresenter.EnemyAttack(1);
                }
                else
                {
                    areaController._onEnemy = true;
                    areaController = MapManager._areas[_enemyPosX, _enemyMove._pointZ].GetComponent<AreaController>();
                    areaController._onEnemy = false;
                    _enemyMove.transform.position = new Vector3(MapManager._areas[_initPosX, _enemyMove._pointZ].transform.position.x, _enemyMove.transform.position.y, _enemyMove.transform.position.z);
                    _enemyPosX = _initPosX;
                    Debug.Log(_count + "����");
                    Debug.Log(_enemyPosX + "���݂̈ʒu");
                }

            }
            else
            {
                //�s�����������̏����m�F�������̂ňړ���̃X�N���v�g���擾����
                var areaController = MapManager._areas[_enemyPosX + _count, _enemyMove._pointZ].GetComponent<AreaController>();

                //�ړ���̏��ɂ���čs�������߂�
                if (areaController._onWall == true)
                {
                    //���΂Ɉړ����邩������Ȃ�
                }
                else if (areaController._onEnemy == true) { _count--; }
                else if (areaController._onPlayer == true)
                {
                    //�U��������
                    _playerPresenter.EnemyAttack(1);
                }
                else
                {
                    areaController._onEnemy = true;
                    areaController = MapManager._areas[_enemyPosX, _enemyMove._pointZ].GetComponent<AreaController>();
                    areaController._onEnemy = false;
                    _enemyMove.transform.position = new Vector3(MapManager._areas[_enemyPosX + _count, _enemyMove._pointZ].transform.position.x, _enemyMove.transform.position.y, _enemyMove.transform.position.z);
                    _enemyPosX = _enemyPosX + _count;
                    Debug.Log(_count + "����");
                    Debug.Log(_enemyPosX + "���݂̈ʒu");
                }
            }


            //if (_count > 0 || _count == 0 && _change == true)//���]��false����count��0��������or���]���Ă��Ċ���count��0����Ȃ�����
            //{
            //    //�s�����������̏����m�F�������̂ňړ���̃X�N���v�g���擾����
            //    var areaController = MapManager._areas[_enemyPosX + 1, _enemyMove._pointZ].GetComponent<AreaController>();

            //    //�ړ���̏��ɂ���čs�������߂�
            //    if (areaController._onWall == true)
            //    {
            //        //���΂Ɉړ����邩������Ȃ�
            //    }
            //    else if (areaController._onEnemy == true) { }
            //    else if (areaController._onPlayer == true)
            //    {
            //        //�U��������
            //        _playerPresenter.EnemyAttack(1);
            //    }
            //    else
            //    {
            //        areaController._onEnemy = true;
            //        areaController = MapManager._areas[_enemyPosX, _enemyMove._pointZ].GetComponent<AreaController>();
            //        areaController._onEnemy = false;
            //        _enemyMove.transform.position = new Vector3(MapManager._areas[_enemyPosX + 1, _enemyMove._pointZ].transform.position.x, _enemyMove.transform.position.y, _enemyMove.transform.position.z);
            //    }
            //    _enemyPosX = _enemyPosX + 1;
            //}
            //else if (_count < 0 || _count == 0 && _change == false) //���]��false����count��0����Ȃ�������or���]���Ă��Ċ���count��0��������
            //{
            //    if (_enemyPosX == 0)
            //    {
            //        Debug.Log("���̐�͉����Ȃ��̂œ����Ȃ�");
            //    }
            //    else
            //    {
            //        //�s�����������̏����m�F�������̂ňړ���̃X�N���v�g���擾����
            //        var areaController = MapManager._areas[_enemyPosX - 1, _enemyMove._pointZ].GetComponent<AreaController>();

            //        //�ړ���̏��ɂ���čs�������߂�
            //        if (areaController._onWall == true)
            //        {

            //        }
            //        else if (areaController._onEnemy == true) { }
            //        else if (areaController._onPlayer == true)
            //        {
            //            //�v���C���[�ɍU������
            //            _playerPresenter.EnemyAttack(1);
            //        }
            //        else
            //        {
            //            areaController._onEnemy = true;
            //            areaController = MapManager._areas[_enemyPosX, _enemyMove._pointZ].GetComponent<AreaController>();
            //            areaController._onEnemy = false;
            //            _enemyMove.transform.position = new Vector3(MapManager._areas[_enemyPosX - 1, _enemyMove._pointZ].transform.position.x, _enemyMove.transform.position.y, _enemyMove.transform.position.z);
            //        }
            //        _enemyPosX = _enemyPosX - 1;
            //    }
            //}

            _canMove = false;
        }

    }
}
