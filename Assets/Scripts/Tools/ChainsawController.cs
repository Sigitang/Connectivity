using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class ChainsawController : MonoBehaviour
{

    private MapManager mapManager;
    private string tileName;
    public Tilemap map;
    public TileBase forest;
    public TileBase house;
    public TileBase empty;
    public TileBase Zh1;
    
    


    void Start()
    { 
        mapManager = FindObjectOfType<MapManager>();
        this.enabled = false;
    }

    

    public void Update()
    {
        

            if (Input.GetMouseButtonDown(0))
            {
               Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
               Vector3Int gridPosition = map.WorldToCell(worldPosition);
               tileName = mapManager.GetTileName(worldPosition);
            
            

            
                if(tileName == "forest") 
                {

                 TileBase newTile = empty;
                 map.SetTile(gridPosition, null);
                 map.SetTile(gridPosition, newTile);
                 GameEvents.current.chainsawUsed.Invoke();
                 
               


               
              




            }

        }

    }

    

}
