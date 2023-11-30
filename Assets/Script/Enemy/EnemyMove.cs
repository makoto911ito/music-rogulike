using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EMove
{
    A,
    B,
}

abstract class MoveBase
{
    public abstract void Move();
}

public class EnemyMove : MonoBehaviour
{
    /// <summary></summary>
    [SerializeField] int _count;
    /// <summary></summary>
    [SerializeField] bool _change;
    /// <summary>スポーンしたＸ座標を取得するための変数</summary>
    public int _pointX;
    /// <summary>スポーンしたＺ座標を取得するための変数</summary>
    public int _pointZ;

    /// <summary>プレイヤー仕様のプレゼンターを取得するための変数</summary>
    PlayerPresenter _playerPresenter;

    /// <summary>敵仕様のプレゼンターを参照するための変数</summary>
    [SerializeField] EnemyPresenter _enemyPresenter = null;

    public bool _canMove = false;

    GameManager _gameManager;

    EnemyList _enemyList;

    bool _once = true;

    //ここで行動の変化を管理する
    private MoveBase _moveType;
    public EMove EMove
    {
        get { return _eMove; }
        set
        {
            switch (value)
            {
                case EMove.A:
                    _moveType = new EnemyMoveBaseA(this, _playerPresenter);
                    break;
                case EMove.B:
                    _moveType = new EnemyMoveBaseB(this, _playerPresenter);
                    break;
            }
            _eMove = value;
        }
    }
    /// <summary>タイプによって動きを変えるそのタイプを管理する変数</summary>
    [SerializeField] EMove _eMove = EMove.A;

    private void Start()
    {
        var gm = GameObject.Find("GameManager");
        _gameManager = gm.GetComponent<GameManager>();

        //プレイヤーの情報を取得
        var gameObject = GameObject.Find("Player");
        if (gameObject == null)
        {
            Debug.Log("プレイヤーを取得できませんでした");
        }

        _playerPresenter = gameObject.GetComponent<PlayerPresenter>();

        _enemyPresenter.GetLisut();
        _enemyPresenter.Init();


    }

    public void MoveEnemy()
    {
        Debug.Log("動けの命令を受け取った");
        //敵を動かす
        _moveType.Move();
    }

    public void Attack()
    {
        //攻撃をする
        _playerPresenter.EnemyAttack(1);
    }


    public void DeleteEnemy()
    {
        var areaController = MapManager._areas[_pointX, _pointZ].GetComponent<AreaController>();
        areaController._onEnemy = false;

        if (gameObject.tag == "Boss")
        {
            _gameManager.DetBoosEnemy();
        }

        if (gameObject.tag == "GameBoss")
        {
            _gameManager.DetGameBoosEnemy();
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        for (int x = 0; x < MapManager._x; x++)
        {
            for (int z = 0; z < MapManager._z; z++)
            {
                if (MapManager._areas[x, z] == collision.gameObject)
                {
                    //現在の位置を調べる
                    _pointX = x;
                    _pointZ = z;
                    //Debug.Log("現在の配列番号" + _pointX + " , " + _pointZ);
                    if(_once != false)
                    {
                        EMove = _eMove;
                        _once = false;
                    }
                    _canMove = true;
                }
            }
        }
    }
}
