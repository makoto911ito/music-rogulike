using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AreaObj
{
    walk = 0,
    wall = 1,
}

public class AreaTable : MonoBehaviour
{
    [SerializeField] AreaObj _areaObj;

    public AreaObj AreaObj
    {
        get => _areaObj;
        set
        {
            _areaObj = value;
        }
    }
}
