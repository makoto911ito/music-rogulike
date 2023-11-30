using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����̏�ɏ���Ă���I�u�W�F�N�g�͉������肷�邽�߂̃X�N���v�g
/// </summary>
public class AreaController : MonoBehaviour
{
    public bool _onPlayer = false;
    public bool _onEnemy = false;
    public bool _onWall = false;
    public bool _stair = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _onPlayer = true;
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Boss"))
        {
            _onEnemy = true;
        }
    }

    public void ResetStatus()
    {
        _onPlayer = false;
        _onEnemy = false;
        _onWall = false;
        _stair = false;
    }

}
