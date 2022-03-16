using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class GameManager : MonoBehaviour
{

    public int discreteTime;
    public int deltaDiscreteTime = 0; //cout en temps a recolter
    readonly List<TimeDependent> timeDependentList;
    readonly Dictionary<GameObject, int> toolDeltaTimes = new Dictionary<GameObject, int>();
    public GameObject chainsaw;
    
    
  
    GameManager() //Constructeur de la classe = premier truc appele lors de la creation de l'objet 1ere priorite
    {
        timeDependentList = new List<TimeDependent>();

    }

    void Start()
    {
       
        

        GameEvents.current.chainsawUsed.AddListener(delegate { ChangeDeltaDiscreteTime(chainsaw); }); //inscrit Changedeltadiscretetime a l'event d'utilisation de la chainsaw
        GameEvents.current.chainsawUsed.AddListener(LaunchOnTick);

        discreteTime = 0; //time = 0 au debut du jeu

        //Chercher les tools
        
        toolDeltaTimes.Add(chainsaw, 2); //Ajouter tools au fur et a mesure avec le coût en temps en value
        
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
        if (Input.GetButtonDown("Jump")) // Quand clique souris alors faire passer le temps de deltaDiscreteTime et OnTick() chaque objet de timeDependent
        { 
             discreteTime += deltaDiscreteTime;
 
             foreach (TimeDependent timeDependent in timeDependentList)
             {
                timeDependent.OnTick(deltaDiscreteTime);

             }
        }
    }


}
