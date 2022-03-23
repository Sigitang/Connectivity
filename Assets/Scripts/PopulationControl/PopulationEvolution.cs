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
    private List<GameObject> coresVoisins = new List<GameObject>();
    private MapManager mapManager;

    
    private Dictionary<string, TileBase> adjacentTiles;
    private Dictionary<string, float> adjacentTilespermeability;










    // Start is called before the first frame update
    protected override void Start()
    {
        


        base.Start(); //Call le start de la classe parente

        //ChercheMapmanager
        mapManager = FindObjectOfType<MapManager>();

        //Si objet tag "seed" present dans la cell alors prend la variable nindivseed de seed et le prend en Nindiv
        localSeeds = GameObject.FindGameObjectsWithTag("seed");
        //Cherche la map
        map = FindObjectOfType<Tilemap>();


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


    private float GetImmigration(List<GameObject> voisins) //va chercher la variable emmigration des tuiles voisines
    {
        float immi = 0;
        foreach(GameObject obj in voisins)
        {

            immi += obj.GetComponent<PopulationEvolution>().emmigration;
            
        }
       
        return immi;   
    }


    private Dictionary<string, float> GetemmigrationEffective(float emmigration, Dictionary<string, TileBase> adjacentTiles)
    {
        adjacentTilespermeability = mapManager.GetAdjacentTilespermeability(adjacentTiles);

        Dictionary<string, float> emmigrationEffective = new Dictionary<string, float>();



        if ((adjacentTilespermeability["NW"] + adjacentTilespermeability["SE"] + adjacentTilespermeability["NE"] + adjacentTilespermeability["SW"]) > 1) //Si emmigration totale > emmigrationtheorique
        {
            //Chaque permeabilite/tuile check s'il passe en priorite pour l'emmigration --> "pool d emmigration"

            float eMax = emmigration;
            int i = 1;
            string[] key = new string[] { "NW", "SE", "NE", "SW" };
            while (eMax >= 1 && i < 5)
            {

                if (adjacentTilespermeability[key[i]] == Mathf.Max(adjacentTilespermeability["NW"], adjacentTilespermeability["SE"], adjacentTilespermeability["NE"], adjacentTilespermeability["SW"]))
                {
                    emmigrationEffective.Add(key[i], adjacentTilespermeability[key[i]] * emmigration);

                    eMax -= emmigrationEffective[key[i]];
                    var max = from x in adjacentTilespermeability where x.Value == adjacentTilespermeability[key[i]] select x.Key; //Linq 
                    adjacentTilespermeability[max.ToString()] = 0;

                    i = 1;

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

        cores = GameObject.FindGameObjectsWithTag("Tilecore"); //Cherche tous les cores
        coresVoisins = new List<GameObject>();

        foreach (GameObject obj in cores) // Importe les cores des grilles environnantes // coresVoisins --> [NW,SE,NE,SW]          // A TRANSFORMER EN METHODE
        {

            if (map.WorldToCell(obj.transform.position) == map.WorldToCell(this.transform.position) + new Vector3Int(-1, 0, 0)) //NW
            {

                coresVoisins.Add(obj);
            }
            
            if (map.WorldToCell(obj.transform.position) == map.WorldToCell(this.transform.position) + new Vector3Int(0, -1, 0)) //SE
            {

                coresVoisins.Add(obj);
            }
            
            if (map.WorldToCell(obj.transform.position) == map.WorldToCell(this.transform.position) + new Vector3Int(1, 0, 0)) //NE
            {

                    coresVoisins.Add(obj);
            }
           
            if (map.WorldToCell(obj.transform.position) == map.WorldToCell(this.transform.position) + new Vector3Int(0, 1, 0)) //SW
            {

                    coresVoisins.Add(obj);
            }
            


        }

        

        int limite = 0;
        //N+1=N*e(r(1-N/K))+i+e
        while (limite < deltaDiscreteTime)
        {

            adjacentTiles = mapManager.GetAdjacentTiles(map.WorldToCell(this.transform.position));
            
            //Kmax
            capaciteMax = mapManager.GetTileKmax(map.WorldToCell(this.transform.position));


            //emmigration
            if (nIndiv > capaciteMax) //si N s'approche de K alors
            {
                emmigration = 0.1f * nIndiv;   //emmigrationtheorique = Nx[(N-K)/K] //A DIVISER PAR NOMBRE DE VOISINS FAVORABLES  
            }


            //Dictionary<string,float> emmigrationEffective = GetemmigrationEffective(emmigration, adjacentTiles);

            //print(emmigrationEffective);

            //immigration
            immigration = GetImmigration(coresVoisins);
            

            



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

