using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    /// <summary>階段生成するスクリプト</summary>
    [SerializeField]
    StairPointCreate _stairPointCreate;
    /// <summary>操作入力のタイミング、流れる曲を管理するスクリプト</summary>
    [SerializeField]
    RhythmController _rhythmController;
    /// <summary>ダンジョンを生成するスクリプト</summary>
    [SerializeField]
    MapManager _mapManager;
    /// <summary>生成したダンジョンを管理するオブジェクト</summary>
    [SerializeField]
    GameObject _mapController;
    /// <summary>読み込み待機中に表示するイメージ</summary>
    [SerializeField]
    Image _loadImage;
    /// <summary>ノーツを管理するスクリプト</summary>
    [SerializeField] NotesController _notesController;
    /// <summary>敵の生成を管理するスクリプト</summary>
    [SerializeField]
    SpawnEnemy _spawnEnemy;
    /// <summary>プレイヤーの生成、初期座標の変更を管理するスクリプト</summary>
    [SerializeField]
    SpawnPlayer _spawnPlayer;

    [SerializeField]
    public Text _powerText;

    AreaController _areaController;

    //生成するフロアの最大値と最小値
    [SerializeField] int _maxFloor = 4;
    [SerializeField] int _minFloor = 2;

    [SerializeField] int _randomFloorNum = 0;

    /// <summary>テスト用のフラグ</summary>
    [SerializeField] bool _tesutFrag = false;

    /// <summary>階段を使った回数</summary>
    [SerializeField] public int _floorcount = 1;

    /// <summary>リザルト画面で表示するクリアか否かの判定</summary>
    public string _gameResult;

    /// <summary>クリアしたの判定</summary>
    public bool _isVictory = false;

    /// <summary>挑戦している時間</summary>
    public float _playTime = 0;

    /// <summary>ダンジョンに潜っているかどうか</summary>
    public bool _nowPlay = false;

    public bool _timuUp = false;

    bool _noteGenerate = false;

    [SerializeField] EnemyList _enemyList;

    bool _noteSceneLoad = true;

    [SerializeField] GameObject _camer;

    [SerializeField] Text _text;

    public void Awake()
    {
        LoadMap(true);
        GeneratePlayer();
        InitMap(false);
    }

    public void Start()
    {
        _randomFloorNum = Random.Range(_minFloor,_maxFloor);
    }

    private void Update()
    {
        if (_nowPlay == true)
        {
            _playTime += Time.deltaTime;
        }

        //if (_rhythmController._sound == true)
        //{
        //    if (_rhythmController._audioSource.isPlaying == false)
        //    {
        //        _rhythmController._sound = false;
        //        Debug.Log("反応している");
        //        var obj = GameObject.Find("door");
        //        Destroy(obj);
        //        _timuUp = true;
        //        MapGenerate(1, _timuUp);
        //    }
        //}

        if (_noteGenerate == true)
        {
            GoSound();
            _noteGenerate = false;
        }
    }

    /// <summary>プレイヤーの生成</summary>
    void GeneratePlayer()
    {
        _spawnPlayer.PlayerGenerate();
    }

    /// <summary>ゲーム開始の流れ</summary>
    public void InitMap(bool BossMap)
    {
        bool genereatCheck = false;

        if (BossMap == false)
        {
            //ダンジョンを生成し完了したらbool型のフラグ（true）をもらう
            genereatCheck = _mapManager.MapCreate(_mapController);
        }
        else
        {
            genereatCheck = _mapManager.BossMapCreate(_mapController);
        }

        //ダンジョンの生成が完了していたらプレイヤーや敵オブジェクトなどを配置する
        if (genereatCheck == true)
        {
            if(BossMap !=true)
            {
                //階段の生成
                _stairPointCreate.PointCreate();
                //生成したマップの状態に応じてプレイヤーの初期位置を決める
                _spawnPlayer.InitPlayerPos();
                //生成したマップの状態に応じて敵を生成する
                _spawnEnemy.spawn(10);
                _spawnEnemy.BossSpawn(BossMap,_timuUp);
                if(_noteSceneLoad != false)
                {
                    //再生する曲のテンポなど設定する
                    _rhythmController.SetUpSound();
                    //ノーツが流れるシーンを生成する
                    NoteSceneLoad();
                    _noteSceneLoad = false;
                }
                else
                {
                   _noteGenerate = _notesController.NoteReset();
                }

            }
            else
            {
                //生成したマップの状態に応じてプレイヤーの初期位置を決める
                _spawnPlayer.InitPlayerPos();
                _spawnEnemy.BossSpawn(BossMap, _timuUp);
                _noteGenerate = _notesController.NoteReset();
            }

        }
    }


    //ノーツが表示されるシーンを追加生成
    public async void NoteSceneLoad()
    {
        SceneManager.LoadScene("NoteScene", LoadSceneMode.Additive);

        Scene _noteScene = SceneManager.GetSceneByName("NoteScene");
        //Debug.Log("noteSceneは" + _noteScene.IsValid());

        GameObject JudgementPos = null;
        GameObject SpawnPos = null;

        //Debug.Log("NoteSceneLoadは呼び出されている");

        int _count = 0;
        bool _flag = true;

        while (_flag)
        {
            GameObject[] _allObject = _noteScene.GetRootGameObjects();
            //Debug.Log("入手したオブジェクトの数" + _allObject.Length);
            if (_allObject.Length > 0)
            {
                foreach (var gameobj in _allObject)
                {
                    //Debug.Log(gameobj.name + "オブジェクト取得中");

                    if (gameobj.name == "NoteJudgementPos" && JudgementPos == null)
                    {
                        //Debug.Log("NoteJudgementPosを取得");
                        JudgementPos = gameobj;
                    }
                    else if (gameobj.name == "NoteGeneratePos" && SpawnPos == null)
                    {
                        //Debug.Log("NoteGeneratePosを取得");
                        SpawnPos = gameobj;
                    }

                    if (JudgementPos != null && SpawnPos != null)
                    {
                        //Debug.Log("取得出来ている");
                        _noteGenerate = _rhythmController.NoteSetUp(JudgementPos, SpawnPos);
                        //Debug.Log("ノーツ生成完了" + _noteGenerate);
                        _flag = false;
                        break;
                    }
                }
            }
            await Task.Delay(1000);
            _count++;
            //Debug.Log(_count + "回目");
            if (_count > 20)
            {
                break;
            }
        }
    }

    //private void Start()
    //{
    //    if (_tesutFrag == false)
    //    {
    //        LoadMap(true);
    //        MapGenerate(0, false);
    //    }
    //    else
    //    {
    //        _mapManager.BossMapCreate(_mapController);
    //    }
    //    _randomFloorNum = Random.Range(_minFloor, _maxFloor + 1);
    //}

    /// <summary>
    /// マップの生成を行う
    /// </summary>
    /// <param name="count">階層を上がったと判定するためのカウント＋１</param>
    /// <param name="timeUp">エリアボスを倒して上がったのか、曲が終わったから上がったのかの判定</param>
    public void NextMapGenerate(int count, bool TimeUp)
    {
        Debug.Log("NextMapGenerate呼ばれた（次のステージだ！）");
        LoadMap(true);
        _floorcount += count;

        //一番最初のフロアであればそのままマップの生成。そうでなければ今あるマップを消去し再度マップの生成を行う
        foreach (Transform map in _mapController.transform)
        {
            _areaController = map.GetComponent<AreaController>();
            _areaController.ResetStatus();
            Destroy(map.gameObject);
            var obj = GameObject.Find("stair");
            Destroy(obj);
        }
        _enemyList.EnemyReset();

        if (_floorcount == _randomFloorNum)
        {
            Debug.Log("ボス部屋作成");
            //ボスの部屋を作る
            StartCoroutine(CreateBoosMap());
        }
        else
        {
            StartCoroutine(CreateMap());
        }
    }

    /// <summary>
    /// マップの生成と閉じている階段の設置
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateMap()
    {
        yield return new WaitForSeconds(0.5f);
        _rhythmController.StopSound();
        InitMap(false);
    }


    /// <summary>
    /// 制限時間経過後のモンスターハウス生成
    /// </summary>
    /// <returns></returns>
    //IEnumerator CreateEnemyHouse()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    _rhythmController.StopSound();
    //    _mapManager.TimeOvareMap(_mapController);
    //    _stairPointCreate.PointCreate();
    //}

    /// <summary>
    /// ボス部屋の生成
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateBoosMap()
    {
        yield return new WaitForSeconds(0.5f);
        _rhythmController.StopSound();
        InitMap(true);
    }

    /// <summary>階段を表示する</summary>
    public void DetBoosEnemy()
    {
        if (_timuUp == true)
        {
            if (_enemyList.Check() == true)
            {
                _stairPointCreate.OpenDoor();
            }
        }
        else
        {
            StartCoroutine("StairText");
            _stairPointCreate.OpenDoor();
        }
    }

    IEnumerator StairText()
    {
        _text.text = "階段が現れた";
        yield return new WaitForSeconds(0.3f);
        _text.text = "";
    }

    /// <summary>最終ボスを倒してゲームクリア表示をする関数</summary>
    public void DetGameBoosEnemy()
    {
        _nowPlay = false;
        _gameResult = "クリアしました";
        _isVictory = true;
        //_nootCanvas.SetActive(false);
        SceneManager.LoadScene("SelectScene", LoadSceneMode.Additive);
    }

    /// <summary>
    /// サウンドを流す
    /// </summary>
    public void GoSound()
    {
        LoadMap(false);
        _timuUp = false;
        _rhythmController.PlaySound();
    }


    /// <summary>
    /// マップ生成～プレイヤースポーンまでの時間にロード画面表示するための関数
    /// </summary>
    /// <param name="Loadflag">ロードしているかどうか</param>
    public void LoadMap(bool Loadflag)
    {
        if (Loadflag == true)
        {
            _nowPlay = false;
            _loadImage.enabled = true;
            //_nootCanvas.SetActive(false);
        }
        else
        {
            _loadImage.enabled = false;
            //_nootCanvas.SetActive(true);
            _nowPlay = true;
        }
    }

    /// <summary>
    /// プレイヤーが倒されたら呼ばれる関数
    /// </summary>
    public void GameOvare(int PlayerPosX, int PlayerPosZ)
    {
        _nowPlay = false;
        //Debug.Log("死亡しました");
        _areaController = MapManager._areas[PlayerPosX, PlayerPosZ].GetComponent<AreaController>();
        //Instantiate(_camer,new Vector3(PlayerPosX,0,PlayerPosZ),Quaternion.identity);
        _areaController._onPlayer = false;
        //_nootCanvas.SetActive(false);
        _gameResult = "倒されました";//表示の仕方を「（敵の名前）に倒されました」にする（できたら）
        _isVictory = false;
        SceneManager.LoadScene("SelectScene", LoadSceneMode.Additive);
    }
}
