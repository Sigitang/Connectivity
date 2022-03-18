using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class GameManager : MonoBehaviour
{

    public int discreteTime;
    public int deltaDiscreteTime; //cout en temps a recolter
    public int limitDiscreteTime;
    readonly List<TimeDependent> timeDependentList;
    readonly Dictionary<GameObject, int> toolDeltaTimes = new Dictionary<GameObject, int>();
    public GameObject chainsaw;
    public GameObject excavation;



    GameManager() //Constructeur de la classe = premier truc appele lors de la creation de l'objet 1ere priorite
    {
        timeDependentList = new List<TimeDependent>();

    }

    void Start()
    {

        GameEvents.current.chainsawUsed.AddListener(delegate { ChangeDeltaDiscreteTime(chainsaw); }); //inscrit Changedeltadiscretetime a l'event d'utilisation de la chainsaw
        GameEvents.current.chainsawUsed.AddListener(LaunchOnTick); //Déclenche le tick quand la chainsaw est utilisée

        GameEvents.current.excavationUsed.AddListener(delegate { ChangeDeltaDiscreteTime(excavation); }); //inscrit Changedeltadiscretetime a l'event d'utilisation de la chainsaw
        GameEvents.current.excavationUsed.AddListener(LaunchOnTick); //Déclenche le tick quand la chainsaw est utilisée

        discreteTime = 0; //time = 0 au debut du jeu
        limitDiscreteTime = 250; //changer en fonction du level

        //Chercher les tools
        
        toolDeltaTimes.Add(chainsaw, 2); //Ajouter tools au fur et a mesure avec le coût en temps en value
        toolDeltaTimes.Add(excavation, 10);
    }

    

    public void RegisterTimeDependant(TimeDependent timeDependent) //register a time dependent object to a list
    {
        timeDependentList.Add(timeDependent);

    }

    public void ChangeDeltaDiscreteTime(GameObject gameObject)
    {
        
        deltaDiscreteTime = toolDeltaTimes[gameObject];

        
    }

    public void LaunchOnTick() //trigger un tick pour tous les elements temps-dependents de valeur deltaDiscreteTime
    {

        discreteTime += deltaDiscreteTime;

        foreach (TimeDependent timeDependent in timeDependentList)
        {
            timeDependent.OnTick(deltaDiscreteTime);

        }

    }
    
    void Update()
    {
        if (Input.GetButtonDown("Jump")) // Quand space alors faire passer le temps de deltaDiscreteTime et OnTick() chaque objet de timeDependent // Test only
        { 
             discreteTime += deltaDiscreteTime;
 
             foreach (TimeDependent timeDependent in timeDependentList)
             {
                timeDependent.OnTick(deltaDiscreteTime);

             }
        }


        if(discreteTime > limitDiscreteTime)
        {
            print(discreteTime);
            print(limitDiscreteTime);
            //Charger scene game over
            Debug.Log("GameOver");

        }
    }


}
