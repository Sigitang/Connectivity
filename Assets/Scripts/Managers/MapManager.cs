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

 
    public Dictionary<string, TileBase> GetAdjacentTiles(Vector3Int gridPosition) //
    {
        Dictionary<string, TileBase> adjacentTiles = new Dictionary<string, TileBase>
        {
            { "NW", map.GetTile(gridPosition + new Vector3Int(0, 1, 0)) },
            { "SE", map.GetTile(gridPosition + new Vector3Int(0, -1, 0)) },
            { "NE", map.GetTile(gridPosition + new Vector3Int(1, 0, 0)) },
            { "SW", map.GetTile(gridPosition + new Vector3Int(-1, 0, 0)) }
        };

       
        return adjacentTiles;
    }

    public Dictionary<string, GameObject> GetadjacentCores(Vector3Int gridposition)
    {
        List<GameObject> allcores = new List<GameObject>(GameObject.FindGameObjectsWithTag("Tilecore")); //get all tilecores

        Dictionary<string, GameObject> adjacentCores = new Dictionary<string, GameObject>
        {
            { "NW", null}, { "SE", null}, { "NE", null}, { "SW", null}
        };

        foreach (GameObject obj in allcores) // Importe les cores des grilles environnantes // adjacentCores --> [NW,SE,NE,SW]       
        {
            List<bool> trackList = new List<bool>
            {
                false,
                false,
                false,
                false
            };


            if (trackList[0]==false)
            {
                if (map.WorldToCell(obj.transform.position) == gridposition + new Vector3Int(0, 1, 0)) //NW
                {
                    adjacentCores["NW"] = obj;
                    trackList[0] = true;
                }
            
            }

           if (trackList[1] == false)
           {
                if (map.WorldToCell(obj.transform.position) == gridposition + new Vector3Int(0, -1, 0)) //SE
                {
                    adjacentCores["SE"] = obj;
                    trackList[1] = true;
                }
               

            }


            if (trackList[2] == false)
            {

                if (map.WorldToCell(obj.transform.position) == gridposition + new Vector3Int(1, 0, 0)) //NE
                {
                    adjacentCores["NE"] = obj;
                    trackList[2] = true;
                }
              
            }

            if (trackList[3] == false)
            {
                if (map.WorldToCell(obj.transform.position) == gridposition + new Vector3Int(-1, 0, 0)) //SW
                {
                    adjacentCores["SW"] = obj;
                    trackList[3] = true;
                }
                

            }

          

        }
        return adjacentCores;
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
        foreach(string direction in key)
        {
            
            if (Tilesvoisines[direction] is null)
             {
                AdjTilespermeability[direction] = 0;
             }
             else 
             {
              AdjTilespermeability[direction] = dataFromTiles[Tilesvoisines[direction]].permeability;
                
            }
          
        }
        
        return AdjTilespermeability;
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
