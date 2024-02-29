using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class NoteData
{
    /// <summary>ノーツオブジェクト</summary>
    public NotesMove _noteData { get; set; }

    /// <summary>ノーツの判定時間</summary>
    public float _noteTime { get; set; }

    /// <summary>ノーツの最終判定タイミング</summary>
    public float _delayTime { get; set; }

    /// <summary>ノーツの番号</summary>
    public int _noteNum { get; set; } 
}

public class NotesController : MonoBehaviour
{

    List<NoteData> _noteDetas = new List<NoteData>();

    /// <summary>ノーツを生成する場所</summary>
    GameObject _noteGeneratePos;
    /// <summary>ノーツを判定する場所</summary>
    GameObject _noteJudgementPos;
    [SerializeField]
    NotesMove _note;

    [SerializeField]
    RhythmController _hythmController;

    AudioSource _audioSource;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="beat">ノーツの判定タイミング</param>
    /// <param name="frequency">曲の周波数</param>
    /// <param name="frameNum">フレームレート</param>
    /// <param name="judgeTime"></param>
    /// <param name="audioSource">オーディオソース</param>
    public void NotesGenerate(int num, float beat, float frequency, float frameNum, float judgeTime)
    {
        //Debug.Log("ノーツを生成している");
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
    /// ノーツを初期位置に戻す
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
