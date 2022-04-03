using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


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
    private MapManager mapManager;


    private Dictionary<string, TileBase> adjacentTiles;
    private Dictionary<string, float> adjacentTilespermeability;
    public Dictionary<string, float> emmigrationEffective;
    public float nindivonKmax;




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
            if (map.WorldToCell(obj.transform.position) == map.WorldToCell(this.transform.position))
            {
                nIndiv = obj.GetComponent<PopulationSpawnSeed>().nIndivStart;

            }

        }

        emmigrationEffective = new Dictionary<string, float> { { "NW", 0 }, { "SE", 0 }, { "NE", 0 }, { "SW", 0 } };


    }

    public void Awake()
    {
        prefabIndiv = Resources.Load("Prefabs/SonneurSmol"); //va chercher le prefab sonneur

    }


    private float GetimmigrationEffective(Dictionary<string, GameObject> voisins) //va chercher la variable emmigrationEffective des tuiles voisines 
    {
        float immigrationEffective = 0;

        if (voisins["NW"] != null)
        {
            immigrationEffective += voisins["NW"].GetComponent<PopulationEvolution>().emmigrationEffective["SE"];
        }


        if (voisins["SE"] != null)
        {
            immigrationEffective += voisins["SE"].GetComponent<PopulationEvolution>().emmigrationEffective["NW"];
        }


        if (voisins["SW"] != null)
        {
            immigrationEffective += voisins["SW"].GetComponent<PopulationEvolution>().emmigrationEffective["NE"];
        }

        if (voisins["NE"] != null)
        {
            immigrationEffective += voisins["NE"].GetComponent<PopulationEvolution>().emmigrationEffective["SW"];
        }

        return immigrationEffective;
    }

    private Dictionary<string, float> GetemmigrationEffective(float emmigration, Dictionary<string, TileBase> adjacentTiles, Dictionary<string, bool> kmaxCheck)
    {

        adjacentTilespermeability = mapManager.GetAdjacentTilespermeability(adjacentTiles); //Get the permeability from the adjacent tiles
                                                                                            //var orientations = new string[]{"NW","NE","SE","SW"}; //liste des keys

        Dictionary<string, float> priorities = new Dictionary<string, float>();
        Dictionary<string, float> emmigrationEffective = new Dictionary<string, float> { { "NW", 0 }, { "SE", 0 }, { "NE", 0 }, { "SW", 0 } };

        //1) check Kmax
        foreach (string keys in adjacentTiles.Keys)
        {
            if (kmaxCheck[keys] == false)
            {
                adjacentTilespermeability[keys] = 0;
            }

        }

         var boucles = 1;

        while (emmigration >= 1 && boucles <= 4)//stop quand le pool d'emmigration est vide
        {
            
            
            
            
            //2) classer par priorite


            foreach (string keys in adjacentTilespermeability.Keys)
            {
                
                if (adjacentTilespermeability[keys] == Mathf.Max(adjacentTilespermeability.Values.ToArray())) //check si adjacentTilespermeability[keys] est le max
                {

                priorities.Add(keys, adjacentTilespermeability[keys]);

                }

            }

            foreach (string keys in priorities.Keys) //calculer eF pour chaque tuile prioritaire divise par le nombre de tuiles prioritaires
            {
                
                if(kmaxCheck[keys]==true)
                { 
                    emmigrationEffective[keys] = (priorities[keys] * emmigration) / priorities.Count; //calcule eF
                    emmigration -= emmigrationEffective[keys]; //Soustrait eF au pool
                }
                else { }


            }

            priorities.Remove("NW"); priorities.Remove("NE"); priorities.Remove("SE"); priorities.Remove("SW");
            boucles++;
        }





        return emmigrationEffective;





    }

    private Dictionary<string, bool> CheckKmaxinAdjacentCores(Dictionary<string, GameObject> voisins) //Check si les tuiles adjacentes ont un nIndiv/Kmax >= 0.8 (evite les retours)
    {
        Dictionary<string, bool> kMaxCheck = new Dictionary<string, bool>();


        foreach (string keys in voisins.Keys)
        {

            if (voisins[keys] != null)
            {
                if (voisins[keys].GetComponent<PopulationEvolution>().nindivonKmax >= 0.8)
                {
                    kMaxCheck.Add(keys, false);
                }
                else
                {
                    kMaxCheck.Add(keys, true);
                }
            }
            else
            {
                kMaxCheck.Add(keys, false);
            }

        }


        return kMaxCheck;
    }


    public override void OnTick(int deltaDiscreteTime) //On Tick déclenché pour tous object de classe "time dependent" par le GameManager // Faire un latetick pour laisser le temps ?

    {



        emmigrationEffective = new Dictionary<string, float> { { "NW", 0 }, { "SE", 0 }, { "NE", 0 }, { "SW", 0 } };


        int limite = 0;
        //N+1=N*e(r(1-N/K))+i+e
        while (limite < deltaDiscreteTime) //1 Tick / discreteTime
        {
            adjacentTiles = mapManager.GetAdjacentTiles(map.WorldToCell(this.transform.position));
            Dictionary<string, GameObject> coresVoisins = mapManager.GetadjacentCores(map.WorldToCell(this.transform.position));

            //Kmax
            capaciteMax = mapManager.GetTileKmax(map.WorldToCell(this.transform.position));

            //emmigration
            if (nIndiv > 0.95 * capaciteMax)//si N s'approche de K alors:
            {
                emmigration = Random.Range(0.1f, 0.2f) * nIndiv;
            }
            else
            {
                emmigration = 0;
            }

            Dictionary<string, bool> kmaxCheck = CheckKmaxinAdjacentCores(coresVoisins);
            //print("NW" + kmaxCheck["NW"]); print("SE" + kmaxCheck["SE"]); print("SW"+kmaxCheck["SW"]); print("NE"+kmaxCheck["NE"]);

            emmigrationEffective = GetemmigrationEffective(emmigration, adjacentTiles, kmaxCheck);

            //immigration    
            immigration = GetimmigrationEffective(coresVoisins);

            var emmigrationEffectiveTotal = emmigrationEffective["NW"] + emmigrationEffective["SE"] + emmigrationEffective["NE"] + emmigrationEffective["SW"];



            //Reproduction
            tauxReproduction = mapManager.GetTilereproductionFactor(map.WorldToCell(this.transform.position));

            //Calcul N+1
            nIndiv = (nIndiv * Mathf.Exp(tauxReproduction * (1 - nIndiv / capaciteMax))) + immigration - emmigrationEffectiveTotal;//run 1xformule evolution pop pour chaque deltaTime passe
            nindivonKmax = nIndiv / capaciteMax;

            limite++;



        }



        //------------------------Detection des sprites sur la tuile---------------------------------

        Vector3 corePosition = map.WorldToCell(this.transform.position);

        var gameObjectsIndivSprite = GameObject.FindGameObjectsWithTag("IndivSprite"); //cherche les objets avec le tag IndivSprite
        var indivSpriteNumber = new List<Object>();


        foreach (GameObject obj in gameObjectsIndivSprite)
        {
            Vector3 spriteTransform = map.WorldToCell(obj.GetComponent<Transform>().position);

            if (spriteTransform == corePosition) //stock les sprites sur la tuile       
            {
                indivSpriteNumber.Add(obj);
            }
        }



        //-------------------------Sonneur deplacements ------------------------------------



        var limiteMovement = 0;

        foreach (GameObject obj in indivSpriteNumber)
        {
            if (limiteMovement <= emmigrationEffective["NW"])
            {
                obj.GetComponent<IndivSpriteController>().moovingNW = true;
                limiteMovement++;
            }

        }




        //--------------------------Spawn de sprites Sonneur ----------------------------------------

        var limiteDelete = indivSpriteNumber.Count - nIndiv;
        foreach (GameObject obj in indivSpriteNumber)
        {
            if (nIndiv < limiteDelete)
            {
                Destroy(obj);
                limiteDelete--;
            }
        }

        var limiteSpawn = indivSpriteNumber.Count;
        while (limiteSpawn < nIndiv && limiteSpawn < 100)
        {

            Vector3 randomPos = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0); // spawn autour du core sur la tile
            Instantiate(prefabIndiv, transform.position + randomPos, Quaternion.identity, GameObject.Find("Units").transform);


            limiteSpawn++;
        }



    }



}

