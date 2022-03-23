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

    public float GetTilepermeability(Vector3Int gridPosition)
    {
        

        //Vector3Int gridPosition = map.WorldToCell(worldPosition);// Conversion world position to grid position
        TileBase tile = map.GetTile(gridPosition);
        

        if (tile == null)
        {
            return 0;
        }
        else
        {
            float permeability = dataFromTiles[tile].permeability; //extrait le nom de la tile
            return permeability;

        }


    }



    public Dictionary<string, TileBase> GetAdjacentTiles(Vector3Int gridPosition) //
    {
        Dictionary<string, TileBase> adjacentTiles = new Dictionary<string, TileBase>
        {
            { "NW", map.GetTile(gridPosition + new Vector3Int(0, -1, 0)) },
            { "SE", map.GetTile(gridPosition + new Vector3Int(0, 1, 0)) },
            { "NE", map.GetTile(gridPosition + new Vector3Int(1, 0, 0)) },
            { "SW", map.GetTile(gridPosition + new Vector3Int(-1, 0, 0)) }
        };

       
        return adjacentTiles;
    }


    public Dictionary<string, float> GetAdjacentTilespermeability(Dictionary<string, TileBase> Tilesvoisines)
    {
        Dictionary<string, float> AdjTilespermeability = new Dictionary<string, float>
        {
            { "NW", 0 },
            { "SE", 0 },
            { "NE", 0 },
            { "SW", 0 }
        };

        string[] key = new string[] { "NW", "SE", "NE", "SW" };
        int limite = 1;
        while (limite <= 4)
        {
            
            if (Tilesvoisines[key[limite]] is null)
             {
                
             }
             else 
             {
              AdjTilespermeability[key[limite]] = dataFromTiles[Tilesvoisines[key[limite]]].permeability;
                
            }
            limite++;
        }

        
        
        return AdjTilespermeability;
    } //Bug out of bounds array
   


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

    public float GetTilereproductionFactor(Vector3Int gridPosition)
    {
        TileBase tile = map.GetTile(gridPosition);

        if (tile == null)
        {
            return 0;

        }
        else
        {
            float reproductionFactor = dataFromTiles[tile].reproductionFactor; //extrait le nom de la tile
            return reproductionFactor;

        }
    }
}
