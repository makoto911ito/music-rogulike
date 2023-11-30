using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private GameObject player;   //�v���C���[���i�[�p
    private Vector3 offset;      //���΋����擾�p

    bool _frag = false;

    public void Init()
    {
        //unitychan�̏����擾
        player = GameObject.Find("Player");

        // MainCamera(�������g)��player�Ƃ̑��΋��������߂�
        offset = transform.position - player.transform.position;

        _frag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_frag != false)
        {
            //�V�����g�����X�t�H�[���̒l��������
            transform.position = player.transform.position + offset;
        }
    }
}
