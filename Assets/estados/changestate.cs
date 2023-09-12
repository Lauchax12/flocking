using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changestate 
{

    Dictionary<string, IState> _states = new Dictionary<string, IState>();
    IState _currentstate;

    public void CreateState(string name,IState state)
    {
        if (!_states.ContainsKey(name))
        {
            _states.Add(name, state);
        }
    }
    
    public void Execute()
    {
        _currentstate.OnUpdate();
    }

    public void ChangeState(string name)
    {
        if (_states.ContainsKey(name))
        {
            if (_currentstate != null)
            {
                _currentstate.OnExit();
            }
            _currentstate = _states[name];
            _currentstate.OnEnter();
        }
    }

}
