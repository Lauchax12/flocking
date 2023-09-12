using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patrol : IState
{
    Transform _transform;
    float _speed;
    changestate _change;

    
    
    Transform[] _waypoint;
    int _actualWaypoint;
    float _minDetectagents;
    float _minDetectWaypoint;

    public patrol(changestate change, Transform transform, float speed,Transform[] waypoints,int actualWaypoint, float minDetectagents, float minDetectWaypoint)
    {
        _change = change;
        _transform = transform;
        _speed = speed;
        
        _waypoint = waypoints;
        _actualWaypoint = actualWaypoint;
        _minDetectagents = minDetectagents;
        _minDetectWaypoint = minDetectWaypoint;
    }

    public void OnEnter()
    {
        Debug.Log("toy patrullando");
        
    }
    public void OnUpdate()
    {
        
        hunter.energy -= Time.deltaTime;
        if (hunter.energy >= 1)
        {
            var dir = _waypoint[_actualWaypoint].position - _transform.position;
            _transform.position += dir.normalized * 3 * Time.deltaTime;

            if (dir.magnitude <= _minDetectWaypoint )
            {
                
                    _actualWaypoint++;

                    if (_actualWaypoint >= _waypoint.Length)
                        _actualWaypoint = 0;
               
            }
            

            foreach (var item in Gamemanager.instance.boids)
            {
                Vector3 dist = item.transform.position - _transform.position;

                if (dist.magnitude <= _minDetectagents)
                {
                    Debug.Log("se cruzó");
                    Debug.Log("mi posicion en patrol es " + _transform.position);

                    _change.ChangeState("chase");

                }
            }





        }
        else
        {
            
            _change.ChangeState("idle");
        }
        

       
    }

    public void OnExit()
    {
        
        Debug.Log("deje de patrullar");
    }
}
