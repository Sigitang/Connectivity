using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopulationEvolution : TimeDependent
{

    public float nIndiv = 0; // Nombre d'individus
    public float tauxReproduction = 2; // a noter ailleurs ? Taux de reproduction
    public float capaciteMax = 100; //Capacite max
    public float immigration = 0;
    public float emmigration = 0;
    public Object prefabIndiv;






    // Start is called before the first frame update
    protected override void Start()
    {
        nIndiv = 10;
        base.Start(); //Call le start de la class parente

        
    }

    public void Awake()
    {
        prefabIndiv = Resources.Load("Prefabs/SonneurBigIndiv"); //va chercher le prefab sonneur
        Debug.Log(prefabIndiv.name);
    }
    



    public override void OnTick(int deltaDiscreteTime) //public override void OnTick()
    {
        int limite = 1;
        
        //dN/dT(costTime) = aN(1-N/K)
        while(limite < deltaDiscreteTime)
        {

            nIndiv = (nIndiv * Mathf.Exp(tauxReproduction*(1- nIndiv / capaciteMax))) + immigration + emmigration; //run 1xformule evolution pop pour chaque deltaTime passé
            limite++;
        }

        var gameObjects = GameObject.FindGameObjectsWithTag("IndivSprite"); //cherche les objets avec le tag IndivSprite et les supprime A CHANGER CAR SUPPRIME SUR TOUS LES TILE CORES
        foreach(GameObject obj in gameObjects)
        {
           Object.Destroy(obj);

        }

        

        limite = 1;

        while(limite < nIndiv/10)
        {
            Vector3 randomPos = new Vector3(Random.Range(-0.1f,0.1f), Random.Range(-0.1f, 0.1f),0); // spawn autour du core
            
            Instantiate(prefabIndiv, transform.position+randomPos, Quaternion.identity); 
            limite++;
        }



    }

    
    
}

