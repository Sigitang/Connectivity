using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCoreControl : MonoBehaviour
{
    //private MapManager mapManager;
    //private float waterDepth;
    private Tilemap map;
    

    private void Start()
    {
        //mapManager = FindObjectOfType<MapManager>();
        map = FindObjectOfType<Tilemap>();

        Vector3Int gridPosition = map.WorldToCell(transform.position);
        TileBase tile = map.GetTile(gridPosition);

        if (tile == null)
        {
            Object.Destroy(this.gameObject);

        }
        else
        {
           // waterDepth = mapManager.GetTilewaterDepth(transform.position);
            
            
        }
    }

    

}
