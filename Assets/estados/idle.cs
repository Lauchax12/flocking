using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idle : IState
{
    changestate _change;
    float _energy;
    public idle(changestate change,float energy)
    {
        _change = change;
        _energy = energy;
    }

    public void OnEnter()
    {
        Debug.Log("toy idle");
        
        
    }

    public void OnUpdate()
    {

        hunter.energy += Time.deltaTime;
        
        if (hunter.energy >= 10)
        {
            _change.ChangeState("patrol");
        }
        


        
    }

    public void OnExit()
    {
        Debug.Log("termine idle");
    }
}
