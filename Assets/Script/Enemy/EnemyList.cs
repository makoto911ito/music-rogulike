using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>スポーンした敵を管理するスクリプト</summary>
public class EnemyList : MonoBehaviour
{
    /// <summary>敵キャラを管理するリスト</summary>
    [SerializeField] List<GameObject> _enemys = new List<GameObject>();

    /// <summary>
    /// 敵オブジェクトをリストに入れるための関数
    /// </summary>
    /// <param name="enemy">敵オブジェクト</param>
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
                Debug.Log("破壊する敵見つけた");

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
    /// リストの中に敵が残っているかどうか
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
    /// 敵を動かすための関数
    /// </summary>
    public void GoEnemyMove()
    {
        Debug.Log("GoEnemyMove　が反応している");
        for (var i = 0; i < _enemys.Count; i++)
        {
            Debug.Log("生成されている敵に命令をしている");
            EnemyMove _enemyMove = _enemys[i].GetComponent<EnemyMove>();
            _enemyMove.MoveEnemy();
        }
    }

    /// <summary>
    /// プレイヤーの攻撃方向にどの敵がいるのか確認しその敵にダメージを与える
    /// </summary>
    /// <param name="posx">プレイヤーのX座標</param>
    /// <param name="posz">プレイヤーのZ座標</param>
    /// <param name="pPower">プレイヤーの攻撃力</param>
    public void CheckEnemy(int posx, int posz, int pPower)
    {
        Debug.Log("呼ばれている");
        for (var j = 0; j < _enemys.Count; j++)
        {
            Debug.Log("調べている");
            var _enemyMove = _enemys[j].GetComponent<EnemyMove>();

            if (_enemyMove._pointX == posx && _enemyMove._pointZ == posz)
            {
                var _enemyPresenter = _enemys[j].GetComponent<EnemyPresenter>();
                _enemyPresenter.Damage(pPower);
            }
        }
    }
}
