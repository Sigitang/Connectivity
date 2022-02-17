using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]

public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    // Liste des variables définies par type de tile
    public float waterDepth, vegetationDensity; 
    public bool  waterPresence, pollutionPresence;


}
