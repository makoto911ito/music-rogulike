using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private GameObject player;   //プレイヤー情報格納用
    private Vector3 offset;      //相対距離取得用

    bool _frag = false;

    public void Init()
    {
        //unitychanの情報を取得
        player = GameObject.Find("Player");

        // MainCamera(自分自身)とplayerとの相対距離を求める
        offset = transform.position - player.transform.position;

        _frag = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_frag != false)
        {
            //新しいトランスフォームの値を代入する
            transform.position = player.transform.position + offset;
        }
    }
}
