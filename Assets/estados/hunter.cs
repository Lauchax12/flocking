using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hunter : MonoBehaviour
{
    public float speed;
    changestate _changestate;
    public static float energy=10;
    public float maxvelocity;
    public Transform[] waypoints;
    int actualWaypoint;
    [Range(0.5f, 10)]
    public float minDetectWaypoint;
    public float minDetectagents;

    Vector3 _velocity;

    boid _boid;

    private void Start()
    {
        _changestate = new changestate();
        _changestate.CreateState("idle", new idle(_changestate,energy));
        _changestate.CreateState("patrol", new patrol(_changestate, transform, speed,waypoints,actualWaypoint,minDetectagents,minDetectWaypoint));
        _changestate.CreateState("chase", new chase(_changestate, transform, speed,  minDetectagents,_velocity, _boid,maxvelocity));

        _changestate.ChangeState("idle");
    }

    private void Update()
    {
        _changestate.Execute();

        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, minDetectWaypoint);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minDetectagents);
    }
}
