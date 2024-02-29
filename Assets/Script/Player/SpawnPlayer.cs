using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーを生成するためのスクリプト
/// </summary>
public class SpawnPlayer : MonoBehaviour
{
    /// <summary>配列の一つ目の要素の場所をランダムで見るための変数</summary>
    int _randomNumX;

    /// <summary>配列の二つ目の要素の場所をランダムで見るための変数</summary>
    int _randomNumZ;

    /// <summary>スポーンできたかどうかのフラグ</summary>
    bool _spawn;

    /// <summary>プレイヤーのオブジェクトを取得</summary>
    [SerializeField] GameObject _player;

    /// <summary>移動先・前を確認、変更するためのAreaControllerスクリプトを獲得する変数</summary>
    AreaController areaController;

    //プレイヤーがスポーンしたときから曲を再生させたいからゲームマネージャーを取得
    [SerializeField] GameManager _gameManager;

    /// <summary>生成したプレイヤーを保持する変数</summary>
    GameObject _playercharacter;

    [SerializeField]
    GameObject _attackEffect;
    [SerializeField]
    GameObject _damageEffect;

    /// <summary>プレイヤーを生成する関数</summary>
    public void PlayerGenerate()
    {
        _playercharacter = Instantiate(_player, new Vector3(6, 6, 6), Quaternion.identity);
        _playercharacter.name = "Player";
        var _playerMove = _playercharacter.GetComponent<PlayerMove>();
        _playerMove.GetEffect(_attackEffect);
    }


    /// <summary>プレイヤーを開始位置へ移動する関数</summary>
    public void InitPlayerPos()
    {
        _spawn = false;

        //Debug.Log(MapManager._areas.GetLength(0) + " " + MapManager._areas.GetLength(0));

        while (_spawn == false)
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

                if (areaController._onEnemy == true || areaController._onWall == true)
                {
                    //Debug.Log("プレイヤーは生成されなかった。");
                    continue;
                }
                else
                {

                    //Debug.Log("プレイヤーを開始位置へ移動した");
                    _playercharacter.transform.position = new Vector3(MapManager._areas[_randomNumX, _randomNumZ].transform.position.x, 1.5f, MapManager._areas[_randomNumX, _randomNumZ].transform.position.z);
                    _spawn = true;

                }
            }
        }
    }

    public void LastPlayerPos(int x, int z)
    {
        while (_spawn == false)
        {
            _randomNumX = Random.Range(1, x+ 1) ;
            _randomNumZ = Random.Range(1, z + 1);

            if (MapManager._areas[_randomNumX, _randomNumZ] == null)
            {
                continue;
            }
            else if (MapManager._areas[_randomNumX, _randomNumZ] != null)
            {
                areaController = MapManager._areas[_randomNumX, _randomNumZ].GetComponent<AreaController>();
                //Debug.Log("反応している");

                if (areaController._onEnemy == true || areaController._onWall == true)
                {
                    //Debug.Log("プレイヤーは生成されなかった。");
                    continue;
                }
                else
                {

                    //Debug.Log("プレイヤーを開始位置へ移動した");
                    _playercharacter.transform.position = new Vector3(MapManager._areas[_randomNumX, _randomNumZ].transform.position.x, 1.5f, MapManager._areas[_randomNumX, _randomNumZ].transform.position.z);
                    _spawn = true;

                }
            }
        }
    }

}
