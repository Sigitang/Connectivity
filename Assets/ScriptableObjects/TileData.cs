using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]

public class TileData : ScriptableObject //Scriptable object stocke donnée de tuile
{
    

    public TileBase[] tiles; //assigne le nom tile a toutes les tiles

    public float waterDepth, vegetationDensity;
    public bool waterPresence, pollutionPresence;

}
