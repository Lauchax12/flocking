using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boid : MonoBehaviour
{
    Vector3 _velocity;
    public float maxSpeed;
    public float maxForce;
    public float viewRadius = 3;
    public float radiusSeparation = 5;
    public GameObject hunter;
    public LayerMask ObstacleLayermask;
    public float arriveRadius;

    food foodobjetive;

    public Vector3 Velocity
    {
        get { return _velocity; }
    }

    

    void Start()
    {
        AddForce((new Vector3(Random.Range(-1f, 1f),0 , Random.Range(-1f, 1f)).normalized * maxSpeed));
        hunter = GameObject.FindWithTag("hunter");

        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * maxForce;
        AddForce(randomDir);

        Gamemanager.instance.AddBoid(this);


    }

    void Update()
    {
       
        AddForce(Separation() * Gamemanager.instance.weightSeparation);
        AddForce(Cohesion() * Gamemanager.instance.weightCohesion);
        AddForce(Alignment() * Gamemanager.instance.weightAlignment);
        AddForce(Avoidance() * Gamemanager.instance.weightAvoidance);
        AddForce(Evade());
        
        transform.position += _velocity * Time.deltaTime;
        transform.forward = _velocity;

        transform.position = Gamemanager.instance.ApplyBound(transform.position);

        foreach(var food in Gamemanager.instance.foods)
        {
            foodobjetive = food;
            Vector3 dist = foodobjetive.transform.position - transform.position;
            if (dist.magnitude <= viewRadius)
            {
                if (foodobjetive != null)
                    AddForce(Arrive(foodobjetive.transform.position));
                else
                    continue;
            }
        }

        //bloqueo de movimiento(no se mueve arriba o abajo)
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
    }





    Vector3 Alignment()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;
        foreach (var item in Gamemanager.instance.boids)
        {
            if (item == this)
                continue;

            Vector3 dist = item.transform.position - transform.position;

            if (dist.magnitude <= viewRadius)
            {
                desired += item._velocity;
                count++;
            }
        }

        if (count <= 1)
            return desired;

        desired /= count;

        desired.Normalize();
        desired *= maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Cohesion()
    {
        Vector3 desired = Vector3.zero;
        int count = 0;

        foreach (var item in Gamemanager.instance.boids)
        {
            if (item == this)
                continue;

            Vector3 dist = item.transform.position - transform.position;

            if (dist.magnitude <= viewRadius)
            {
                desired += item.transform.position;
                count++;
            }
        }

        if (count <= 1)
            return desired;

        desired /= count;
        desired -= transform.position;

        desired.Normalize();
        desired *= maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Separation()
    {
        Vector3 desired = Vector3.zero;

        foreach (var item in Gamemanager.instance.boids)
        {
            Vector3 dist = item.transform.position - transform.position;

            if (dist.magnitude <= radiusSeparation)
                desired += dist;
        }

        if (desired == Vector3.zero)
            return desired;

        desired = -desired;

        desired.Normalize();
        desired *= maxForce;

        return CalculateSteering(desired);
    }

    Vector3 Evade()
    {
        Vector3 desired = Vector3.zero;
        foreach (var item in Gamemanager.instance.boids)
        {
            Vector3 dist = hunter.transform.position - transform.position;

            if (dist.magnitude <= viewRadius)
                desired += dist;
        }
        if(desired == Vector3.zero)
            return desired;

        desired = -desired;

        desired.Normalize();
        desired *= maxForce;

        return CalculateSteering(desired);

    }

    Vector3 Seek(Vector3 targetPos)
    {
        return Seek(targetPos, maxSpeed);
    }

    Vector3 Seek(Vector3 targetPos, float maxspeed)
    {
       

        return CalculateSteering((targetPos - transform.position).normalized * maxspeed);
    }

    Vector3 Arrive(Vector3 targetPos)
    {
        
        if (Vector3.Distance(transform.position, targetPos) > arriveRadius) return Seek(targetPos);

        return Seek(targetPos, (maxSpeed * (Vector3.Distance(transform.position, targetPos) / arriveRadius)));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "food")
        {
            foodobjetive = null;
        }
    }

    Vector3 Avoidance()
    {
        

        if (Physics.Raycast(transform.position+transform.right/2,transform.forward, viewRadius, ObstacleLayermask))
        {
            foreach (var item in Gamemanager.instance.boids)
            {
                if (item == this)
                    continue;
                return CalculateSteering((transform.position + transform.right) * maxSpeed);
            }
               
        }
        else if (Physics.Raycast(transform.position - transform.right / 2, transform.forward, viewRadius, ObstacleLayermask))
        {
            foreach (var item in Gamemanager.instance.boids)
            {
                if (item == this)
                    continue;
                return CalculateSteering((transform.position - transform.right) * maxSpeed);
            }
                
        }

        return Vector3.zero;
    }

    Vector3 CalculateSteering(Vector3 desired)
    {
        return Vector3.ClampMagnitude(desired - _velocity, maxSpeed);
    }

    void AddForce(Vector3 force)
    {
        _velocity = Vector3.ClampMagnitude(_velocity + force, maxForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSeparation);

        Vector3 oringinpoint = transform.position + transform.right / 2;
        Vector3 oringinpoint2 = transform.position - transform.right / 2;

        Gizmos.DrawLine(oringinpoint,oringinpoint + transform.forward * viewRadius);
        Gizmos.DrawLine(oringinpoint2, oringinpoint2 + transform.forward * viewRadius);
    }

}
