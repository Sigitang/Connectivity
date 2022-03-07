using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public UnityEvent chainsawButtonTrigger;
    public GameObject chainsaw;
    public bool toolActivated;
    

    private void Start()
    {
        
        toolActivated = false;

        chainsawButtonTrigger.AddListener(ToolActivated);
        chainsawButtonTrigger.AddListener(ActivateChainsaw);
        

    }

    public void Update()
    {
  
        

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
