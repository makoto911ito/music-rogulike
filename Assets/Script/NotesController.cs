using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class NoteData
{
    /// <summary>�m�[�c�I�u�W�F�N�g</summary>
    public NotesMove _noteData { get; set; }

    /// <summary>�m�[�c�̔��莞��</summary>
    public float _noteTime { get; set; }

    /// <summary>�m�[�c�̍ŏI����^�C�~���O</summary>
    public float _delayTime { get; set; }

    /// <summary>�m�[�c�̔ԍ�</summary>
    public int _noteNum { get; set; } 
}

public class NotesController : MonoBehaviour
{

    List<NoteData> _noteDetas = new List<NoteData>();

    /// <summary>�m�[�c�𐶐�����ꏊ</summary>
    GameObject _noteGeneratePos;
    /// <summary>�m�[�c�𔻒肷��ꏊ</summary>
    GameObject _noteJudgementPos;
    [SerializeField]
    NotesMove _note;

    [SerializeField]
    RhythmController _hythmController;

    AudioSource _audioSource;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="beat">�m�[�c�̔���^�C�~���O</param>
    /// <param name="frequency">�Ȃ̎��g��</param>
    /// <param name="frameNum">�t���[�����[�g</param>
    /// <param name="judgeTime"></param>
    /// <param name="audioSource">�I�[�f�B�I�\�[�X</param>
    public void NotesGenerate(int num, float beat, float frequency, float frameNum, float judgeTime)
    {
        //Debug.Log("�m�[�c�𐶐����Ă���");
        NoteData data = new NoteData();
        data._noteTime = beat / frequency;
        data._delayTime = (beat + judgeTime) / frequency + (frameNum / 60);
        data._noteData = Instantiate(_note, _noteGeneratePos.transform);
        data._noteNum = num;
        data._noteData.name = num.ToString();
        data._noteData.SetUp(_noteJudgementPos, data._noteTime, data._delayTime, _audioSource,this);
        _noteDetas.Add(data);
    }

    /// <summary>
    /// �m�[�c�������ʒu�ɖ߂�
    /// </summary>
    public bool NoteReset()
    {
        foreach(NoteData note in _noteDetas)
        {
            note._noteData.gameObject.SetActive(true);
            note._noteData.gameObject.transform.position = _noteGeneratePos.transform.position;
        }

        return true;
    }

    public void GetAudio(AudioSource audioSource)
    {
        _audioSource = audioSource;
    }

    public void GeNotetAreaPos(GameObject JudgementPos,GameObject GeneratePos)
    {
        _noteJudgementPos = JudgementPos;
        _noteGeneratePos = GeneratePos;
    }

    public void goDestroy(int noteNum)
    {
        _noteDetas[noteNum]._noteData.gameObject.SetActive(false);
        _hythmController.Misu();
    }

    public void nnn(float bete)
    {
        for (int i = 0; i < _noteDetas.Count; i++)
        {
            if(_noteDetas[i]._noteTime == bete)
            {
                _noteDetas[i]._noteData.gameObject.SetActive(false);
                break;
            }
        }

    }

    public void NoteDestroy(int noteNum)
    {
        var noteobj = _noteDetas[noteNum]._noteData.gameObject;
        //_noteDetas.Remove(_noteDetas[noteNum]);
        noteobj.SetActive(false);
    }
}
