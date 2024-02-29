using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Slider _slider = null;

    Text _pPowerText;

    GameObject _playerImage;

    /// <summary>HPを表示するためのスライダー</summary>
    public Slider _heartSlider;

    public void InitView(GameObject _heart, Text powertext,GameObject player)
    {
        _pPowerText = powertext;
        _playerImage = player;
        var slider = _heart.transform.GetChild(0);
        _heartSlider = slider.GetComponent<Slider>();
    }

    public void ChangePowerView(float power)
    {
        _pPowerText.text = "攻撃力 " + power.ToString();
    }

    public void ChangeSliderValue(int maxHp,int currentHp)
    {
        if(_heartSlider.value > currentHp)
        {
            StartCoroutine(Image());
        }
        //Debug.Log(currentHp + "受け取った現在のHP");
        _heartSlider.maxValue = maxHp;
        _heartSlider.value = currentHp;
    }

    IEnumerator Image()
    {
        _playerImage.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        _playerImage.SetActive(true);
    }
}
