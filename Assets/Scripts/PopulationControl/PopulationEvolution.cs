using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PopulationEvolution : TimeDependent
{

    public float nIndiv = 0; // Nombre d'individus
    public float tauxReproduction = 2; // a noter ailleurs ? Taux de reproduction
    public float capaciteMax = 100; //Capacite max
    private float immigration = 0;
    private float emmigration = 0;
    public Object prefabIndiv;
    private float deltaPrefabIndiv = 0;
    public Tilemap map;







    // Start is called before the first frame update
    protected override void Start()
    {
        map = FindObjectOfType<Tilemap>();
        
        base.Start(); //Call le start de la classe parente
        
    }

    public void Awake()
    {
        prefabIndiv = Resources.Load("Prefabs/SonneurBigIndiv"); //va chercher le prefab sonneur
        
    }



    
    public override void OnTick(int deltaDiscreteTime) //On Tick déclenché pour tous object de classe "time dependent" par le GameManager
    {



        int limite = 1;
        //N+1=N*e(r(1-N/K))+i+e
        while(limite < deltaDiscreteTime)
        {

            //emmigration
            if (nIndiv >= (capaciteMax - (0.1*capaciteMax))) //si N s'approche de K alors
            {
                emmigration = nIndiv*0.1f; //emmigration devient 0.1*N
                
            }

            //immigration




            nIndiv = (nIndiv * Mathf.Exp(tauxReproduction*(1 - nIndiv / capaciteMax))) + immigration - emmigration; //run 1xformule evolution pop pour chaque deltaTime passé
            limite++;
        }




        //Spawn de sprites Sonneur

        Vector3 corePosition = map.WorldToCell(this.transform.position);
        var gameObjects = GameObject.FindGameObjectsWithTag("IndivSprite"); //cherche les objets avec le tag IndivSprite et les supprime A CHANGER CAR SUPPRIME SUR TOUS LES TILE CORES

        deltaPrefabIndiv = gameObjects.Length;

        foreach(GameObject obj in gameObjects)
        {
            Vector3 spriteTransform = map.WorldToCell(obj.GetComponent<RectTransform>().position);
            
            
            

            if (spriteTransform == corePosition )
            {
                Debug.Log("suppr");
                Object.Destroy(obj);

            }
            
            

        }

        //Random pos
        limite = 1;
        while(limite < nIndiv/10)
        {
            Vector3 randomPos = new Vector3(Random.Range(-0.1f,0.1f), Random.Range(-0.1f, 0.1f),0); // spawn autour du core sur la tile
            
            Instantiate(prefabIndiv, transform.position+randomPos, Quaternion.identity); 
            limite++;
        }

        }


    

    
    
}

