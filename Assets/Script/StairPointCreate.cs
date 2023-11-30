using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairPointCreate : MonoBehaviour
{
    /// <summary>配列の一つ目の要素の場所をランダムで見るための変数</summary>
    int _randomNumX;

    /// <summary>配列の二つ目の要素の場所をランダムで見るための変数</summary>
    int _randomNumZ;

    /// <summary>スポーンできたかどうかのフラグ</summary>
    bool _spon;

    [SerializeField]
    GameObject _stair;

    [SerializeField]
    GameObject _door;

    /// <summary>移動先・前を確認、変更するためのAreaControllerスクリプトを獲得する変数</summary>
    AreaController areaController;


    /// <summary>
    /// 階段の生成
    /// </summary>
    /// <param name="door"></param>
    public void PointCreate()
    {
        _spon = true;

        while (_spon == true)
        {
            _randomNumX = Random.Range(0, MapManager._x);
            _randomNumZ = Random.Range(0, MapManager._z);

            if (MapManager._areas[_randomNumX, _randomNumZ] == null)
            {
                continue;
            }
            else if (MapManager._areas[_randomNumX, _randomNumZ] != null)
            {
                areaController = MapManager._areas[_randomNumX, _randomNumZ].GetComponent<AreaController>();
                //Debug.Log("反応している");

                if (areaController._onWall == true)
                {
                    continue;
                }
                else
                {
                    Destroy(MapManager._areas[_randomNumX, _randomNumZ]);
                    MapManager._areas[_randomNumX, _randomNumZ] = Instantiate(_stair, new Vector3(_randomNumX, 0, _randomNumZ), Quaternion.identity);
                    //ここに階段の情報を持たせる
                    var _instantObj = Instantiate(_door, new Vector3(_randomNumX, 0.51f, _randomNumZ), Quaternion.identity);
                    _instantObj.name = "door";
                    _spon = false;
                }
            }
        }
    }

    public void OpenDoor()
    {
        var doorImage = GameObject.Find("door");
        Destroy(doorImage);
        areaController = MapManager._areas[_randomNumX, _randomNumZ].GetComponent<AreaController>();
        areaController._stair = true;
        Debug.Log(areaController._stair);
    }
}
