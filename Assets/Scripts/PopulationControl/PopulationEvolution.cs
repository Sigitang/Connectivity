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
    public float emmigration = 0f;
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


    private float GetimmigrationEffective(Dictionary<string, GameObject> voisins) //va chercher la variable emmigration des tuiles voisines
    {
        float immigrationEffective = 0;

        if (voisins["NW"] == null)
        { }
        else { immigrationEffective += voisins["NW"].GetComponent<PopulationEvolution>().emmigrationEffective["SE"]; }


        if (voisins["SE"] == null)
        { }
        else { immigrationEffective += voisins["SE"].GetComponent<PopulationEvolution>().emmigrationEffective["NW"]; }

        if (voisins["SW"] == null)
        { }
        else { immigrationEffective += voisins["SW"].GetComponent<PopulationEvolution>().emmigrationEffective["NE"]; }

        if (voisins["NE"] == null)
        { }
        else { immigrationEffective += voisins["NE"].GetComponent<PopulationEvolution>().emmigrationEffective["SW"]; }

        return immigrationEffective;
    } //Bug de reference

    private Dictionary<string, float> GetemmigrationEffective(float emmigration, Dictionary<string, TileBase> adjacentTiles)
    {
        adjacentTilespermeability = mapManager.GetAdjacentTilespermeability(adjacentTiles);

        Dictionary<string, float> emmigrationEffective = new Dictionary<string, float>
        {
            {"NW", 0 }, {"SE", 0 }, {"NE", 0 }, {"SW", 0 }

        };


        if ((adjacentTilespermeability["NW"] + adjacentTilespermeability["SE"] + adjacentTilespermeability["NE"] + adjacentTilespermeability["SW"]) > 1) //Si emmigration totale > emmigrationtheorique
        {
            //Chaque permeabilite/tuile check s'il passe en priorite pour l'emmigration --> "pool d emmigration"

            float eMax = emmigration;
            int i = 0;
            string[] key = new string[] { "NW", "SE", "NE", "SW" };
            while (eMax >= 1 && i < 4)
            {

                if (adjacentTilespermeability[key[i]] == Mathf.Max(adjacentTilespermeability["NW"], adjacentTilespermeability["SE"], adjacentTilespermeability["NE"], adjacentTilespermeability["SW"]))
                {
                    emmigrationEffective[key[i]] = adjacentTilespermeability[key[i]] * emmigration;

                    eMax -= emmigrationEffective[key[i]];
                    var max = from x in adjacentTilespermeability where x.Value == adjacentTilespermeability[key[i]] select x.Key; //Linq 
                    adjacentTilespermeability[max.ToString()] = 0;

                    i = 0;

                }
                else
                {

                    i++;

                }



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
            if (nIndiv > capaciteMax) //si N s'approche de K alors
            {
                emmigration = 0.1f * nIndiv;   //emmigrationtheorique = Nx[(N-K)/K] //A DIVISER PAR NOMBRE DE VOISINS FAVORABLES  
            }


            emmigrationEffective = GetemmigrationEffective(emmigration, adjacentTiles);


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

