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
    public bool toolActivated;


    private void Awake()
    {
        current = this;
    }

    

    private void Start()
    {

       

        toolActivated = false;

        chainsawButtonTrigger.AddListener(ToolActivated); 
        chainsawButtonTrigger.AddListener(ActivateChainsaw);
        chainsawUsed.AddListener(ActivateChainsaw);
        



    }


    public void ChainSawButton()
    {
        chainsawButtonTrigger.Invoke();


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
