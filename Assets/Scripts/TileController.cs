using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileController : MonoBehaviour
{
    public GameObject TileCore;
    private BoundsInt etenduTiles;
    public Tilemap tilemap;
    public Grid grid;

    // Start is called before the first frame update
    void Start()
    {


        BoundsInt bounds = tilemap.cellBounds;

        foreach(var position in bounds.allPositionsWithin)
        {
            Vector3 worldPos = tilemap.GetCellCenterWorld(position);
           
            Debug.Log(worldPos);
        
            var newPrefab = Instantiate(TileCore, worldPos, Quaternion.identity);
            newPrefab.name = ("TileCore"+worldPos);
        }
        
                



                //tilemap.CompressBounds();

                //foreach (var position in tilemap.cellBounds.allPositionsWithin) {
                // Instantiate(TileCore, grid.CellToWorld(position), Quaternion.identity);
                // }

    }
        

    
}
