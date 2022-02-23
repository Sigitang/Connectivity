using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{

    public int discreteTime;
    public int deltaDiscreteTime = 2; //cout en temps a recolter
    List<TimeDependent> timeDependentList;
    

    GameManager() //Constructeur de la classe = premier truc appele lors de la creation de l'objet 1ere priorite
    {
        timeDependentList = new List<TimeDependent>();

    }

    void Start()
    {
        discreteTime = 0;
        
    }

    

    public void RegisterTimeDependant(TimeDependent timeDependent)
    {
        timeDependentList.Add(timeDependent);

    }



    
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // jouer une carte
        { 
             discreteTime =+ deltaDiscreteTime;
 
             foreach (TimeDependent timeDependent in timeDependentList)
             {
                timeDependent.OnTick(deltaDiscreteTime);

             }
        }
    }


}
