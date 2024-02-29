using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesMove : MonoBehaviour
{
    //判定ラインの座標
    Transform endMarker;

    //スピード
    [SerializeField] public float _noteSpeed = 0.5F;

    AudioSource _audioSource;
    NotesController _notesController;

    /// <summary>このノーツの判定時間（ジャスト）</summary>
    float _time;
    /// <summary>このノーツの最終判定時間</summary>
    float _delay = 0;

    bool _go;

    /// <summary>
    /// ノーツを動かすための準備
    /// </summary>
    /// <param name="spon">ノーツを生成した場所</param>
    /// <param name="point">ノーツを判定する場所</param>
    /// <param name="time">ノーツの判定時間</param>
    /// <param name="delay">ノーツの最終判定時間</param>
    /// <param name="audioSource"></param>
    /// <param name="notesController"></param>
    public void SetUp(GameObject point, float time, float delay, AudioSource audioSource, NotesController notesController)
    {
        _notesController = notesController;
        endMarker = point.transform;
        _time = time;
        _delay = delay;
        _audioSource = audioSource;
        _go = true;
    }


    //private void FixedUpdate()
    //{
    //    if (_go == true)
    //    {
    //        if (endMarker.position.x - endMarker.position.x * _noteSpeed * (_time - _audioSource.time) > 0)
    //        {

    //            transform.position = new Vector3(endMarker.position.x - endMarker.position.x * _noteSpeed * (_time - _audioSource.time), transform.position.y, -7);

    //            if (endMarker.position.x - endMarker.position.x * _noteSpeed * (_delay - _audioSource.time) > endMarker.position.x)
    //            {
    //               _notesController.goDestroy(int.Parse(this.name));
    //            }

    //        }
    //        else
    //        {

    //        }
    //    }
    //}

    private void Update()
    {
        if (_go == true)
        {
            if (endMarker.position.x - endMarker.position.x * _noteSpeed * (_time - _audioSource.time) > 0)
            {

                transform.position = new Vector3(endMarker.position.x - endMarker.position.x * _noteSpeed * (_time - _audioSource.time), transform.position.y, -7);

                if (endMarker.position.x - endMarker.position.x * _noteSpeed * (_delay - _audioSource.time) > endMarker.position.x)
                {
                    _notesController.goDestroy(int.Parse(this.name));
                }

            }
            else
            {

            }
        }
    }
}
