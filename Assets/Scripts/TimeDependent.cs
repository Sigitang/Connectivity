using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeDependent : MonoBehaviour //abstract --> Ne peut pas instancier un objet de ce type (ne peut que instancier un objet qui dérive de cette classe)
{
    private GameManager gameManager;

    protected virtual void Start()
    {
       gameManager = FindObjectOfType<GameManager>();
       
       gameManager.RegisterTimeDependant(this);

    }

    public virtual void OnTick() //Virtual = peut etre override
    {
        

    }
}

