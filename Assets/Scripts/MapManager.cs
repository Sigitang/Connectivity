using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    [SerializeField]
    private Tilemap map;

    [SerializeField]
    private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    


    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {

            foreach (var tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);

            }





        }

       
    }

   

    public float GetTilewaterDepth(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);

        TileBase tile = map.GetTile(gridPosition);
        

            float waterDepth = dataFromTiles[tile].waterDepth;
            return waterDepth;

    }

    public string GetTileName(Vector2 worldPosition)

    {
        
        Vector3Int gridPosition = map.WorldToCell(worldPosition); // Conversion world position to grid position
        TileBase tile = map.GetTile(gridPosition);
        if(tile == null) 
        {
            return "Null";
        }
        else
        {
            string tileName = tile.name.ToString(); //extrait le nom de la tile
            return tileName;

        }
    }
}
