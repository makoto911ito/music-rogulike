using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RhythmController : MonoBehaviour
{
    [SerializeField] AudioClip _audio;
    [SerializeField] AudioClip _misuClip;
    public AudioSource _gemeSound;
    public AudioSource _misuSound;

    float _bpm = 0;
    /// <summary>�r�[�g�̉�</summary>
    int _count = 0;
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
    [SerializeField] EnemyAttackObjController _enemyAttackObjController;

    public bool _sound = false;

    [SerializeField] float _frameNum = 2;

    [SerializeField] float _size = 0.5f;

    [SerializeField] int _fpsValue = 120;

    [SerializeField] InputAction _inputAction;

    [SerializeField] Text _conboText;

    /// <summary>good�̉摜</summary>
    [SerializeField]
    Image _judgeImageGood;
    /// <summary>miss�̉摜</summary>
    [SerializeField]
    Image _judgeImageMiss;

    [SerializeField]
    float _judgeDisplayTime = 0.1f;

    //[SerializeField] GameManager _gameManager;

    NotesMove[] _notes;

    float _addFlame = 0;

    Keyboard _current;

    float _bad = 0;

    float _just = 0;

    /// <summary>�R���{�������Ă��邩�ǂ���</summary>
    bool _iscombo = false;
    /// <summary>�R���{��</summary>
    int _comboCount = 0;

    [SerializeField] NotesController _notesController;

    /// <summary>�~�X���������ǂ���</summary>
    bool _misufrag = false;

    List<float> _noteJudgeBeet = new List<float>();

    public void Judgetiming(float audioTime, Vector2 vector2)
    {
        //Debug.Log(audioTime + "���̓^�C���@�F�@" + (_beat / _frequency) + "����^�C��");
        //Debug.Log(Mathf.Abs(audioTime - (_beat / _frequency)) + "���͍�");
        //Debug.Log((_frameNum / 60) / 2 + "����");
        //Debug.Log(_bad + "�����^�C�~���O�̌v�Z����");

        if (Mathf.Abs(audioTime - (_beat / _frequency)) < _just)
        {
            _playerMove.GoMove(vector2);
            Debug.Log("������");
            if (_iscombo == true)
            {
                _comboCount++;
                if (_comboCount > 1)
                {
                    _conboText.text = _comboCount.ToString() + "�R���{";
                }

                if (_comboCount > 5)//�Œ�R���{����ݒ肵����ȏ�̔���̓v���C���[�̕��ɔC����
                {
                    //�v���C���[�̍U���͂�ς���
                    _playerMove.ChangPower(_comboCount);
                }
            }
            else
            {
                _iscombo = true;
            }
            _notesController.nnn(_beat / _frequency);
            //_notesController.NoteDestroy();
        }
        else if (Mathf.Abs(audioTime - (_beat / _frequency)) < _bad)
        {
            Misu();
            // �v���C���[�̍U���͂����Ƃɖ߂�
            Debug.Log("else �������Ă���");
            _notesController.nnn(_beat / _frequency);
            //_notesController.NoteDestroy();
        }
    }

    public void Misu()
    {
        _misuSound.clip = _misuClip;
        _misuSound.Play();
        StartCoroutine(JudgeImage(false));
        if (_misufrag == false)
        {
            _misufrag = true;
            _playerMove.ResetPower();
            _iscombo = false;
            _comboCount = 0;
        }
        _conboText.text = "";
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
        float inputTime = _gemeSound.time;

        var judgeTimeA = Mathf.Abs(inputTime - (_noteJudgeBeet[_count] / _frequency));
        var judgeTimeB = Mathf.Abs(inputTime - (_noteJudgeBeet[_count - 1] / _frequency));

        //Debug.Log(judgeTimeA + " " + _count + " / " + judgeTimeB + " " + _count+ "- 1") ;

        //Debug.Log("���͂�������" + inputTime);

        if(judgeTimeA > judgeTimeB)
        {
            Debug.Log("judgeTimeA > judgeTimeB �̕�");
            if(judgeTimeB < _just)
            {
                _misufrag = false;
                StartCoroutine(JudgeImage(true));
                _playerMove.GoMove(vector);
                //Debug.Log("������");
                if (_iscombo == true)
                {
                    _comboCount++;
                    if (_comboCount > 1)
                    {
                        _conboText.text = _comboCount.ToString() + "�R���{";
                    }

                    if (_comboCount >= 5)//�Œ�R���{����ݒ肵����ȏ�̔���̓v���C���[�̕��ɔC����
                    {
                        //�v���C���[�̍U���͂�ς���
                        _playerMove.ChangPower(_comboCount);
                    }
                }
                else
                {
                    _iscombo = true;
                }
                _notesController.nnn(_noteJudgeBeet[_count - 1] / _frequency);
            }
            else if (judgeTimeB < _bad)
            {
                Misu();
                // �v���C���[�̍U���͂����Ƃɖ߂�
                //Debug.Log("else �������Ă���");
                _notesController.nnn(_noteJudgeBeet[_count - 1] / _frequency);
                //_notesController.NoteDestroy();
            }
        }
        else
        {
            //Debug.Log("judgeTimeA < judgeTimeB �̕�");
            if (judgeTimeA < _just)
            {
                _misufrag = false;
                StartCoroutine(JudgeImage(true));
                _playerMove.GoMove(vector);
                //Debug.Log("������");
                if (_iscombo == true)
                {
                    _comboCount++;
                    if (_comboCount > 1)
                    {
                        _conboText.text = _comboCount.ToString() + "�R���{";
                    }

                    if (_comboCount >= 5)//�Œ�R���{����ݒ肵����ȏ�̔���̓v���C���[�̕��ɔC����
                    {
                        //�v���C���[�̍U���͂�ς���
                        _playerMove.ChangPower(_comboCount);
                    }
                }
                else
                {
                    _iscombo = true;
                }
                _notesController.nnn(_noteJudgeBeet[_count] / _frequency);
            }
            else if (judgeTimeA < _bad)
            {
                Misu();
                // �v���C���[�̍U���͂����Ƃɖ߂�
                //Debug.Log("else �������Ă���");
                _notesController.nnn(_noteJudgeBeet[_count] / _frequency);
                //_notesController.NoteDestroy();
            }
        }

        //if (Mathf.Abs(inputTime - (_beat / _frequency)) < _just)
        //{
        //    _misufrag = false;
        //    StartCoroutine(JudgeImage(true));
        //    _playerMove.GoMove(vector);
        //    //Debug.Log("������");
        //    if (_iscombo == true)
        //    {
        //        _comboCount++;
        //        if (_comboCount > 1)
        //        {
        //            _conboText.text = _comboCount.ToString() + "�R���{";
        //        }

        //        if (_comboCount >= 5)//�Œ�R���{����ݒ肵����ȏ�̔���̓v���C���[�̕��ɔC����
        //        {
        //            //�v���C���[�̍U���͂�ς���
        //            _playerMove.ChangPower(_comboCount);
        //        }
        //    }
        //    else
        //    {
        //        _iscombo = true;
        //    }
        //    _notesController.nnn(_beat / _frequency);
        //    //_notesController.NoteDestroy();
        //}
        //else if (Mathf.Abs(inputTime - (_beat / _frequency)) < _bad)
        //{
        //    Misu();
        //    // �v���C���[�̍U���͂����Ƃɖ߂�
        //    //Debug.Log("else �������Ă���");
        //    _notesController.nnn(_beat / _frequency);
        //    //_notesController.NoteDestroy();
        //}
        //Judgetiming(a, vector);
    }

    IEnumerator JudgeImage(bool judge)
    {
        //Debug.Log("�������Ă���");

        if (judge == true)
        {
            _judgeImageGood.enabled = true;
        }
        else
        {
            _judgeImageMiss.enabled = true;
        }
        yield return new WaitForSeconds(_judgeDisplayTime);

        if (judge == true)
        {
            _judgeImageGood.enabled = false;
        }
        else
        {
            _judgeImageMiss.enabled= false;
        }
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

        Debug.Log(_audio.length + " �Ȃ̕b��");
        var num = (_audio.length / (Beat(1) / _frequency)) + 1;
        //Debug.Log(num + "�m�[�c��");
        //Debug.Log((int)num + "�m�[�c��");

        _bad = (_frameNum + _addframe) / 60 / 2;

        _just = _frameNum / 60;
        //Debug.Log(_just + "�}");

        for (int i = 1; i < num; i++)
        {
            //Debug.Log("notesController���Ƀ^�C�~���O�̃f�[�^��n���Ă���");
            _beat = Beat(i);
            _noteJudgeBeet.Add(_beat);
            var count = i - 1;
            _notesController.NotesGenerate(count, _beat, _frequency, _frameNum, _judgeTime);
        }

        return true;

        //_gameManager.GoSound();
    }

    public void SetUpSound()
    {
        if (_gemeSound == null)
        {
            _gemeSound = GetComponent<AudioSource>();
        }

        //�^����ꂽ�����̎��g�����m�F
        _frequency = _audio.frequency;

        _bpm = BpmChecker.AnalyzeBpm(_audio, _frequency);
        if (_bpm < 0)//�A���Ă���BPM�̒l���O�����������l�ł���Ή����������Ă��Ȃ����ƂɂȂ邻����G���[�Ƃ��ăf�o�b�N����
        {
            //Debug.LogError("AudioClip is null.");
            return;
        }

        Debug.Log(_bpm);

        _beatTime = 60 / _bpm;

        _noteTime = _beatTime * _frequency;

        //Debug.Log(_frequency);

        //����̃Y���̕b�����擾
        _judgeTime = _frequency * Time.fixedDeltaTime;
        // Debug.Log("����̃Y���G" + _judgeTime);

        _notesController.GetAudio(_gemeSound);
    }

    /// <summary>���y���Đ�����</summary>
    public void PlaySound()
    {
        //�Ȃ𗬂�
        _gemeSound.clip = _audio;
        _gemeSound.Play();
        _sound = true;
    }

    /// <summary>�K�w�ړ����ɉ�������Ȃ��悤�ɉ��y���~�߂邽�߂̊֐�</summary>
    public void StopSound()
    {
        _sound = false;
        _gemeSound.Stop();
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
            _moveTime = _gemeSound.timeSamples;//�Ȃ̌o�ߎ��Ԃ��擾

            //_beat = Beat(_count);//����̊Ԋu���擾
            _mae = (_beat - _judgeTime) / _frequency;// - (_frameNum / 60);//�擾��������̑O�Y��
            _usiro = (_beat + _judgeTime) / _frequency;// + (_frameNum / 60);//�擾�����Ԋu�̌�Y��
            //Debug.Log(_usiro + "�m�[�c�̍ŏI����l�i���Y��controller�j");

            if (_moveTime >= _noteJudgeBeet[_count])
            {
                //Debug.Log("�J�E���g����Ă���");
                _enemyList.GoEnemyMove();
                _enemyAttackObjController.GoAObj();
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
