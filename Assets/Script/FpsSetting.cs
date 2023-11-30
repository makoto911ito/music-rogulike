using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsSetting : MonoBehaviour
{
    public void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
    }

}
