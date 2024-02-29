using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�𐶐����邽�߂̃X�N���v�g
/// </summary>
public class SpawnPlayer : MonoBehaviour
{
    /// <summary>�z��̈�ڂ̗v�f�̏ꏊ�������_���Ō��邽�߂̕ϐ�</summary>
    int _randomNumX;

    /// <summary>�z��̓�ڂ̗v�f�̏ꏊ�������_���Ō��邽�߂̕ϐ�</summary>
    int _randomNumZ;

    /// <summary>�X�|�[���ł������ǂ����̃t���O</summary>
    bool _spawn;

    /// <summary>�v���C���[�̃I�u�W�F�N�g���擾</summary>
    [SerializeField] GameObject _player;

    /// <summary>�ړ���E�O���m�F�A�ύX���邽�߂�AreaController�X�N���v�g���l������ϐ�</summary>
    AreaController areaController;

    //�v���C���[���X�|�[�������Ƃ�����Ȃ��Đ�������������Q�[���}�l�[�W���[���擾
    [SerializeField] GameManager _gameManager;

    /// <summary>���������v���C���[��ێ�����ϐ�</summary>
    GameObject _playercharacter;

    [SerializeField]
    GameObject _attackEffect;
    [SerializeField]
    GameObject _damageEffect;

    /// <summary>�v���C���[�𐶐�����֐�</summary>
    public void PlayerGenerate()
    {
        _playercharacter = Instantiate(_player, new Vector3(6, 6, 6), Quaternion.identity);
        _playercharacter.name = "Player";
        var _playerMove = _playercharacter.GetComponent<PlayerMove>();
        _playerMove.GetEffect(_attackEffect);
    }


    /// <summary>�v���C���[���J�n�ʒu�ֈړ�����֐�</summary>
    public void InitPlayerPos()
    {
        _spawn = false;

        //Debug.Log(MapManager._areas.GetLength(0) + " " + MapManager._areas.GetLength(0));

        while (_spawn == false)
        {
            _randomNumX = Random.Range(0, MapManager._x);
            _randomNumZ = Random.Range(0, MapManager._z);

            if (MapManager._areas[_randomNumX, _randomNumZ] == null)
            {
                continue;
            }
            else if (MapManager._areas[_randomNumX, _randomNumZ] != null)
            {
                areaController = MapManager._areas[_randomNumX, _randomNumZ].GetComponent<AreaController>();
                //Debug.Log("�������Ă���");

                if (areaController._onEnemy == true || areaController._onWall == true)
                {
                    //Debug.Log("�v���C���[�͐�������Ȃ������B");
                    continue;
                }
                else
                {

                    //Debug.Log("�v���C���[���J�n�ʒu�ֈړ�����");
                    _playercharacter.transform.position = new Vector3(MapManager._areas[_randomNumX, _randomNumZ].transform.position.x, 1.5f, MapManager._areas[_randomNumX, _randomNumZ].transform.position.z);
                    _spawn = true;

                }
            }
        }
    }

    public void LastPlayerPos(int x, int z)
    {
        while (_spawn == false)
        {
            _randomNumX = Random.Range(1, x+ 1) ;
            _randomNumZ = Random.Range(1, z + 1);

            if (MapManager._areas[_randomNumX, _randomNumZ] == null)
            {
                continue;
            }
            else if (MapManager._areas[_randomNumX, _randomNumZ] != null)
            {
                areaController = MapManager._areas[_randomNumX, _randomNumZ].GetComponent<AreaController>();
                //Debug.Log("�������Ă���");

                if (areaController._onEnemy == true || areaController._onWall == true)
                {
                    //Debug.Log("�v���C���[�͐�������Ȃ������B");
                    continue;
                }
                else
                {

                    //Debug.Log("�v���C���[���J�n�ʒu�ֈړ�����");
                    _playercharacter.transform.position = new Vector3(MapManager._areas[_randomNumX, _randomNumZ].transform.position.x, 1.5f, MapManager._areas[_randomNumX, _randomNumZ].transform.position.z);
                    _spawn = true;

                }
            }
        }
    }

}
