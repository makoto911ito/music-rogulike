using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    [SerializeField] Slider _slider = null; 

    public void ChangeSliderValue(int maxHp, float currentHp)
    {
        _slider.maxValue = maxHp;
        _slider.value = currentHp;
    }
}
