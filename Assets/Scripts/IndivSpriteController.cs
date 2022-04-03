using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IndivSpriteController : MonoBehaviour
{

    public bool moovingSE = true;
    public bool moovingSW = true;
    public bool moovingNW = true;
    public bool moovingNE = true;
    public Tilemap map;
    private Vector3 start;

    private void Start()
    {
        map = FindObjectOfType<Tilemap>();
        start = this.transform.position;
    }

    void FixedUpdate()
    {
        
            this.transform.position = Vector3.MoveTowards(start, start + new Vector3(0,5000,0), 20f*Time.deltaTime);
            
        

       
    }
}
