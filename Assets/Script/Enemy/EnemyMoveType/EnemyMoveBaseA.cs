using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>このスクリプトでの敵の動き型は二拍に一回行動するコードを書いている</summary>
class EnemyMoveBaseA : MoveBase
{
    /// <summary>中心にいるかどうか判断するためのカウント変数（０だと中心（初期スポーン置））</summary>
    int _count = 0;
    /// <summary>折り返すかどうか</summary>
    bool _change = false;

    bool _canMove = false;

    private readonly EnemyMove _enemyMove;
    private readonly PlayerPresenter _playerPresenter;

    bool _once = true;

    int _initPosX;
    //移動後の場所を記憶するための変数
    int _enemyPosX;

    public EnemyMoveBaseA(EnemyMove enemyMove, PlayerPresenter playerPresenter)
    {
        _enemyMove = enemyMove;
        _playerPresenter = playerPresenter;
    }

    /// <summary>行動の関数</summary>
    public override void Move()
    {
        if(_once != false)
        {
            _initPosX = _enemyMove._pointX;
            _enemyPosX = _enemyMove._pointX;
            Debug.Log(_enemyPosX + "敵の位置X");
            _once = false;
        }
        //Debug.Log(_enemyMove._pointX + "敵の位置X");

        //Debug.Log(_count + "回数");

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
            else//中心にいる場合
            {
                if (_change == true)
                {
                    Debug.Log(_count + "/" + _change + "折り返し");
                    _count--;
                }
                else
                {
                    Debug.Log(_count + "/" + _change + "折り返し");
                    _count++;
                }

            }


            if(_count == 0)
            {
                //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
                var areaController = MapManager._areas[_initPosX, _enemyMove._pointZ].GetComponent<AreaController>();

                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                    //反対に移動するかもしれない
                }
                else if (areaController._onEnemy == true) { }
                else if (areaController._onPlayer == true)
                {
                    //攻撃をする
                    _playerPresenter.EnemyAttack(1);
                }
                else
                {
                    areaController._onEnemy = true;
                    areaController = MapManager._areas[_enemyPosX, _enemyMove._pointZ].GetComponent<AreaController>();
                    areaController._onEnemy = false;
                    _enemyMove.transform.position = new Vector3(MapManager._areas[_initPosX, _enemyMove._pointZ].transform.position.x, _enemyMove.transform.position.y, _enemyMove.transform.position.z);
                    _enemyPosX = _initPosX;
                    Debug.Log(_count + "方向");
                    Debug.Log(_enemyPosX + "現在の位置");
                }

            }
            else
            {
                //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
                var areaController = MapManager._areas[_enemyPosX + _count, _enemyMove._pointZ].GetComponent<AreaController>();

                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                    //反対に移動するかもしれない
                }
                else if (areaController._onEnemy == true) { _count--; }
                else if (areaController._onPlayer == true)
                {
                    //攻撃をする
                    _playerPresenter.EnemyAttack(1);
                }
                else
                {
                    areaController._onEnemy = true;
                    areaController = MapManager._areas[_enemyPosX, _enemyMove._pointZ].GetComponent<AreaController>();
                    areaController._onEnemy = false;
                    _enemyMove.transform.position = new Vector3(MapManager._areas[_enemyPosX + _count, _enemyMove._pointZ].transform.position.x, _enemyMove.transform.position.y, _enemyMove.transform.position.z);
                    _enemyPosX = _enemyPosX + _count;
                    Debug.Log(_count + "方向");
                    Debug.Log(_enemyPosX + "現在の位置");
                }
            }


            //if (_count > 0 || _count == 0 && _change == true)//反転がfalse且つcountが0だったらor反転していて且つcountが0じゃなったら
            //{
            //    //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
            //    var areaController = MapManager._areas[_enemyPosX + 1, _enemyMove._pointZ].GetComponent<AreaController>();

            //    //移動先の情報によって行動を決める
            //    if (areaController._onWall == true)
            //    {
            //        //反対に移動するかもしれない
            //    }
            //    else if (areaController._onEnemy == true) { }
            //    else if (areaController._onPlayer == true)
            //    {
            //        //攻撃をする
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
            //else if (_count < 0 || _count == 0 && _change == false) //反転がfalse且つcountが0じゃなかったらor反転していて且つcountが0だったら
            //{
            //    if (_enemyPosX == 0)
            //    {
            //        Debug.Log("その先は何もないので動けない");
            //    }
            //    else
            //    {
            //        //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
            //        var areaController = MapManager._areas[_enemyPosX - 1, _enemyMove._pointZ].GetComponent<AreaController>();

            //        //移動先の情報によって行動を決める
            //        if (areaController._onWall == true)
            //        {

            //        }
            //        else if (areaController._onEnemy == true) { }
            //        else if (areaController._onPlayer == true)
            //        {
            //            //プレイヤーに攻撃する
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
