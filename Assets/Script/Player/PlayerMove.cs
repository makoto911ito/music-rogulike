using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///プレイヤーの動きを管理するスクリプト
/// </summary>
public class PlayerMove : MonoBehaviour
{
    /// <summary>現在地のｘ軸</summary>
    int _pointX = 0;
    /// <summary>現在地のｚ軸</summary>
    int _pointZ = 0;

    /// <summary>現マップに存在する敵のリストを取得</summary>
    EnemyList _EnemyList;

    RhythmController _rizumuController;

    AudioSource _audioSource;

    float _audioTime = 0;

    [SerializeField] PlayerPresenter _playerPresenter = null;

    /// <summary>移動先・前を確認、変更するためのAreaControllerスクリプトを獲得する変数</summary>
    AreaController areaController;

    GameManager _gameManager;

    GameObject _attacEffect;
    Animator _attacAnimator;
    string _effectname;

    int _moveNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        var gm = GameObject.Find("GameManager");
        _gameManager = gm.GetComponent<GameManager>();
        if (_gameManager == null)
        {
            //Debug.Log("GameManagerが取得できていません");
        }

        var life = GameObject.Find("heartSlider");
        if (life == null)
        {
            //Debug.Log("heartSliderが取得出来てません");
        }
        
        var player = transform.GetChild(1).gameObject;
        //Debug.Log(player.gameObject.name);

        _playerPresenter.SetLife(life, _gameManager,player);

        var enemyList = GameObject.Find("EnemyList");

        _EnemyList = enemyList.GetComponent<EnemyList>();

        var rhythmController = GameObject.Find("RhythmController");

        if (rhythmController == null)
        {
            //Debug.Log("RhythmControllerが取得出来ていない");
        }

        _audioSource = rhythmController.GetComponent<AudioSource>();

