using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBossMap : MonoBehaviour
{
    [SerializeField] public GameObject _mapController;

    MapManager _mapManager;

    private void Start()
    {
        _mapManager = GetComponent<MapManager>();
    }

    private void Update()
    {
        if(Input.GetButtonDown("Horizontal"))
        {
            _mapManager.BossMapCreate(_mapController);
        }
    }
}
