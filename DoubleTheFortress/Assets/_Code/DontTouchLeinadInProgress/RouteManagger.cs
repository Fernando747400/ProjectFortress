using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManagger : MonoBehaviour
{
    public static RouteManagger Instance;
    public enum Route { One, Two, Three, Four}

    public Route myRoute;

    public List<Transform> routeOne = new List<Transform>();
    public List<Transform> routeTwo = new List<Transform>();
    public List<Transform> routeThree = new List<Transform>();
    public List<Transform> routeFour = new List<Transform>();

    private Queue<Transform> theRoute = new Queue<Transform>();

    private int randomNum;

    private void Awake()
    {
        Instance = this;
    }

    private void RandomNum()
    {
        
        
    }

    public Queue<Transform> RandomRoute()
    {
        theRoute.Clear();
        int randomRoute = Random.Range(1, 5);
        randomNum = randomRoute;
        switch (randomNum)
        {
            case 1:
                return BuildQueue(routeOne);
            case 2:
                return BuildQueue(routeTwo);
            case 3:
                return BuildQueue(routeThree);
            case 4:
                return BuildQueue(routeFour);
        }
        Vector3 nullvect = new Vector3(0, 0, 0);
        return null;
    }

    private Queue<Transform> BuildQueue(List<Transform> list)
    {
        Queue<Transform> route = new Queue<Transform>();

        foreach (Transform t in list)
        {
            route.Enqueue(t);
        }
        return route;
    }
}
