using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    private MapManager mapManager;



    private void Awake()
    {
        mapManager = FindObjectOfType<MapManager>();

    }

    private void Update()
    {
        float waterDepth = mapManager.GetTilewaterDepth(transform.position);
        print(waterDepth);

    }
}
