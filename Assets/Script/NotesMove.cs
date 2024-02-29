using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesMove : MonoBehaviour
{
    //���胉�C���̍��W
    Transform endMarker;

    //�X�s�[�h
    [SerializeField] public float _noteSpeed = 0.5F;

    AudioSource _audioSource;
    NotesController _notesController;

    /// <summary>���̃m�[�c�̔��莞�ԁi�W���X�g�j</summary>
    float _time;
    /// <summary>���̃m�[�c�̍ŏI���莞��</summary>
    float _delay = 0;

    bool _go;

    /// <summary>
    /// �m�[�c�𓮂������߂̏���
    /// </summary>
    /// <param name="spon">�m�[�c�𐶐������ꏊ</param>
    /// <param name="point">�m�[�c�𔻒肷��ꏊ</param>
    /// <param name="time">�m�[�c�̔��莞��</param>
    /// <param name="delay">�m�[�c�̍ŏI���莞��</param>
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
