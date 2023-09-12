using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    [SerializeField]
    public float width = 15;
    [SerializeField]
    public float height = 9;

    public List<boid> boids = new List<boid>();
    public List<food> foods = new List<food>();

    [Range(0, 3)]
    public float weightSeparation = 1;
    [Range(0, 3)]
    public float weightCohesion = 1;
    [Range(0, 3)]
    public float weightAlignment = 1;
    [Range(0, 3)]
    public float weightAvoidance = 1;
    public static Gamemanager instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddBoid(boid b)
    {
        if (!boids.Contains(b))
            boids.Add(b);
    }

    public void AddFood(food f)
    {
        if (!foods.Contains(f))
            foods.Add(f);
    }

    public Vector3 ApplyBound(Vector3 objectPosition)
    {
        if (objectPosition.x > width)
            objectPosition.x = -width;
        if (objectPosition.x < -width)
            objectPosition.x = width;

        if (objectPosition.z > height)
            objectPosition.z = -height;
        if (objectPosition.z < -height)
            objectPosition.z = height;

        return objectPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 topLeft = new Vector3(-width, 0, height);
        Vector3 topRight = new Vector3(width, 0, height);
        Vector3 botRight = new Vector3(width, 0, -height);
        Vector3 botLeft = new Vector3(-width, 0, -height);

        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, botRight);
        Gizmos.DrawLine(botRight, botLeft);
        Gizmos.DrawLine(botLeft, topLeft);
    }
}
