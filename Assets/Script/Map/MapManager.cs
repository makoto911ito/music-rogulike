using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    /// <summary>横幅</summary>
    [SerializeField] static public int _x = 35;
    /// <summary>縦幅</summary>
    [SerializeField] static public int _z = 35;
    /// <summary>エリアの数</summary>
    [SerializeField] int _areaNum = 4;

    static public GameObject[,] _areas;

    //部屋の大きさの決めるための範囲
    /// <summary>エリア大きさの最小値</summary>
    [SerializeField] int _mapMin = 3;
    /// <summary>エリア大きさの最大値</summary>
    [SerializeField] int _mapMax = 7;

    //床や壁などのオブジェクト
    public GameObject[] _obj;

    //分割したエリアで精製されるマップの中心（z座標）
    int _areaUnderPointZ = 0;

    /// <summary>分割したエリアの上側で生成されるマップの中心（z座標） </summary>
    int _areaUpPointZ = 0;

    /// <summary>分割するエリアの大きさの最大値</summary>
    int _areaSize = 0;

    //分けたエリアの内でランダムにx座標を取得
    /// <summary>各区分の中心となるx座標</summary>
    int _randomPosX;

    /// <summary>今のx座標の最大値</summary>
    int keepMaxArea = 1;
    //
    /// <summary>前回のx座標の最大値</summary>
    int _keepMinAreaSize = 1;

    /// <summary>生成されたエリアの一番後ろのマスのｘ座標</summary>
    int _keepBackSide = 0;
    /// <summary>生成されたエリアの一番最初のマスのｘ座標</summary>
    int _keepFrontSide = 0;

    /// <summary>生成したエリアの中心（ｚ座標）</summary>
    int _center = 0;

    /// <summary>エリアの大きさ</summary>
    int _randomNum = 0;

    /// <summary>一番最初に生成されたマスのｘ座標をとるためのカウント</summary>
    int _count = 0;

    /// <summary>横方向の区切り位置</summary>
    int _zLine = 0;

    /// <summary>どちらの方向に生成するか判定するための変数</summary>
    int _randomJudgeNum = 0;

    /// <summary>指定エリアを横方向に区切るための範囲の最低値</summary>
    [SerializeField] int _minZLine = 10;

    /// <summary>プレイヤーを生成するためのスクリプトを参照するための変数</summary>
    [SerializeField] SpawnPlayer _sponPlayer;

    /// <summary>敵を生成するためのスクリプトを参照するための変数</summary>
    [SerializeField] SpawnEnemy _sponEnemy;

    AreaController areaController;

    bool _lastMap = false;

    public bool MapCreate(GameObject _mapController)
    {
        _areas = new GameObject[_x + 1, _z + 1];

        _areaSize = _x / _areaNum; //分割する大きさを決める

        _zLine = Random.Range(_minZLine, _z);// 横の区切りの位置を決める
        //Debug.Log(_zLine + " 横区切りの値");

        for (int i = 0; i < _areaNum; i++)　//マップの生成
        {
            _keepMinAreaSize = keepMaxArea; //前回の最大幅を保存する

            if (i == 0) // 最初の区画
            {
                keepMaxArea = _areaSize;
                _randomPosX = Random.Range(1, keepMaxArea);
                //Debug.Log(_randomPosX + " 最初の中心点");
            }
            else if (i == _areaNum - 1) //最後の区画の場合
            {
                keepMaxArea = _x - 1;
                //Debug.Log(_keepMinAreaSize + "前回の最大の幅");
                //Debug.Log(keepMaxArea + "　今回の最大の幅");
                _randomPosX = Random.Range(_keepMinAreaSize, keepMaxArea);
                //Debug.Log(_randomPosX + "　最後の中心点");
            }
            else // 間の区画
            {
                keepMaxArea += _areaSize;
                //Debug.Log(_keepMinAreaSize + "前回の最大の幅");
                //Debug.Log(keepMaxArea + "　今回の最大の幅");
                _randomPosX = Random.Range(_keepMinAreaSize, keepMaxArea);
                //Debug.Log(_randomPosX + "　今回の中心点");
            }

            _randomNum = Random.Range(_mapMin, _mapMax);　//部屋の大きさを決めている
            //Debug.Log("エリアの大きさ" + _randomNum);

            // Z座標をランダムで決める
            _areaUnderPointZ = Random.Range(1, _zLine - 1);// z座標１の所からz座標の区切った場所までの間で決める
            _areaUpPointZ = Random.Range(_zLine + 1, _z - 2);// z座標の区切った場所からz座標の最大値までの間で決める

            _randomJudgeNum = Random.Range(0, 2);

            //エリアを作るfor文
            for (int x = _randomPosX - _randomNum; x < _randomPosX + _randomNum; x++)
            {
                if (_randomJudgeNum == 0)
                {
                    for (int z = _areaUpPointZ - _randomNum; z < _areaUpPointZ + _randomNum; z++)
                    {
                        if (x > 0 && x < _x) // 一番端は外枠になるため
                        {
                            //マップ自体をくっつけないように最大幅から一マス離したところに生成させる
                            if (x >= _keepMinAreaSize + 2)
                            {
                                if (x <= keepMaxArea - 2)
                                {
                                    if (z < _z)
                                    {
                                        if (z > _zLine)
                                        {
                                            var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(x, 0, z), Quaternion.identity);
                                            obj.name = x + "/" + z;
                                            _areas[x, z] = obj;
                                            _areas[x, z].transform.parent = _mapController.transform;
                                            _keepBackSide = x;
                                            _count++;
                                        }
                                    }
                                }

                                if (_count == 1)
                                {
                                    _keepFrontSide = x;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int z = _areaUnderPointZ - _randomNum; z < _areaUnderPointZ + _randomNum; z++)
                    {
                        if (x > 0 && x < _x) // 一番端は外枠になるため
                        {
                            //マップ自体をくっつけないように最大幅から一マス離したところに生成させる
                            if (x >= _keepMinAreaSize + 2)
                            {
                                if (x <= keepMaxArea - 2)
                                {
                                    if (z > 0)
                                    {
                                        if (z < _zLine)
                                        {
                                            var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(x, 0, z), Quaternion.identity);
                                            obj.name = x + "/" + z;
                                            _areas[x, z] = obj;
                                            _areas[x, z].transform.parent = _mapController.transform;
                                            _keepBackSide = x;
                                            _count++;
                                        }
                                    }
                                }

                                if (_count == 1)
                                {
                                    _keepFrontSide = x;
                                }
                            }
                        }
                    }
                }

            }

            _count = 0;//ここでカウントをリセット
            //Debug.Log(_keepBackSide + "エリア最後尾の位置");

            //生成したエリアの最後尾のマスから今回の最大幅まで道をつなげる
            if (i != _areaNum - 1)
            {
                if (_randomJudgeNum == 0)
                {
                    for (int miti = _keepBackSide + 1; miti <= keepMaxArea; miti++)
                    {
                        //Debug.Log("動いた");
                        var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(miti, 0, _areaUpPointZ), Quaternion.identity);
                        obj.name = miti + "/" + _areaUpPointZ;
                        _areas[miti, _areaUpPointZ] = obj;
                        _areas[miti, _areaUpPointZ].transform.parent = _mapController.transform;
                    }
                }
                else
                {
                    for (int miti = _keepBackSide + 1; miti <= keepMaxArea; miti++)
                    {
                        //Debug.Log("動いた");
                        var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(miti, 0, _areaUnderPointZ), Quaternion.identity);
                        obj.name = miti + "/" + _areaUnderPointZ;
                        _areas[miti, _areaUnderPointZ] = obj;
                        _areas[miti, _areaUnderPointZ].transform.parent = _mapController.transform;
                    }
                }
            }

            //前回の最大幅から今回のエリアの一番最初に生成されたの列にところまで道を作る
            if (i > 0)
            {
                if (_randomJudgeNum == 0)
                {
                    for (int aisle = _keepMinAreaSize; aisle < _keepFrontSide; aisle++)
                    {
                        var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(aisle, 0, _areaUpPointZ), Quaternion.identity);
                        obj.name = aisle + "/" + _areaUpPointZ;
                        _areas[aisle, _areaUpPointZ] = obj;
                        _areas[aisle, _areaUpPointZ].transform.parent = _mapController.transform;
                    }

                    //道をつなげるためのコード
                    if (_areaUpPointZ < _center)
                    {
                        for (int rodo = _areaUpPointZ + 1; rodo < _center; rodo++)
                        {
                            var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(_keepMinAreaSize, 0, rodo), Quaternion.identity);
                            obj.name = _keepMinAreaSize + "/" + rodo;
                            _areas[_keepMinAreaSize, rodo] = obj;
                            _areas[_keepMinAreaSize, rodo].transform.parent = _mapController.transform;
                        }
                    }
                    else
                    {
                        for (int rodo = _center + 1; rodo < _areaUpPointZ; rodo++)
                        {
                            var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(_keepMinAreaSize, 0, rodo), Quaternion.identity);
                            obj.name = _keepMinAreaSize + "/" + rodo;
                            _areas[_keepMinAreaSize, rodo] = obj;
                            _areas[_keepMinAreaSize, rodo].transform.parent = _mapController.transform;
                        }
                    }
                }
                else
                {
                    for (int aisle = _keepMinAreaSize; aisle < _keepFrontSide; aisle++)
                    {
                        var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(aisle, 0, _areaUnderPointZ), Quaternion.identity);
                        obj.name = aisle + "/" + _areaUnderPointZ;
                        _areas[aisle, _areaUnderPointZ] = obj;
                        _areas[aisle, _areaUnderPointZ].transform.parent = _mapController.transform;
                    }

                    //道をつなげるためのコード
                    if (_areaUnderPointZ < _center)
                    {
                        for (int rodo = _areaUnderPointZ + 1; rodo < _center; rodo++)
                        {
                            var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(_keepMinAreaSize, 0, rodo), Quaternion.identity);
                            obj.name = _keepMinAreaSize + "/" + rodo;
                            _areas[_keepMinAreaSize, rodo] = obj;
                            _areas[_keepMinAreaSize, rodo].transform.parent = _mapController.transform;
                        }
                    }
                    else
                    {
                        for (int rodo = _center + 1; rodo < _areaUnderPointZ; rodo++)
                        {
                            var obj = Instantiate(_obj[(int)AreaObj.walk], new Vector3(_keepMinAreaSize, 0, rodo), Quaternion.identity);
                            obj.name = _keepMinAreaSize + "/" + rodo;
                            _areas[_keepMinAreaSize, rodo] = obj;
                            _areas[_keepMinAreaSize, rodo].transform.parent = _mapController.transform;
                        }
                    }
                }

            }

            //Z座標を保存する
            if (_randomJudgeNum == 0)
            {
                _center = _areaUpPointZ; // 今回のZ座標を保存
            }
            else
            {
                _center = _areaUnderPointZ; // 今回のZ座標を保存
            }

        }

        //歩けない場所を壁で埋める
        WallCreate(_mapController);

        return true;

        //StartCoroutine(Spawner());

    }

    IEnumerator Spawner()
    {
        int _sponNum = Random.Range(5, 20);

        yield return new WaitForSeconds(0.5f);
        //プレイヤーをスポーンさせる
        _sponPlayer.InitPlayerPos();
        //敵キャラをスポーンさせる
        _sponEnemy.spawn(_sponNum);
        //ボス敵をスポーンさせる
        _sponEnemy.BossSpawn(_lastMap, false);
    }

    //IEnumerator TimeUpAreaSpawner()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    //プレイヤーをスポーンさせる
    //    _sponPlayer.InitPlayerPos();
    //    //ボス敵をスポーンさせる
    //    _sponEnemy.EnemyHouse(true);
    //}

    IEnumerator LastSpawner(int x,int z)
    {
        _lastMap = true;

        yield return new WaitForSeconds(0.5f);
        //プレイヤーをスポーンさせる
        _sponPlayer.LastPlayerPos(x,z);
        //ボス敵をスポーンさせる
        _sponEnemy.BossSpawn(_lastMap, false);
    }


    void WallCreate(GameObject _mapController)
    {
        for (var x = 0; x <= _x; x++)
        {
            for (var z = 0; z <= _z; z++)
            {
                if (_areas[x, z] == null)
                {
                    _areas[x, z] = Instantiate(_obj[(int)AreaObj.wall], new Vector3(x, 1, z), Quaternion.identity);
                    _areas[x, z].transform.parent = _mapController.transform;
                    _areas[x, z].name = x + "/" + z + "(wall)";
                    areaController = MapManager._areas[x, z].GetComponent<AreaController>();
                    areaController._onWall = true;
                }
            }
        }
    }



    public int _bossMapX = 30;
    public int _bossMapZ = 30;

    public bool BossMapCreate(GameObject _mapController)
    {
        _x = _bossMapX;
        _z = _bossMapZ;

        _areas = new GameObject[_x + 2, _z + 2];

        for (var x = 1; x < _bossMapX; x++)
        {
            for (var z = 6; z < _bossMapZ; z++)
            {
                _areas[x, z] = Instantiate(_obj[(int)AreaObj.walk], new Vector3(x, 0, z), Quaternion.identity);
                _areas[x, z].name = x + "/" + z;
                _areas[x, z].transform.parent = _mapController.transform;
            }
        }

        //最初の小部屋
        for (var x = 11; x < 21; x++)
        {
            for (var z = 1; z < 6; z++)
            {
                _areas[x, z] = Instantiate(_obj[(int)AreaObj.walk], new Vector3(x, 0, z), Quaternion.identity);
                _areas[x, z].name = x + "/" + z;
                _areas[x, z].transform.parent = _mapController.transform;
            }
        }

        //壁設置
        for (var x = 0; x <= _bossMapX; x++)
        {
            for (var z = 0; z <= _bossMapZ; z++)
            {
                if (_areas[x, z] == null)
                {
                    _areas[x, z] = Instantiate(_obj[(int)AreaObj.wall], new Vector3(x, 1, z), Quaternion.identity);
                    _areas[x, z].transform.parent = _mapController.transform;
                    _areas[x, z].name = x + "/" + z + "(wall)";
                    areaController = MapManager._areas[x, z].GetComponent<AreaController>();
                    areaController._onWall = true;
                }
            }
        }

        StartCoroutine(LastSpawner(21,6));

        return true;
    }



    //public void TimeOvareMap(GameObject _mapController)
    //{
    //    _x = 10;
    //    _z = 10;

    //    _areas = new GameObject[_x + 1, _z + 1];

    //    for (var x = 1; x < _x; x++)
    //    {
    //        for (var z = 1; z < _z; z++)
    //        {
    //            _areas[x, z] = Instantiate(_obj[(int)AreaObj.walk], new Vector3(x, 0, z), Quaternion.identity);
    //            _areas[x, z].transform.parent = _mapController.transform;

    //        }
    //    }

    //    //壁設置
    //    for (var x = 0; x <= _x; x++)
    //    {
    //        for (var z = 0; z <= _z; z++)
    //        {
    //            if (_areas[x, z] == null)
    //            {
    //                //_areas[x, z] = Instantiate(_obj[(int)AreaObj.wall], new Vector3(x, 1, z), Quaternion.identity);
    //                //_areas[x, z].transform.parent = _mapController.transform;
    //                //areaController = MapManager._areas[x, z].GetComponent<AreaController>();
    //                //areaController._onWall = true;
    //            }
    //            else
    //            {

    //            }
    //        }
    //    }


    //    StartCoroutine(TimeUpAreaSpawner());
    //}

}
