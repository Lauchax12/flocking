using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chase : IState
{
    Transform _transform;
    float _speed;
    changestate _change;
    boid _boidis;



    Transform[] _waypoint;
    int _actualWaypoint;
    float _minDetectagents;
    float _minDetectWaypoint;
    float _maxvelocity;
    Vector3 _velocity;

    public chase(changestate change, Transform transform, float speed,  float minDetectagents, Vector3 velocity, boid boidis,float maxvelocity)
    {
        _change = change;
        _transform = transform;
        _speed = speed;
        _minDetectagents = minDetectagents;
        _velocity = velocity;
        _boidis = boidis;
        _maxvelocity = maxvelocity;
    }

    //boid GetclosestBoid()
    //{

    //    //Vector3 currentpos = _transform.position;
    //    //foreach (boid t in boids)
    //    //{
    //    //    float dist = Vector3.Distance(t.transform.position, currentpos);
    //    //    if (dist < _minDetectagents)
    //    //    {
    //    //        _boidis = t;
    //    //        _minDetectagents = dist;
    //    //    }
    //    //}
    //    //return _boidis;

        
    //}


    public void OnEnter()
    {
        Debug.Log("persiguiendo");
        foreach (var item in Gamemanager.instance.boids)
        {
            Vector3 dist = item.transform.position - _transform.position;

            if (dist.magnitude <= _minDetectagents)
            {
                Debug.Log("se cruzó");
                Debug.Log("mi posicion en patrol es " + _transform.position);

                _boidis = item;
                
            }
        }
    }

    public void OnUpdate()
    {
        Debug.Log("mi posicion en patrol es " + _transform.position);
        hunter.energy -= Time.deltaTime;
        if (hunter.energy >= 1)
        {
            AddForce(Pursuit(_boidis));
            _transform.position += _velocity * _speed * Time.deltaTime;
            _transform.forward = _velocity;
        }
        else
        {
         _change.ChangeState("idle");
        }
        //if (hunter.energy == 1111)
        //    _change.ChangeState("idle");
        
    }

    public void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, _maxvelocity);
        
    }

    Vector3 Pursuit(boid target)
    {
        
        
        Vector3 desired = (target.transform.position + target.Velocity * Time.deltaTime) - _transform.position;
        desired.Normalize();
        desired *= _maxvelocity;

        Vector3 steering = desired - _velocity;
        Debug.Log("posicion del objetivo " + target.transform.position);
        return steering;
    }

    public void OnExit()
    {
        Debug.Log("deje de perseguir");
        _boidis = null;
    }


}
