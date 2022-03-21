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


    public string GetTileName(Vector3Int gridPosition)

    {
        
        //Vector3Int gridPosition = map.WorldToCell(worldPosition); // Conversion world position to grid position
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

    public float GetTileImmigration(Vector3Int gridPosition)
    {
        

        //Vector3Int gridPosition = map.WorldToCell(worldPosition);// Conversion world position to grid position
        TileBase tile = map.GetTile(gridPosition);
        

        if (tile == null)
        {
            return 0;
        }
        else
        {
            float immigrationFactor = dataFromTiles[tile].immigrationFactor; //extrait le nom de la tile
            return immigrationFactor;

        }


    }

    public float GetTileKmax(Vector3Int gridPosition)
    {
        //Vector3Int gridPosition = map.WorldToCell(worldPosition);// Conversion world position to grid position
        TileBase tile = map.GetTile(gridPosition);

        if (tile == null)
        {
            return 0;
          
        }
        else
        {
            float Kmax = dataFromTiles[tile].Kmax; //extrait le nom de la tile
            return Kmax;

        }
    }
}
