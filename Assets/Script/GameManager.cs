using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    /// <summary>�K�i��������X�N���v�g</summary>
    [SerializeField]
    StairPointCreate _stairPointCreate;
    /// <summary>������͂̃^�C�~���O�A�����Ȃ��Ǘ�����X�N���v�g</summary>
    [SerializeField]
    RhythmController _rhythmController;
    /// <summary>�_���W�����𐶐�����X�N���v�g</summary>
    [SerializeField]
    MapManager _mapManager;
    /// <summary>���������_���W�������Ǘ�����I�u�W�F�N�g</summary>
    [SerializeField]
    GameObject _mapController;
    /// <summary>�ǂݍ��ݑҋ@���ɕ\������C���[�W</summary>
    [SerializeField]
    Image _loadImage;
    /// <summary>�m�[�c���Ǘ�����X�N���v�g</summary>
    [SerializeField] NotesController _notesController;
    /// <summary>�G�̐������Ǘ�����X�N���v�g</summary>
    [SerializeField]
    SpawnEnemy _spawnEnemy;
    /// <summary>�v���C���[�̐����A�������W�̕ύX���Ǘ�����X�N���v�g</summary>
    [SerializeField]
    SpawnPlayer _spawnPlayer;

    [SerializeField]
    public Text _powerText;

    AreaController _areaController;

    //��������t���A�̍ő�l�ƍŏ��l
    [SerializeField] int _maxFloor = 4;
    [SerializeField] int _minFloor = 2;

    [SerializeField] int _randomFloorNum = 0;

    /// <summary>�e�X�g�p�̃t���O</summary>
    [SerializeField] bool _tesutFrag = false;

    /// <summary>�K�i���g������</summary>
    [SerializeField] public int _floorcount = 1;

    /// <summary>���U���g��ʂŕ\������N���A���ۂ��̔���</summary>
    public string _gameResult;

    /// <summary>�N���A�����̔���</summary>
    public bool _isVictory = false;

    /// <summary>���킵�Ă��鎞��</summary>
    public float _playTime = 0;

    /// <summary>�_���W�����ɐ����Ă��邩�ǂ���</summary>
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
        //        Debug.Log("�������Ă���");
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

    /// <summary>�v���C���[�̐���</summary>
    void GeneratePlayer()
    {
        _spawnPlayer.PlayerGenerate();
    }

    /// <summary>�Q�[���J�n�̗���</summary>
    public void InitMap(bool BossMap)
    {
        bool genereatCheck = false;

        if (BossMap == false)
        {
            //�_���W�����𐶐�������������bool�^�̃t���O�itrue�j�����炤
            genereatCheck = _mapManager.MapCreate(_mapController);
        }
        else
        {
            genereatCheck = _mapManager.BossMapCreate(_mapController);
        }

        //�_���W�����̐������������Ă�����v���C���[��G�I�u�W�F�N�g�Ȃǂ�z�u����
        if (genereatCheck == true)
        {
            if(BossMap !=true)
            {
                //�K�i�̐���
                _stairPointCreate.PointCreate();
                //���������}�b�v�̏�Ԃɉ����ăv���C���[�̏����ʒu�����߂�
                _spawnPlayer.InitPlayerPos();
                //���������}�b�v�̏�Ԃɉ����ēG�𐶐�����
                _spawnEnemy.spawn(10);
                _spawnEnemy.BossSpawn(BossMap,_timuUp);
                if(_noteSceneLoad != false)
                {
                    //�Đ�����Ȃ̃e���|�Ȃǐݒ肷��
                    _rhythmController.SetUpSound();
                    //�m�[�c�������V�[���𐶐�����
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
                //���������}�b�v�̏�Ԃɉ����ăv���C���[�̏����ʒu�����߂�
                _spawnPlayer.InitPlayerPos();
                _spawnEnemy.BossSpawn(BossMap, _timuUp);
                _noteGenerate = _notesController.NoteReset();
            }

        }
    }


    //�m�[�c���\�������V�[����ǉ�����
    public async void NoteSceneLoad()
    {
        SceneManager.LoadScene("NoteScene", LoadSceneMode.Additive);

        Scene _noteScene = SceneManager.GetSceneByName("NoteScene");
        //Debug.Log("noteScene��" + _noteScene.IsValid());

        GameObject JudgementPos = null;
        GameObject SpawnPos = null;

        //Debug.Log("NoteSceneLoad�͌Ăяo����Ă���");

        int _count = 0;
        bool _flag = true;

        while (_flag)
        {
            GameObject[] _allObject = _noteScene.GetRootGameObjects();
            //Debug.Log("���肵���I�u�W�F�N�g�̐�" + _allObject.Length);
            if (_allObject.Length > 0)
            {
                foreach (var gameobj in _allObject)
                {
                    //Debug.Log(gameobj.name + "�I�u�W�F�N�g�擾��");

                    if (gameobj.name == "NoteJudgementPos" && JudgementPos == null)
                    {
                        //Debug.Log("NoteJudgementPos���擾");
                        JudgementPos = gameobj;
                    }
                    else if (gameobj.name == "NoteGeneratePos" && SpawnPos == null)
                    {
                        //Debug.Log("NoteGeneratePos���擾");
                        SpawnPos = gameobj;
                    }

                    if (JudgementPos != null && SpawnPos != null)
                    {
                        //Debug.Log("�擾�o���Ă���");
                        _noteGenerate = _rhythmController.NoteSetUp(JudgementPos, SpawnPos);
                        //Debug.Log("�m�[�c��������" + _noteGenerate);
                        _flag = false;
                        break;
                    }
                }
            }
            await Task.Delay(1000);
            _count++;
            //Debug.Log(_count + "���");
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
    /// �}�b�v�̐������s��
    /// </summary>
    /// <param name="count">�K�w���オ�����Ɣ��肷�邽�߂̃J�E���g�{�P</param>
    /// <param name="timeUp">�G���A�{�X��|���ďオ�����̂��A�Ȃ��I���������オ�����̂��̔���</param>
    public void NextMapGenerate(int count, bool TimeUp)
    {
        Debug.Log("NextMapGenerate�Ă΂ꂽ�i���̃X�e�[�W���I�j");
        LoadMap(true);
        _floorcount += count;

        //��ԍŏ��̃t���A�ł���΂��̂܂܃}�b�v�̐����B�����łȂ���΍�����}�b�v���������ēx�}�b�v�̐������s��
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
            Debug.Log("�{�X�����쐬");
            //�{�X�̕��������
            StartCoroutine(CreateBoosMap());
        }
        else
        {
            StartCoroutine(CreateMap());
        }
    }

    /// <summary>
    /// �}�b�v�̐����ƕ��Ă���K�i�̐ݒu
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateMap()
    {
        yield return new WaitForSeconds(0.5f);
        _rhythmController.StopSound();
        InitMap(false);
    }


    /// <summary>
    /// �������Ԍo�ߌ�̃����X�^�[�n�E�X����
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
    /// �{�X�����̐���
    /// </summary>
    /// <returns></returns>
    IEnumerator CreateBoosMap()
    {
        yield return new WaitForSeconds(0.5f);
        _rhythmController.StopSound();
        InitMap(true);
    }

    /// <summary>�K�i��\������</summary>
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
        _text.text = "�K�i�����ꂽ";
        yield return new WaitForSeconds(0.3f);
        _text.text = "";
    }

    /// <summary>�ŏI�{�X��|���ăQ�[���N���A�\��������֐�</summary>
    public void DetGameBoosEnemy()
    {
        _nowPlay = false;
        _gameResult = "�N���A���܂���";
        _isVictory = true;
        //_nootCanvas.SetActive(false);
        SceneManager.LoadScene("SelectScene", LoadSceneMode.Additive);
    }

    /// <summary>
    /// �T�E���h�𗬂�
    /// </summary>
    public void GoSound()
    {
        LoadMap(false);
        _timuUp = false;
        _rhythmController.PlaySound();
    }


    /// <summary>
    /// �}�b�v�����`�v���C���[�X�|�[���܂ł̎��ԂɃ��[�h��ʕ\�����邽�߂̊֐�
    /// </summary>
    /// <param name="Loadflag">���[�h���Ă��邩�ǂ���</param>
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
    /// �v���C���[���|���ꂽ��Ă΂��֐�
    /// </summary>
    public void GameOvare(int PlayerPosX, int PlayerPosZ)
    {
        _nowPlay = false;
        //Debug.Log("���S���܂���");
        _areaController = MapManager._areas[PlayerPosX, PlayerPosZ].GetComponent<AreaController>();
        //Instantiate(_camer,new Vector3(PlayerPosX,0,PlayerPosZ),Quaternion.identity);
        _areaController._onPlayer = false;
        //_nootCanvas.SetActive(false);
        _gameResult = "�|����܂���";//�\���̎d�����u�i�G�̖��O�j�ɓ|����܂����v�ɂ���i�ł�����j
        _isVictory = false;
        SceneManager.LoadScene("SelectScene", LoadSceneMode.Additive);
    }
}
