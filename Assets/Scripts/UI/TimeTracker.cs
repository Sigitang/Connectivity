using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeTracker : MonoBehaviour
{
    private GameManager gameManager;
    public TextMeshProUGUI timeTrackerText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        timeTrackerText.text =  ("Time passed: " + gameManager.discreteTime.ToString());

    }
}
