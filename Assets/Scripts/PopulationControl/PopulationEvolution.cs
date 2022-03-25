using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;


public class PopulationEvolution : TimeDependent
{
    [SerializeField]
    private float nIndiv = 0; // Nombre d'individus
    public float tauxReproduction = 1f; // a noter ailleurs ? Taux de reproduction
    public float capaciteMax; //Capacite max

    [SerializeField]
    private float immigration;
    public float emmigration = 0;
    public Object prefabIndiv;
    public Tilemap map;
    private GameObject[] localSeeds;
    private GameObject[] cores;
    private MapManager mapManager;

    
    private Dictionary<string, TileBase> adjacentTiles;
    private Dictionary<string, float> adjacentTilespermeability;
    public Dictionary<string, float> emmigrationEffective;
        










    // Start is called before the first frame update
    protected override void Start()
    {
        //Cherche la map
        map = FindObjectOfType<Tilemap>();


        base.Start(); //Call le start de la classe parente

        //ChercheMapmanager
        mapManager = FindObjectOfType<MapManager>();

        //Si objet tag "seed" present dans la cell alors prend la variable nindivseed de seed et le prend en Nindiv
        localSeeds = GameObject.FindGameObjectsWithTag("seed");
        foreach (GameObject obj in localSeeds)
        {
            if(map.WorldToCell(obj.transform.position) == map.WorldToCell(this.transform.position))
            {
                nIndiv = obj.GetComponent<PopulationSpawnSeed>().nIndivStart;
                
            }

        }

       


    }

    public void Awake()
    {
        prefabIndiv = Resources.Load("Prefabs/SonneurSmol"); //va chercher le prefab sonneur

    }


    private float GetimmigrationEffective(Dictionary<string, GameObject> voisins) //va chercher la variable emmigration des tuiles voisines BUGBUGBUG
    {
        
        float immigrationEffective = 0;

       

        if (voisins["NW"] != null)
        {
            immigrationEffective += voisins["NW"].GetComponent<PopulationEvolution>().emmigrationEffective["SE"];
            Debug.Log("NW found");
        }
        


        if (voisins["SE"] != null)
        {
            immigrationEffective += voisins["SE"].GetComponent<PopulationEvolution>().emmigrationEffective["NW"];
            Debug.Log("SE found");
        }
       

        if (voisins["SW"] != null)
        {
            //immigrationEffective += voisins["SW"].GetComponent<PopulationEvolution>().emmigrationEffective["NE"];
            var vNW = voisins["SW"];
            var compvNW = vNW.GetComponent<PopulationEvolution>();
            var eF = compvNW.emmigrationEffective;
            immigrationEffective += eF["NE"];

            Debug.Log("SW found");
        }
        
        if (voisins["NE"] != null)
        {
            immigrationEffective += voisins["NE"].GetComponent<PopulationEvolution>().emmigrationEffective["SW"];
            Debug.Log("NE found");
        }




        return immigrationEffective;
    }  



    private Dictionary<string, float> GetemmigrationEffective(float emmigration, Dictionary<string, TileBase> adjacentTiles)
    {

        adjacentTilespermeability = mapManager.GetAdjacentTilespermeability(adjacentTiles); //Get the permeability from the adjacent tiles
        //var orientations = new string[]{"NW","NE","SE","SW"}; //liste des keys
        
        Dictionary<string, float> priorities = new Dictionary<string, float>();
        Dictionary<string, float> emmigrationEffective = new Dictionary<string, float> { { "NW", 0 }, { "SE", 0 }, { "NE", 0 }, { "SW", 0 } };
       

        while (emmigration >= 1)//stop quand le pool d'emmigraiton est vide
        {
            //1) classer par priorite
            foreach (string keys in adjacentTilespermeability.Keys)
            {
                if(adjacentTilespermeability[keys] == Mathf.Max(adjacentTilespermeability.Values.ToArray())) //check si adjacentTilespermeability[keys] est le max
                {
                    priorities.Add(keys, adjacentTilespermeability[keys]);

                }
                

            }

            foreach(string keys in priorities.Keys) //calculer eF pour chaque tuile prioritaire divise par le nombre de tuiles prioritaires
            {
                emmigrationEffective[keys] = (priorities[keys]*emmigration)/priorities.Count; //calcule eF
                emmigration -= emmigrationEffective[keys]; //Soustrait eF au pool
               
            }



        }

        return emmigrationEffective;


    }








       
 

    public override void OnTick(int deltaDiscreteTime) //On Tick déclenché pour tous object de classe "time dependent" par le GameManager
    {

        
        //importe les cores voisins

        

        int limite = 0;
        //N+1=N*e(r(1-N/K))+i+e
        while (limite < deltaDiscreteTime) //1 Tick / discreteTime
        {
            adjacentTiles = mapManager.GetAdjacentTiles(map.WorldToCell(this.transform.position));
            
            //Kmax
            capaciteMax = mapManager.GetTileKmax(map.WorldToCell(this.transform.position));

            //emmigration
            if (nIndiv > capaciteMax)//si N s'approche de K alors
            {  
                    emmigration = 0.1f * nIndiv; 
            }
            else
            {
                emmigration = 0;
            }

            emmigrationEffective = GetemmigrationEffective(emmigration, adjacentTiles); //renvoi null a t = 0 ?
            Debug.Log(emmigrationEffective["SE"]);
            Debug.Log(emmigrationEffective["NW"]);
            Debug.Log(emmigrationEffective["SW"]);
            Debug.Log(emmigrationEffective["NE"]);

            //immigration    
            Dictionary<string, GameObject> coresVoisins = mapManager.GetadjacentCores(map.WorldToCell(this.transform.position));
            immigration = GetimmigrationEffective(coresVoisins);


            //Reproduction
            //tauxReproduction *= mapManager.GetTilereproductionFactor(map.WorldToCell(this.transform.position));

            //Calcul N+1
            nIndiv = (nIndiv * Mathf.Exp(tauxReproduction * (1 - nIndiv / capaciteMax))) + immigration - emmigration;//run 1xformule evolution pop pour chaque deltaTime passe
            limite++;

        }



                
        //--------------------------Spawn de sprites Sonneur

        Vector3 corePosition = map.WorldToCell(this.transform.position);

        var gameObjects = GameObject.FindGameObjectsWithTag("IndivSprite"); //cherche les objets avec le tag IndivSprite et les supprime 

        

        foreach(GameObject obj in gameObjects)
        {
            Vector3 spriteTransform = map.WorldToCell(obj.GetComponent<Transform>().position);
            
            
            

            if (spriteTransform == corePosition ) //supprime les sprites sur la case du core        //PAS PARFAIT: FAIRE UN SPAWN ET DESPAWN DIFFERENCIE
            {
                Object.Destroy(obj);

            }
            
            

        }

        //Random pos
        limite = 1;                                         
        while(limite < nIndiv/10) //1 sprite pour 10 individus
        {
            Vector3 randomPos = new Vector3(Random.Range(-0.1f,0.1f), Random.Range(-0.1f, 0.1f),0); // spawn autour du core sur la tile
            
            Instantiate(prefabIndiv, transform.position+randomPos, Quaternion.identity, GameObject.Find("Units").transform); 
            limite++;
        }

        }


    

    
    
}

