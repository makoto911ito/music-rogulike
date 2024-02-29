using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerModel;

public class PlayerPresenter : MonoBehaviour
{
    /// <summary>�v���C���[��HP�Ɋւ��ẴN���X</summary>
    Hp _playerHpModel = null;

    /// <summary>�v���C���[�̍U���͂Ɋւ��ẴN���X</summary>
    PowerModel _playerPowerModel = null;

    /// <summary>�v���C���[�̕\���Ɋւ��ẴN���X</summary>
    [SerializeField] PlayerView _playerView = null;

    //���݂̃}�b�v��ɂ���G�̏����܂Ƃ߂����X�g�̏���
    //[SerializeField] EnemyList _enemyList = null;

    /// <summary>�v���C���[��HP</summary>
    [SerializeField] int _playerHp = 1;
    public float _playerPower = 1;

    GameManager _gameManager = null;

    int _myPosX = 0;
    int _myPosZ = 0;


    public void SetLife(GameObject life, GameManager gm,GameObject playerImage)
    {
        _gameManager = gm;
        _playerView.InitView(life, _gameManager._powerText,playerImage);
        StartCoroutine(StatInit());
    }

    IEnumerator StatInit()
    {
        yield return new WaitForSeconds(1);
        Init();
    }

    public void Init()
    {
        _playerHpModel = new Hp(
            _playerHp,
            x =>
            {
                //Debug.Log(x + "PlayerPresenter��HP");
                _playerView.ChangeSliderValue(_playerHp, x);
                if (x <= 0)
                {
                    _gameManager.GameOvare(_myPosX, _myPosZ);
                    Destroy(this.gameObject);
                }
            },
            _playerView.gameObject);

        _playerPowerModel = new PowerModel(
            1,
            x =>
            {
                _playerPower = x;
                _playerView.ChangePowerView(x);
            },
            _playerView.gameObject);
    }





    public void SaveMyPosition(int posX, int posZ)
    {
        _myPosX = posX;
        _myPosZ = posZ;

        //Debug.Log(_myPosX + " / " + _myPosZ);
    }

    //public void Attack(int posx, int posz)
    //{
    //    _enemyList.CheckEnemy(posx, posz, _playerModel._pPower);
    //}

    public void EnemyAttack(int ePower)
    {
        if (_playerHpModel == null)
        {
            //Debug.Log("PlayerModel��null�ł�");
        }

        //Debug.Log("�G����_���[�W��H����Ă���");
        _playerHpModel.Damage(ePower);

    }

    public void ChangPower(float deta)
    {
        if (_playerPowerModel == null) { }
        else
        {
            _playerPowerModel.ChangPower(deta);
        }
    }
}
