using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class EnemyMoveBaseC : MoveBase
{
    private readonly EnemyMove _enemyMove;
    private readonly PlayerPresenter _playerPresenter;
    private readonly EnemyAttackObjController _enemyAttackObjController;

    GameObject _enemyAttackObj;

    bool _frg = false;
    bool _canMove = false;

    /// <summary>ステージの情報を管理しているスクリプトを参照するための変数</summary>
    AreaController areaController;

    public EnemyMoveBaseC(EnemyMove enemyMove, PlayerPresenter playerPresenter , EnemyAttackObjController enemyAttackObjController , GameObject enemyAttackObj)
    {
        _enemyMove = enemyMove;
        _playerPresenter = playerPresenter;
        if(_playerPresenter == null)
        {
            Debug.Log("渡されてないよ");
        }
        _enemyAttackObjController = enemyAttackObjController;
        _enemyAttackObj = enemyAttackObj;
    }

    public override void Move()
    {
        if (_canMove == false)
        {
            _canMove = true;
        }
        else
        {
            _canMove = false;
        }

        //Debug.Log("敵の初期値" + "x " + _enemyMove._pointX + " z " + _enemyMove._pointZ);
        if (_enemyMove._canMove == true)
        {
            if (_canMove == true)
            {
                for (int i = _enemyMove._pointX - 10; i <= _enemyMove._pointX + 10; i++)
                {
                    if (MapManager._areas.GetLength(0) <= i || 0 > i)
                    {
                        continue;
                    }
                    else
                    {
                        for (int j = _enemyMove._pointZ - 10; j <= _enemyMove._pointZ + 10; j++)
                        {
                            if (MapManager._areas.GetLength(1) <= j || 0 > j)
                            {
                                continue;
                            }
                            else
                            {

                                if(MapManager._areas[i, j] != null)
                                {

                                    var areaController = MapManager._areas[i, j].GetComponent<AreaController>();

                                    if (areaController._onPlayer == true)
                                    {
                                        //Debug.Log("プレイヤーを見つけた");
                                        CheackDistance(i, j);

                                        break;
                                    }
                                }
                                else
                                {
                                    Debug.Log("何もない");
                                }

                                //Debug.Log("x " + i + " z " + j);
                                //if (MapManager._areas[i, j].GetComponent<AreaController>() == null)
                                //{
                                //    Debug.Log("何もない");
                                //}
                                //else { }

                            }
                        }
                    }

                    //Debug.Log("プレイヤーが見つからなかった");
                }
            }

        }
    }

    private void CheackDistance(int _PlayerX, int _PlayerZ)
    {
        var _cheackNumX = 0;
        var _cheackNumZ = 0;

        if (_PlayerX < 0 && _enemyMove._pointX < 0 || _PlayerZ > 0 && _enemyMove._pointX > 0)
        {
            int _distanceX = _PlayerX - _enemyMove._pointX;
            int _distanceZ = _PlayerZ - _enemyMove._pointZ;

            if (_distanceX < 0)//マイナスのままだと比較しずらいのでマイナスを外す
            {
                _cheackNumX = _distanceX * -1;
            }
            else
            {
                _cheackNumX = _distanceX;
            }

            if (_distanceZ < 0)//マイナスのままだと比較しずらいのでマイナスを外す
            {
                _cheackNumZ = _distanceZ * -1;
            }
            else
            {
                _cheackNumZ = _distanceZ;
            }

            //Debug.Log("計算結果" + _cheackNumX + "  " + _cheackNumZ);

            if (_cheackNumX == _cheackNumZ)
            {
                int _rndom = Random.Range(0, 2);

                if (_rndom == 0)
                {
                    if (_PlayerX - _enemyMove._pointX < 0)
                    {
                        _frg = false;
                    }
                    else
                    {
                        _frg = true;
                    }
                    GoMove("x", false);
                }
                else
                {
                    if (_PlayerZ - _enemyMove._pointZ < 0)
                    {
                        _frg = false;
                    }
                    else
                    {
                        _frg = true;
                    }
                    //Zのほうが距離遠いので詰めるように動く
                    GoMove("z",false);
                }
            }
            else if (_cheackNumX == 0)
            {
                if (_PlayerZ - _enemyMove._pointZ < 0)
                {
                    _frg = false;
                }
                else
                {
                    _frg = true;
                }
                //X座標の距離が０だったのでZの方に行くコードに飛ぶ
                GoMove("z",true);
            }
            else if (_cheackNumZ == 0)
            {
                if (_PlayerX - _enemyMove._pointX < 0)
                {
                    _frg = false;
                }
                else
                {
                    _frg = true;
                }

                //Z座標の距離が０だったのでXの方に行くコードに飛ぶ
                GoMove("x",true);
            }
            else
            {
                if (_cheackNumX > _cheackNumZ)
                {
                    if (_PlayerX - _enemyMove._pointX < 0)
                    {
                        _frg = false;
                    }
                    else
                    {
                        _frg = true;
                    }

                    //Xの方が距離遠いので詰めるように動く
                    GoMove("x", false);
                }
                else
                {
                    if (_PlayerZ - _enemyMove._pointZ < 0)
                    {
                        _frg = false;
                    }
                    else
                    {
                        _frg = true;
                    }
                    //Zのほうが距離遠いので詰めるように動く
                    GoMove("z", false);
                }
            }
        }
    }

    private void GoMove(string _muki,bool front)
    {
        if (_muki == "x")
        {
            if (_frg == true)
            {
                areaController = MapManager._areas[_enemyMove._pointX + 1, _enemyMove._pointZ].GetComponent<AreaController>();
                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                }
                else if (areaController._onEnemy == true) { }
                else if (front == true)
                {
                    //攻撃をする（飛び道具を放つ）
                    _enemyAttackObjController.Generate(_enemyAttackObj,_muki, _frg,_playerPresenter,_enemyMove._pointX,_enemyMove._pointZ);
                }
                else
                {
                    areaController._onEnemy = true;
                    areaController = MapManager._areas[_enemyMove._pointX, _enemyMove._pointZ].GetComponent<AreaController>();
                    areaController._onEnemy = false;
                    _enemyMove.transform.position = new Vector3(MapManager._areas[_enemyMove._pointX + 1, _enemyMove._pointZ].transform.position.x, _enemyMove.transform.position.y, _enemyMove.transform.position.z);
                    _enemyMove._pointX = _enemyMove._pointX + 1;
                }
            }
            else
            {
                areaController = MapManager._areas[_enemyMove._pointX - 1, _enemyMove._pointZ].GetComponent<AreaController>();
                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                    //反対に移動するかもしれない
                }
                else if (areaController._onEnemy == true) { }
                else if (front == true)
                {
                    //攻撃をする（飛び道具を放つ）
                    _enemyAttackObjController.Generate(_enemyAttackObj, _muki, _frg, _playerPresenter, _enemyMove._pointX, _enemyMove._pointZ);
                }
                else
                {
                    areaController._onEnemy = true;
                    areaController = MapManager._areas[_enemyMove._pointX, _enemyMove._pointZ].GetComponent<AreaController>();
                    areaController._onEnemy = false;
                    _enemyMove.transform.position = new Vector3(MapManager._areas[_enemyMove._pointX - 1, _enemyMove._pointZ].transform.position.x, _enemyMove.transform.position.y, _enemyMove.transform.position.z);
                    _enemyMove._pointX = _enemyMove._pointX - 1;
                }
            }
        }
        else
        {
            if (_frg == true)
            {
                areaController = MapManager._areas[_enemyMove._pointX, _enemyMove._pointZ + 1].GetComponent<AreaController>();
                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {

                }
                else if (areaController._onEnemy == true) { }
                else if (front == true)
                {
                    //攻撃をする（飛び道具を放つ）
                    _enemyAttackObjController.Generate(_enemyAttackObj, _muki, _frg, _playerPresenter, _enemyMove._pointX, _enemyMove._pointZ);
                }
                else
                {
                    areaController._onEnemy = true;
                    areaController = MapManager._areas[_enemyMove._pointX, _enemyMove._pointZ].GetComponent<AreaController>();
                    areaController._onEnemy = false;
                    _enemyMove.transform.position = new Vector3(_enemyMove.transform.position.x, _enemyMove.transform.position.y, MapManager._areas[_enemyMove._pointX, _enemyMove._pointZ + 1].transform.position.z);

                    //Debug.Log("変更前の位置" + _enemyMove._pointZ);
                    _enemyMove._pointZ = _enemyMove._pointZ + 1;
                    //Debug.Log("変更後の位置" + _enemyMove._pointZ);
                }
            }
            else
            {
                areaController = MapManager._areas[_enemyMove._pointX, _enemyMove._pointZ - 1].GetComponent<AreaController>();
                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                }
                else if (areaController._onEnemy == true) { }
                else if (front == true)
                {
                    //攻撃をする（飛び道具を放つ）
                    _enemyAttackObjController.Generate(_enemyAttackObj, _muki, _frg, _playerPresenter, _enemyMove._pointX, _enemyMove._pointZ);
                }
                else
                {
                    areaController._onEnemy = true;
                    areaController = MapManager._areas[_enemyMove._pointX, _enemyMove._pointZ].GetComponent<AreaController>();
                    areaController._onEnemy = false;
                    _enemyMove.transform.position = new Vector3(_enemyMove.transform.position.x, _enemyMove.transform.position.y, MapManager._areas[_enemyMove._pointX, _enemyMove._pointZ - 1].transform.position.z);
                    //Debug.Log("変更前の位置" + _enemyMove._pointZ);
                    _enemyMove._pointZ = _enemyMove._pointZ - 1;
                    //Debug.Log("変更後の位置" + _enemyMove._pointZ);
                }
            }
        }
    }
}
