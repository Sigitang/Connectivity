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
    public float emmigration = 0;
    public Object prefabIndiv;
    public Tilemap map;
    private GameObject[] localSeeds;
    private GameObject[] cores;
    public List<GameObject> coresVoisins = new List<GameObject>();








    // Start is called before the first frame update
    protected override void Start()
    {
        
        
        base.Start(); //Call le start de la classe parente

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


        cores = GameObject.FindGameObjectsWithTag("Tilecore"); //Cherche tous les cores
        
        
        foreach (GameObject obj in cores) // Importe les cores des grilles environnantes // coresVoisins --> [NW,SE,NE,SW]          // A TRANSFORMER EN METHODE
        {

            if (map.WorldToCell(obj.transform.position) == map.WorldToCell(this.transform.position) + new Vector3Int(0, 1, 0)) //NW
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

            if (map.WorldToCell(obj.transform.position) == map.WorldToCell(this.transform.position) + new Vector3Int(-1, 0, 0)) //SW
            {
                
                coresVoisins.Add(obj);
            }

            
            
        }




    }

    public void Awake()
    {
        prefabIndiv = Resources.Load("Prefabs/SonneurBigIndiv"); //va chercher le prefab sonneur
        

        


    }



    
    public override void OnTick(int deltaDiscreteTime) //On Tick d�clench� pour tous object de classe "time dependent" par le GameManager
    {
        

        


        int limite = 1;
        //N+1=N*e(r(1-N/K))+i+e
        while(limite < deltaDiscreteTime)
        {

            //emmigration
            if (nIndiv >= (capaciteMax - (0.1*capaciteMax))) //si N s'approche de K alors
            {
                emmigration = nIndiv*0.1f; //emmigration devient 0.1*N                              // A DIVISER PAR NOMBRE DE VOISINS FAVORABLES
                
            }

            //immigration
            immigration = coresVoisins[1].GetComponent<PopulationEvolution>().emmigration + coresVoisins[2].GetComponent<PopulationEvolution>().emmigration
             + coresVoisins[3].GetComponent<PopulationEvolution>().emmigration + coresVoisins[4].GetComponent<PopulationEvolution>().emmigration;







            nIndiv = (nIndiv * Mathf.Exp(tauxReproduction*(1 - nIndiv / capaciteMax))) + immigration - emmigration; //run 1xformule evolution pop pour chaque deltaTime pass�
            limite++;
        }




        //Spawn de sprites Sonneur

        Vector3 corePosition = map.WorldToCell(this.transform.position);

        var gameObjects = GameObject.FindGameObjectsWithTag("IndivSprite"); //cherche les objets avec le tag IndivSprite et les supprime 

        

        foreach(GameObject obj in gameObjects)
        {
            Vector3 spriteTransform = map.WorldToCell(obj.GetComponent<RectTransform>().position);
            
            
            

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
            
            Instantiate(prefabIndiv, transform.position+randomPos, Quaternion.identity); 
            limite++;
        }

        }


    

    
    
}

