using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Slider _slider = null;

    Slider _heartSlider;

    public void SetSlider(GameObject _heart)
    {
        var slider = _heart.transform.GetChild(0);
        _heartSlider = slider.GetComponent<Slider>();
    }


    public void ChangeSliderValue(int maxHp,int currentHp)
    {
        _heartSlider.maxValue = maxHp;
        _heartSlider.value = currentHp;
    }
}
