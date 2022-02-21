using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{


    public int gameTime;
    List<TimeDependent> timeDependentList;
    public int timeCost; // A coder : cout en temps

    GameManager() //Constructeur de la classe = premier truc appele lors de la creation de l'objet 1ere priorite
    {
        timeDependentList = new List<TimeDependent>();

    }

    void Start()
    {
        gameTime = 0;
        
    }

    

    public void RegisterTimeDependant(TimeDependent timeDependent)
    {
        timeDependentList.Add(timeDependent);

    }



    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        { 
             gameTime = gameTime + timeCost;
 
             foreach (TimeDependent timeDependent in timeDependentList)
             {
                timeDependent.OnTick();

             }
        }
    }


}
