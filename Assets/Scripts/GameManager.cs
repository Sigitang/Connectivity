using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{

    public int discreteTime;
    public int deltaDiscreteTime = 2; //cout en temps a recolter
    readonly List<TimeDependent> timeDependentList;
    

    GameManager() //Constructeur de la classe = premier truc appele lors de la creation de l'objet 1ere priorite
    {
        timeDependentList = new List<TimeDependent>();

    }

    void Start()
    {
        discreteTime = 0; //time = 0 au debut du jeu
        
    }

    

    public void RegisterTimeDependant(TimeDependent timeDependent) //register a time dependent object to a list
    {
        timeDependentList.Add(timeDependent);

    }



    
    void Update()
    {
        if (Input.GetButtonDown("Jump")) // Quand clique souris alors faire passer le temps de deltaDiscreteTime et OnTick() chaque objet de timeDependent
        { 
             discreteTime =+ deltaDiscreteTime;
 
             foreach (TimeDependent timeDependent in timeDependentList)
             {
                timeDependent.OnTick(deltaDiscreteTime);

             }
        }
    }


}
