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

    private void Awake()
    {
        Instance = this;
    }

    public Queue<Transform> SelectRoute()
    {
        theRoute.Clear();
        switch(myRoute)
        {
            case Route.One:
                return BuildQueue(routeOne);
            case Route.Two:
                return BuildQueue(routeTwo);
            case Route.Three:
                return BuildQueue(routeThree);
            case Route.Four:
                return BuildQueue(routeFour);
        }
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
