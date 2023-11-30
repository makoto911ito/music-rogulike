using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RhythmController : MonoBehaviour
{
    [SerializeField] AudioClip _audio;
    public AudioSource _audioSource;

    float _bpm = 0;

    int _count = 1;
    /// <summary>与えられた音源の周波数</summary>
    int _frequency = 0;

    /// <summary></summary>
    float _time = 0;
    static public float _beat = 0;
    float _judgeTime = 0;

    /// <summary>１ビートの秒数</summary>
    float _beatTime = 0;
    float _noteTime = 0;

    static public float _moveTime = 0;

    static float _mae = 0;
    static float _usiro = 0;

    PlayerMove _playerMove;

    [SerializeField] float _addframe;

    [SerializeField] EnemyList _enemyList;

    public bool _sound = false;

    [SerializeField] float _frameNum = 2;

    [SerializeField] float _size = 0.5f;

    [SerializeField] int _fpsValue = 120;

    [SerializeField] InputAction _inputAction;

    //[SerializeField] GameManager _gameManager;

    NotesMove[] _notes;

    float _addFlame = 0;

    Keyboard _current;

    float _bad = 0;

    float _just = 0;

    [SerializeField] NotesController _notesController;

    public void Judgetiming(float audioTime, Vector2 vector2)
    {
        //Debug.Log(audioTime + "入力タイム　：　" + (_beat / _frequency) + "判定タイム");
        //Debug.Log(Mathf.Abs(audioTime - (_beat / _frequency)) + "入力差");
        //Debug.Log((_frameNum / 60) / 2 + "判定");
        //Debug.Log(_bad + "早いタイミングの計算結果");

        if (Mathf.Abs(audioTime - (_beat / _frequency)) < _just)
        {
            Debug.Log("動ける");
            _notesController.nnn(_beat / _frequency);
            _playerMove.GoMove(vector2);
            //_notesController.NoteDestroy();
        }
        else if (Mathf.Abs(audioTime - (_beat / _frequency)) < _bad)
        {
            Debug.Log("else 反応している");
            _notesController.nnn(_beat / _frequency);
            //_notesController.NoteDestroy();
        }

        ////Debug.Log("判定時間 :" + _noteDetas[0]._judgeTime);
        //float _timeDifferenc = _noteDetas[_count]._judgeTime - audioTime;
        //Debug.Log("入力時間 : " + audioTime + " 判定時間 : " + _noteDetas[_count]._judgeTime + " ノーツカウント : " + _count);

        //if (_timeDifferenc < 0)
        //{
        //    _timeDifferenc *= -1;
        //}

        //Debug.Log("入力されたタイミングと判定時間の差:" + _timeDifferenc);

        //if (_timeDifferenc <= _judgeTime)
        //{
        //    Debug.Log("動ける");
        //}
        //else
        //{
        //    Debug.Log("ミス");
        //}
    }

    private void OnEnable()
    {
        _inputAction.performed += OnPreformed;

        //InputActionを有効化させる
        _inputAction?.Enable();
    }

    private void OnDisable()
    {
        _inputAction.performed -= OnPreformed;

        //InputActionを無効化させる
        _inputAction?.Disable();
    }

    //入力感知
    void OnPreformed(InputAction.CallbackContext context)
    {
        //Debug.Log("呼ばれた");
        //float _audioTime = _audioSource.timeSamples / _frequency;
        var vector = context.ReadValue<Vector2>();
        //Debug.Log(vector + "反映された");
        float a = _audioSource.time;
        Judgetiming(a, vector);

    }

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _fpsValue;

        //SetUpSound();

        _current = Keyboard.current;

        _addFlame = _frameNum / _fpsValue;
        //Debug.Log("追加フレーム" + _addFlame);

        if (_current == null)
        {
            return;
        }
    }

    public bool NoteSetUp(GameObject JudgementPos, GameObject SpawnPos)
    {
        _notesController.GeNotetAreaPos(JudgementPos, SpawnPos);

        var num = (_audio.length / (Beat(1) / _frequency)) + 1;
        //Debug.Log(num + "ノーツ数");
        //Debug.Log((int)num + "ノーツ数");

        _bad = (_frameNum + _addframe) / 60 / 2;

        _just = _frameNum / 60 / 2;

        for (int i = 1; i < num; i++)
        {
            //Debug.Log("notesController側にタイミングのデータを渡している");
            _beat = Beat(i);
            var count = i - 1;
            _notesController.NotesGenerate(count, _beat, _frequency, _frameNum, _judgeTime);
        }

        return true;

        //_gameManager.GoSound();
    }

    public void SetUpSound()
    {
        if (_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }

        //与えられた音源の周波数を確認
        _frequency = _audio.frequency;

        _bpm = BpmChecker.AnalyzeBpm(_audio, _frequency);
        if (_bpm < 0)//帰ってきたBPMの値が０よりも小さい値であれば音源が入っていないことになるそれをエラーとしてデバックする
        {
            //Debug.LogError("AudioClip is null.");
            return;
        }

        //Debug.Log(_bpm);

        _beatTime = 60 / _bpm;

        _noteTime = _beatTime * _frequency;

        //Debug.Log(_frequency);

        //判定のズレの秒数を取得
        _judgeTime = _frequency * Time.fixedDeltaTime;
       // Debug.Log("判定のズレ；" + _judgeTime);

        _notesController.GetAudio(_audioSource);
    }

    /// <summary>音楽を再生する</summary>
    public void PlaySound()
    {
        //曲を流す
        _audioSource.clip = _audio;
        _audioSource.Play();
        _sound = true;
    }

    /// <summary>階層移動中に音が流れないように音楽を止めるための関数</summary>
    public void StopSound()
    {
        _sound = false;
        _audioSource.Stop();
        _count = 1;
    }

    /// <summary>プレイヤーの情報を入手するための関数</summary>
    /// <param name="player"></param>
    public void NowPlayerSpawn(PlayerMove player)
    {
        _playerMove = player.GetComponent<PlayerMove>();
        
    }

    //Debug.Log(_audioSource.timeSamples + "　タイム");
    //    float bite = 60f / (float)_bpm;
    //Debug.Log(bite + "ビート");
    //    float ans = _frequency * bite;
    //Debug.Log(ans + " サンプル/ビート");

    private void FixedUpdate()
    {
        if (_sound == true)
        {
            //Debug.Log(_audioSource.time + "タイム");
            _moveTime = _audioSource.timeSamples;//曲の経過時間を取得

            _beat = Beat(_count);//判定の間隔を取得
            _mae = (_beat - _judgeTime) / _frequency;// - (_frameNum / 60);//取得した判定の前ズレ
            _usiro = (_beat + _judgeTime) / _frequency;// + (_frameNum / 60);//取得した間隔の後ズレ
            //Debug.Log(_usiro + "ノーツの最終判定値（リズムcontroller）");

            if (_moveTime >= _beat)
            {
                Debug.Log("カウントされている");
                _enemyList.GoEnemyMove();
                _count++;
            }
        }
    }

    /// <summary>
    /// 判定の間隔を求める
    /// </summary>
    /// <param name="count">回数</param>
    /// <returns></returns>
    float Beat(float count)
    {
        return _frequency * (60 * count / _bpm);
    }
}
