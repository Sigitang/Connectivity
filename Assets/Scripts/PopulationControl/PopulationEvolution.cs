using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationEvolution : TimeDependent
{

    public double Nindiv = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        Nindiv = 10;
        base.Start(); //Call le start de la class parente


    }

     public override void OnTick()
    {

        //dN/dT(costTime) = aN(1-N/K)

        Nindiv = (3 * Nindiv) * (1 - Nindiv / 100);

    }

    // Update is called once per frame
    
}

