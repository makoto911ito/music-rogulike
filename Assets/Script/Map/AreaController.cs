using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自分の上に乗っているオブジェクトは何か判定するためのスクリプト
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