        _rizumuController = rhythmController.GetComponent<RhythmController>();
    }

    public void GetEffect(GameObject aEffect)
    {
        _attacEffect = Instantiate(aEffect);
        _attacAnimator = _attacEffect.GetComponent<Animator>();
    }

    /// <summary>
    /// プレイヤーを動かす関数
    /// </summary>
    /// <param name="moveNum"></param>
    public void GoMove(Vector2 moveNum)
    {
        var x = (int)moveNum.x;
        var y = (int)moveNum.y;
        //Debug.Log("渡された移動される値 " + moveNum);

        if (y == 0)
        {
            //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
            areaController = MapManager._areas[_pointX + x, _pointZ].GetComponent<AreaController>();

            //移動先の情報によって行動を決める
            if (areaController._onWall == true)
            {

            }
            else if (areaController._onEnemy == true)
            {
                //Debug.Log("攻撃");
                _attacEffect.SetActive(true);
                _attacEffect.transform.position = new Vector3(_pointX + x, 1, this.transform.position.z);
                _attacAnimator.Play("AttackEffect");
                _EnemyList.CheckEnemy(_pointX + x, _pointZ, _playerPresenter._playerPower);
                //_playerPresenter.Attack(_pointX + 1, _pointZ);
            }
            else
            {
                areaController._onPlayer = true;
                areaController = MapManager._areas[_pointX, _pointZ].GetComponent<AreaController>();
                areaController._onPlayer = false;
                //Debug.Log("横に移動するコードが反応している");
                this.transform.position = new Vector3(MapManager._areas[_pointX + x, _pointZ].transform.position.x, this.transform.position.y, this.transform.position.z);
                _pointX = _pointX + x;
                StairCheck();
                //_rizumuBase._bottu = true;
            }
        }
        else if (x == 0)
        {
            //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
            areaController = MapManager._areas[_pointX, _pointZ + y].GetComponent<AreaController>();


            //移動先の情報によって行動を決める
            if (areaController._onWall == true)
            {

            }
            else if (areaController._onEnemy == true)
            {
                //Debug.Log("攻撃");
                _attacEffect.SetActive(true);
                _attacEffect.transform.position = new Vector3(this.transform.position.x, 1, _pointZ + y);
                _attacAnimator.Play("AttackEffect");
                _EnemyList.CheckEnemy(_pointX, _pointZ + y, _playerPresenter._playerPower);
                //_playerPresenter.Attack(_pointX + 1, _pointZ);
            }
            else
            {
                areaController._onPlayer = true;
                areaController = MapManager._areas[_pointX, _pointZ].GetComponent<AreaController>();
                areaController._onPlayer = false;
                //Debug.Log("縦に移動するコードが反応している");
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, MapManager._areas[_pointX, _pointZ + y].transform.position.z);
                _pointZ = _pointZ + y;
                StairCheck();
                //_rizumuBase._bottu = true;
            }
        }
        _playerPresenter.SaveMyPosition(_pointX, _pointZ);
    }

    //IEnumerator Effect()
    //{
    //   // yield return new WaitForSeconds(_attacAnimator, 0);
    //}

    /// <summary>
    /// 攻撃力を上げるための関数
    /// </summary>
    public void ChangPower(int conboCount)
    {
        if (conboCount > 10)
        {
            _playerPresenter.ChangPower(2);
        }
        else
        {
            _playerPresenter.ChangPower(1.5f);
        }
    }

    public void ResetPower()
    {
        _playerPresenter.ChangPower(1);
    }

    /// <summary>
    /// 今いる場所が階段の上かどうか判断する関数
    /// </summary>
    void StairCheck()
    {
        //Debug.Log("階段チェック");
        areaController = MapManager._areas[_pointX, _pointZ].GetComponent<AreaController>();

        //階段の上であれば次の階層に移動する
        if (areaController._stair == true)
        {
            //Debug.Log("階段の上にいる");
            //ここに次の階層に移動するコードを書く
            _gameManager.NextMapGenerate(1, false);

        }
    }

    /// <summary>プレイヤーを動かすための関数 </summary>
    //void Pmove()
    //{
    //    var velox = Input.GetAxisRaw("Horizontal");
    //    var vertical = Input.GetAxisRaw("Vertical");


    //    if (Input.GetButtonDown("Horizontal"))
    //    {
    //        if (_rizumuBase._moveFlag == true && _rizumuBase._bottu == false)
    //        {
    //            if (velox > 0)
    //            {
    //                //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
    //                areaController = MapManager._areas[_pointX + 1, _pointZ].GetComponent<AreaController>();

    //                //移動先の情報によって行動を決める
    //                if (areaController._onWall == true)
    //                {

    //                }
    //                else if (areaController._onEnemy == true)
    //                {
    //                    Debug.Log("攻撃");
    //                    _EnemyList.CheckEnemy(_pointX + 1, _pointZ, 1);
    //                    //_playerPresenter.Attack(_pointX + 1, _pointZ);
    //                }
    //                else
    //                {
    //                    areaController._onPlayer = true;
    //                    areaController = MapManager._areas[_pointX, _pointZ].GetComponent<AreaController>();
    //                    areaController._onPlayer = false;
    //                    this.transform.position = new Vector3(MapManager._areas[_pointX + 1, _pointZ].transform.position.x, this.transform.position.y, this.transform.position.z);
    //                    _pointX = _pointX + 1;
    //                    _rizumuBase._bottu = true;
    //                }

    //                //this.transform.position = new Vector3(MapManager._areas[_pointX + 1, _pointZ].transform.position.x, MapManager._areas[_pointX + 1, _pointZ].transform.position.y, MapManager._areas[_pointX + 1, _pointZ].transform.position.z);
    //            }
    //            else if (velox < 0)
    //            {
    //                areaController = MapManager._areas[_pointX - 1, _pointZ].GetComponent<AreaController>();

    //                //移動先の情報によって行動を決める
    //                if (areaController._onWall == true)
    //                {

    //                }
    //                else if (areaController._onEnemy == true)
    //                {
    //                    Debug.Log("攻撃");
    //                    _EnemyList.CheckEnemy(_pointX - 1, _pointZ, 1);
    //                    //_playerPresenter.Attack(_pointX - 1, _pointZ);
    //                }
    //                else
    //                {
    //                    areaController._onPlayer = true;
    //                    areaController = MapManager._areas[_pointX, _pointZ].GetComponent<AreaController>();
    //                    areaController._onPlayer = false;
    //                    this.transform.position = new Vector3(MapManager._areas[_pointX - 1, _pointZ].transform.position.x, this.transform.position.y, this.transform.position.z);
    //                    _pointX = _pointX - 1;
    //                    _rizumuBase._bottu = true;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("MISS");
    //            _rizumuBase._bottu = true;
    //        }

    //    }

    //    if (Input.GetButtonDown("Vertical"))
    //    {
    //        if (_rizumuController._moveFlag == true && _rizumuController._bottu == false)
    //        {
    //            if (vertical > 0)
    //            {
    //                //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
    //                areaController = MapManager._areas[_pointX, _pointZ + 1].GetComponent<AreaController>();

    //                //移動先の情報によって行動を決める
    //                if (areaController._onEnemy == true)
    //                {
    //                    //攻撃
    //                    Debug.Log("攻撃");
    //                    _EnemyList.CheckEnemy(_pointX, _pointZ + 1, 1);
    //                    //_playerPresenter.Attack(_pointX, _pointZ + 1);
    //                }
    //                else if (areaController._onWall == true)
    //                {
    //                    //破壊するかも
    //                }
    //                else
    //                {
    //                    areaController._onPlayer = true;
    //                    areaController = MapManager._areas[_pointX, _pointZ].GetComponent<AreaController>();
    //                    areaController._onPlayer = false;
    //                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, MapManager._areas[_pointX, _pointZ + 1].transform.position.z);
    //                    _pointZ = _pointZ + 1;
    //                    _rizumuBase._bottu = true;
    //                }

    //            }
    //            else if (vertical < 0)
    //            {
    //                //行きたい方向の情報を確認したいので移動先のスクリプトを取得する
    //                areaController = MapManager._areas[_pointX, _pointZ - 1].GetComponent<AreaController>();

    //                //移動先の情報によって行動を決める
    //                if (areaController._onEnemy == true)
    //                {
    //                    Debug.Log("攻撃");
    //                    _EnemyList.CheckEnemy(_pointX, _pointZ- 1, 1);
    //                    //_playerPresenter.Attack(_pointX, _pointZ - 1);
    //                }
    //                else if (areaController._onWall == true)
    //                {

    //                }
    //                else
    //                {
    //                    areaController._onPlayer = true;
    //                    areaController = MapManager._areas[_pointX, _pointZ].GetComponent<AreaController>();
    //                    areaController._onPlayer = false;
    //                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, MapManager._areas[_pointX, _pointZ - 1].transform.position.z);
    //                    _pointZ = _pointZ - 1;
    //                    _rizumuBase._bottu = true;
    //                }

    //            }
    //        }
    //        else
    //        {
    //            Debug.Log("MISS");
    //            _rizumuBase._bottu = true;
    //        }
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        _rizumuController.NowPlayerSpawn(this);

        //Debug.Log("プレイヤーが持っている配列の最大数" + MapManager._areas.GetLength(0) + " " + MapManager._areas.GetLength(0));

        for (int x = 0; x < MapManager._x; x++)
        {
            for (int z = 0; z < MapManager._z; z++)
            {
                if (MapManager._areas[x, z] == collision.gameObject)
                {
                    //現在のプレイヤーの位置を調べる
                    _pointX = x;
                    _pointZ = z;
                    //Debug.Log("現在の配列番号" + _pointX + "x" + " , " + _pointZ + "z");
                    _playerPresenter.SaveMyPosition(_pointX, _pointZ);
                }
            }
        }
    }
}
