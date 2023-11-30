using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    /// <summary>配列の一つ目の要素の場所をランダムで見るための変数</summary>
    int _randomNumX;

    /// <summary>配列の二つ目の要素の場所をランダムで見るための変数</summary>
    int _randomNumZ;

    /// <summary>敵のオブジェクトを配列に保存するための変数</summary>
    [SerializeField] GameObject[] _enemys;

    /// <summary>ボス敵のオブジェクトを配列に保存するための変数</summary>
    [SerializeField] GameObject[] _bossEnemys;

    [SerializeField] GameObject[] _gameBossEnemys;

    /// <summary>スポーンさせる敵の合計数</summary>
    [SerializeField] int _spawnEnemyNum;

    /// <summary>スポーンさせる敵キャラをランダムで選ぶための変数</summary>
    int _randoEnemyNum;

    /// <summary>エリア情報を取得するためのAreaControllerスクリプトを獲得する変数</summary>
    AreaController areaController;

    /// <summary></summary>
    [SerializeField] EnemyList _enemyList = null;

    [SerializeField]

    public void spawn(int spawnNum)
    {
        _spawnEnemyNum = spawnNum;

        for (var i = 0; i < _spawnEnemyNum; i++)
        {
            _randoEnemyNum = Random.Range(0, _enemys.Length);
            _randomNumX = Random.Range(0, MapManager._x);
            _randomNumZ = Random.Range(0, MapManager._z);

            if (MapManager._areas[_randomNumX, _randomNumZ] == null)
            {
                i -= 1;
                continue;
            }
            else if (MapManager._areas[_randomNumX, _randomNumZ] != null)
            {
                areaController = MapManager._areas[_randomNumX, _randomNumZ].GetComponent<AreaController>();
                //Debug.Log("反応している");

                if (areaController._onEnemy == true || areaController._onWall == true || areaController._onPlayer == true)
                {
                    i -= 1;
                    continue;
                }
                else
                {
                    Debug.Log("敵キャラが生成された");
                    GameObject _enemy = Instantiate(_enemys[_randoEnemyNum], new Vector3(MapManager._areas[_randomNumX, _randomNumZ].transform.position.x, 1.5f, MapManager._areas[_randomNumX, _randomNumZ].transform.position.z), Quaternion.identity);
                    _enemyList.Enemy(_enemy);

                }
            }
        }

    }

    public void EnemyHouse(bool TimeUp)
    {
        bool _isSpon = true;
        int spawnNum = Random.Range(3, 8);
        int _count = 1;

        while (_isSpon)
        {
            int _randomNum = Random.Range(0, _bossEnemys.Length);
            int _randomBossPosNumX = Random.Range(0, MapManager._x);
            int _randomBossPosNumZ = Random.Range(0, MapManager._z);

            if (MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ] == null)
            {
                continue;
            }
            else if (MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ] != null)
            {
                areaController = MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].GetComponent<AreaController>();
                //Debug.Log("反応している");

                if (areaController._onEnemy == true || areaController._onWall == true || areaController._onPlayer == true)
                {
                    continue;
                }
                else
                {
                    //Debug.Log("ボスキャラが生成された");
                    GameObject _enemy = Instantiate(_bossEnemys[_randomNum], new Vector3(MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].transform.position.x, 1.5f, MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].transform.position.z), Quaternion.identity);
                    _enemyList.Enemy(_enemy);
                    if (_count == spawnNum)
                    {
                        _isSpon = false;
                    }
                    else
                    {
                        _count++;
                    }
                }
            }
        }
    }

    public void BossSpawn(bool LastMap, bool TimeUp)
    {
        int _randomNum = Random.Range(0, _bossEnemys.Length);
        bool _isSpon = true;

        while (_isSpon)
        {
            int _randomBossPosNumX = Random.Range(0, MapManager._x);
            int _randomBossPosNumZ = Random.Range(0, MapManager._z);

            if (MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ] == null)
            {
                continue;
            }
            else if (MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ] != null)
            {
                areaController = MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].GetComponent<AreaController>();
                //Debug.Log("反応している");

                if (areaController._onEnemy == true || areaController._onWall == true || areaController._onPlayer == true)
                {
                    continue;
                }
                else
                {
                    if (LastMap != true)
                    {
                        if (TimeUp != true)
                        {
                            //Debug.Log("ボスキャラが生成された");
                            GameObject _enemy = Instantiate(_bossEnemys[_randomNum], new Vector3(MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].transform.position.x, 1.5f, MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].transform.position.z), Quaternion.identity);
                            _enemyList.Enemy(_enemy);
                            _isSpon = false;
                        }
                        else
                        {
                            int spawnNum = Random.Range(3, 11);

                            for (int i = 0; i <= spawnNum; i++)
                            {
                                //Debug.Log("ボスキャラが生成された");
                                GameObject _enemy = Instantiate(_bossEnemys[_randomNum], new Vector3(MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].transform.position.x, 1.5f, MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].transform.position.z), Quaternion.identity);
                                _enemyList.Enemy(_enemy);
                            }
                        }

                    }
                    else
                    {
                        int num = Random.Range(0, _gameBossEnemys.Length);
                        //Debug.Log("ラスボスが現れた");
                        GameObject _enemy = Instantiate(_gameBossEnemys[num], new Vector3(MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].transform.position.x, 1.5f, MapManager._areas[_randomBossPosNumX, _randomBossPosNumZ].transform.position.z), Quaternion.identity);
                        _enemyList.Enemy(_enemy);
                        _isSpon = false;
                    }

                }
            }
        }
    }

}
