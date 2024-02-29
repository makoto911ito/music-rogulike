using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackObj : MonoBehaviour
{
    /// <summary>ステージの情報を管理しているスクリプトを参照するための変数</summary>
    AreaController areaController;

    PlayerPresenter _playerPresenter;

    EnemyAttackObjController _enemyAttackObjController;

    /// <summary>進んでいく方向(何軸か)</summary>
    string _direction;
    /// <summary>進んでいく向き({true = + }{false = - }方向)</summary>
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
                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                    //自身を破壊する
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else if (areaController._onPlayer == true)
                {
                    //攻撃をする
                    _playerPresenter.EnemyAttack(1);
                    //破壊する
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
                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                    //破壊する
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else if (areaController._onPlayer == true)
                {
                    //Debug.Log("プレイヤーに攻撃");
                    //攻撃をする
                    _playerPresenter.EnemyAttack(1);
                    //破壊する
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
                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                    //破壊する
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else if (areaController._onPlayer == true)
                {
                    //Debug.Log("プレイヤーに攻撃");
                    //攻撃をする
                    _playerPresenter.EnemyAttack(1);
                    //破壊する
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
                //移動先の情報によって行動を決める
                if (areaController._onWall == true)
                {
                    //破壊する
                    _enemyAttackObjController.DestroyAObj(this.gameObject);
                }
                else if (areaController._onPlayer == true)
                {
                    //Debug.Log("プレイヤーに攻撃");
                    //攻撃をする
                    _playerPresenter.EnemyAttack(1);
                    //破壊する
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
