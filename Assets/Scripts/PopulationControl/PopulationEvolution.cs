using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopulationEvolution : TimeDependent
{

    public float nIndiv = 0; // Nombre d'individus
    public float tauxReproduction = 2; // a noter ailleurs ? Taux de reproduction
    public float capaciteMax = 50; //Capacite max
    public float immigration = 0;
    public float emmigration = 0;
    public GameObject SonneurBig;
  


    // Start is called before the first frame update
    protected override void Start()
    {
        nIndiv = 10;
        base.Start(); //Call le start de la class parente
        

    }

     public override void OnTick(int deltaDiscreteTime) //public override void OnTick()
    {
        int limite = 1;
        Debug.Log("ontick");
        //dN/dT(costTime) = aN(1-N/K)
        while(limite < deltaDiscreteTime)
        {

            nIndiv = (nIndiv * Mathf.Exp(tauxReproduction*(1- nIndiv / capaciteMax))) + immigration + emmigration;
            
            print("N+1");

            limite++;
        }

        var gameObjects = GameObject.FindGameObjectsWithTag("IndivSprite"); //cherche les objets avec le tag IndivSprite et les supprime
        foreach(GameObject obj in gameObjects)
        {
           Object.Destroy(obj);

        }

        

        limite = 1;

        while(limite < nIndiv)
        {
            
            var newPrefab = Instantiate(SonneurBig, transform.position+posModif, Quaternion.identity); // Spawn au même endroit...
            limite++;
        }



    }

    
    
}

