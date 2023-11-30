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
    /// <summary>�^����ꂽ�����̎��g��</summary>
    int _frequency = 0;

    /// <summary></summary>
    float _time = 0;
    static public float _beat = 0;
    float _judgeTime = 0;

    /// <summary>�P�r�[�g�̕b��</summary>
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
        //Debug.Log(audioTime + "���̓^�C���@�F�@" + (_beat / _frequency) + "����^�C��");
        //Debug.Log(Mathf.Abs(audioTime - (_beat / _frequency)) + "���͍�");
        //Debug.Log((_frameNum / 60) / 2 + "����");
        //Debug.Log(_bad + "�����^�C�~���O�̌v�Z����");

        if (Mathf.Abs(audioTime - (_beat / _frequency)) < _just)
        {
            Debug.Log("������");
            _notesController.nnn(_beat / _frequency);
            _playerMove.GoMove(vector2);
            //_notesController.NoteDestroy();
        }
        else if (Mathf.Abs(audioTime - (_beat / _frequency)) < _bad)
        {
            Debug.Log("else �������Ă���");
            _notesController.nnn(_beat / _frequency);
            //_notesController.NoteDestroy();
        }

        ////Debug.Log("���莞�� :" + _noteDetas[0]._judgeTime);
        //float _timeDifferenc = _noteDetas[_count]._judgeTime - audioTime;
        //Debug.Log("���͎��� : " + audioTime + " ���莞�� : " + _noteDetas[_count]._judgeTime + " �m�[�c�J�E���g : " + _count);

        //if (_timeDifferenc < 0)
        //{
        //    _timeDifferenc *= -1;
        //}

        //Debug.Log("���͂��ꂽ�^�C�~���O�Ɣ��莞�Ԃ̍�:" + _timeDifferenc);

        //if (_timeDifferenc <= _judgeTime)
        //{
        //    Debug.Log("������");
        //}
        //else
        //{
        //    Debug.Log("�~�X");
        //}
    }

    private void OnEnable()
    {
        _inputAction.performed += OnPreformed;

        //InputAction��L����������
        _inputAction?.Enable();
    }

    private void OnDisable()
    {
        _inputAction.performed -= OnPreformed;

        //InputAction�𖳌���������
        _inputAction?.Disable();
    }

    //���͊��m
    void OnPreformed(InputAction.CallbackContext context)
    {
        //Debug.Log("�Ă΂ꂽ");
        //float _audioTime = _audioSource.timeSamples / _frequency;
        var vector = context.ReadValue<Vector2>();
        //Debug.Log(vector + "���f���ꂽ");
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
        //Debug.Log("�ǉ��t���[��" + _addFlame);

        if (_current == null)
        {
            return;
        }
    }

    public bool NoteSetUp(GameObject JudgementPos, GameObject SpawnPos)
    {
        _notesController.GeNotetAreaPos(JudgementPos, SpawnPos);

        var num = (_audio.length / (Beat(1) / _frequency)) + 1;
        //Debug.Log(num + "�m�[�c��");
        //Debug.Log((int)num + "�m�[�c��");

        _bad = (_frameNum + _addframe) / 60 / 2;

        _just = _frameNum / 60 / 2;

        for (int i = 1; i < num; i++)
        {
            //Debug.Log("notesController���Ƀ^�C�~���O�̃f�[�^��n���Ă���");
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

        //�^����ꂽ�����̎��g�����m�F
        _frequency = _audio.frequency;

        _bpm = BpmChecker.AnalyzeBpm(_audio, _frequency);
        if (_bpm < 0)//�A���Ă���BPM�̒l���O�����������l�ł���Ή����������Ă��Ȃ����ƂɂȂ邻����G���[�Ƃ��ăf�o�b�N����
        {
            //Debug.LogError("AudioClip is null.");
            return;
        }

        //Debug.Log(_bpm);

        _beatTime = 60 / _bpm;

        _noteTime = _beatTime * _frequency;

        //Debug.Log(_frequency);

        //����̃Y���̕b�����擾
        _judgeTime = _frequency * Time.fixedDeltaTime;
       // Debug.Log("����̃Y���G" + _judgeTime);

        _notesController.GetAudio(_audioSource);
    }

    /// <summary>���y���Đ�����</summary>
    public void PlaySound()
    {
        //�Ȃ𗬂�
        _audioSource.clip = _audio;
        _audioSource.Play();
        _sound = true;
    }

    /// <summary>�K�w�ړ����ɉ�������Ȃ��悤�ɉ��y���~�߂邽�߂̊֐�</summary>
    public void StopSound()
    {
        _sound = false;
        _audioSource.Stop();
        _count = 1;
    }

    /// <summary>�v���C���[�̏�����肷�邽�߂̊֐�</summary>
    /// <param name="player"></param>
    public void NowPlayerSpawn(PlayerMove player)
    {
        _playerMove = player.GetComponent<PlayerMove>();
        
    }

    //Debug.Log(_audioSource.timeSamples + "�@�^�C��");
    //    float bite = 60f / (float)_bpm;
    //Debug.Log(bite + "�r�[�g");
    //    float ans = _frequency * bite;
    //Debug.Log(ans + " �T���v��/�r�[�g");

    private void FixedUpdate()
    {
        if (_sound == true)
        {
            //Debug.Log(_audioSource.time + "�^�C��");
            _moveTime = _audioSource.timeSamples;//�Ȃ̌o�ߎ��Ԃ��擾

            _beat = Beat(_count);//����̊Ԋu���擾
            _mae = (_beat - _judgeTime) / _frequency;// - (_frameNum / 60);//�擾��������̑O�Y��
            _usiro = (_beat + _judgeTime) / _frequency;// + (_frameNum / 60);//�擾�����Ԋu�̌�Y��
            //Debug.Log(_usiro + "�m�[�c�̍ŏI����l�i���Y��controller�j");

            if (_moveTime >= _beat)
            {
                Debug.Log("�J�E���g����Ă���");
                _enemyList.GoEnemyMove();
                _count++;
            }
        }
    }

    /// <summary>
    /// ����̊Ԋu�����߂�
    /// </summary>
    /// <param name="count">��</param>
    /// <returns></returns>
    float Beat(float count)
    {
        return _frequency * (60 * count / _bpm);
    }
}
