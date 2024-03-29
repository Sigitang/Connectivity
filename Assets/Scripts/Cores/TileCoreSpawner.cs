using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileCoreSpawner : MonoBehaviour
{
    public GameObject TileCore;
    public Tilemap tilemap;
    public Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parent = GameObject.Find("Cores");

        BoundsInt bounds = tilemap.cellBounds;

        foreach(var position in bounds.allPositionsWithin)
        {
            Vector3 worldPos = tilemap.GetCellCenterWorld(position);
           
            
        
            var newPrefab = Instantiate(TileCore, worldPos, Quaternion.identity);
            newPrefab.name = ("TileCore"+tilemap.WorldToCell(worldPos));
            newPrefab.AddComponent(typeof(TileCoreControl));
            newPrefab.AddComponent(typeof(PopulationEvolution));
            newPrefab.transform.parent = parent.transform;
        }
        
                

        

                //tilemap.CompressBounds();

                //foreach (var position in tilemap.cellBounds.allPositionsWithin) {
                // Instantiate(TileCore, grid.CellToWorld(position), Quaternion.identity);
                // }

    }
        

    
}
