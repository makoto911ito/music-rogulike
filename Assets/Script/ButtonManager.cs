using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    //ゲームシーンへの移動
    public void GoGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    //タイトルシーンへの移動
    public void GoTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void RuleScene()
    {
        SceneManager.LoadScene("RuleScene");
    }
}
