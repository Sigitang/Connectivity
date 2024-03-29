using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    public UnityEvent chainsawButtonTrigger;
    public GameObject chainsaw;
    public UnityEvent chainsawUsed;
    public UnityEvent excavationButtonTrigger;
    public GameObject excavation;
    public UnityEvent excavationUsed;
    public bool toolActivated;
    public GameObject ecopath;
    public UnityEvent ecopathUsed;
    public UnityEvent ecopathButtonTrigger;


    private void Awake()
    {
        current = this;
    }

    

    private void Start()
    {

       

        toolActivated = false;

        chainsawButtonTrigger.AddListener(ToolActivated); //Signale qu'un tool est active
        chainsawButtonTrigger.AddListener(ActivateChainsaw); //Active le script du tool
        chainsawUsed.AddListener(ActivateChainsaw); //D�sactive une fois utilise

        excavationButtonTrigger.AddListener(ToolActivated);
        excavationButtonTrigger.AddListener(ActivateExcavation);
        excavationUsed.AddListener(ActivateExcavation);

        excavationButtonTrigger.AddListener(ToolActivated);
        excavationButtonTrigger.AddListener(ActivateEcopath);
        excavationUsed.AddListener(ActivateEcopath);





    }


    public void ChainSawButton()
    {
        chainsawButtonTrigger.Invoke();


    }

    public void ExcavationButton()
    {
        excavationButtonTrigger.Invoke();

    }

    public void EcopathButton()
    {
        excavationButtonTrigger.Invoke();

    }


    private void ActivateChainsaw()
    {
      if(chainsaw.GetComponent<ChainsawController>().enabled == true)
        {

            chainsaw.GetComponent<ChainsawController>().enabled = false;
            

        }
        else
        {
           
            
                chainsaw.GetComponent<ChainsawController>().enabled = true;
           
        }
      

    }

    private void ActivateExcavation()
    {
        if (excavation.GetComponent<ExcavationController>().enabled == true)
        {

            excavation.GetComponent<ExcavationController>().enabled = false;


        }
        else
        {
                      
                excavation.GetComponent<ExcavationController>().enabled = true;

        }




    }


    private void ActivateEcopath()
    {
        if (ecopath.GetComponent<EcopathController>().enabled == true)
        {

            ecopath.GetComponent<EcopathController>().enabled = false;


        }
        else
        {

            ecopath.GetComponent<EcopathController>().enabled = true;

        }




    }

    private void ToolActivated()
    {
        if (toolActivated == true)
        {
            toolActivated = false;

        }

        else
        {
            toolActivated = true;
        }


    }

}
