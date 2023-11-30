using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairPointCreate : MonoBehaviour
{
    /// <summary>�z��̈�ڂ̗v�f�̏ꏊ�������_���Ō��邽�߂̕ϐ�</summary>
    int _randomNumX;

    /// <summary>�z��̓�ڂ̗v�f�̏ꏊ�������_���Ō��邽�߂̕ϐ�</summary>
    int _randomNumZ;

    /// <summary>�X�|�[���ł������ǂ����̃t���O</summary>
    bool _spon;

    [SerializeField]
    GameObject _stair;

    [SerializeField]
    GameObject _door;

    /// <summary>�ړ���E�O���m�F�A�ύX���邽�߂�AreaController�X�N���v�g���l������ϐ�</summary>
    AreaController areaController;


    /// <summary>
    /// �K�i�̐���
    /// </summary>
    /// <param name="door"></param>
    public void PointCreate()
    {
        _spon = true;

        while (_spon == true)
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

                if (areaController._onWall == true)
                {
                    continue;
                }
                else
                {
                    Destroy(MapManager._areas[_randomNumX, _randomNumZ]);
                    MapManager._areas[_randomNumX, _randomNumZ] = Instantiate(_stair, new Vector3(_randomNumX, 0, _randomNumZ), Quaternion.identity);
                    //�����ɊK�i�̏�����������
                    var _instantObj = Instantiate(_door, new Vector3(_randomNumX, 0.51f, _randomNumZ), Quaternion.identity);
                    _instantObj.name = "door";
                    _spon = false;
                }
            }
        }
    }

    public void OpenDoor()
    {
        var doorImage = GameObject.Find("door");
        Destroy(doorImage);
        areaController = MapManager._areas[_randomNumX, _randomNumZ].GetComponent<AreaController>();
        areaController._stair = true;
        Debug.Log(areaController._stair);
    }
}
