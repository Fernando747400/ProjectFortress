using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManagger : MonoBehaviour
{
    public static RouteManagger Instance;
    public enum Route { One, Two, Three, Four}

    public Route myRoute;

    private Queue<Transform> theRoute = new Queue<Transform>();

    private int randomNum;

    [System.Serializable]
    public class Points
    {
        public List<Transform> list;
    }

    [System.Serializable]
    public class PointsList
    {
        public List<Points> listOfList;
    }

    public PointsList ListOfPointLists = new PointsList();
    public PointsList SpecialRoutes = new PointsList();

    private void Awake()
    {
        Instance = this;
    }

    public Queue<Transform> RandomRoute()
    {
        theRoute.Clear();
        int randomRoute = Random.Range(1, 13);
        randomNum = randomRoute;
        switch (randomNum)
        {
            case 1:
                return BuildQueue(ListOfPointLists.listOfList[0].list);
            case 2:
                return BuildQueue(ListOfPointLists.listOfList[1].list);
            case 3:
                return BuildQueue(ListOfPointLists.listOfList[2].list);
            case 4:
                return BuildQueue(ListOfPointLists.listOfList[3].list);
            case 5:
                return BuildQueue(ListOfPointLists.listOfList[4].list);
            case 6:
                return BuildQueue(ListOfPointLists.listOfList[5].list);
            case 7:
                return BuildQueue(ListOfPointLists.listOfList[6].list);
            case 8:
                return BuildQueue(ListOfPointLists.listOfList[7].list);
            case 9:
                return BuildQueue(ListOfPointLists.listOfList[8].list);
            case 10:
                return BuildQueue(ListOfPointLists.listOfList[9].list);
            case 11:
                return BuildQueue(ListOfPointLists.listOfList[10].list);
            case 12:
                return BuildQueue(ListOfPointLists.listOfList[11].list);
        }
        Vector3 nullvect = new Vector3(0, 0, 0);
        return null;
    }

    public Queue<Transform> RandomSpecialRoute()
    {
        theRoute.Clear();
        int randomRoute = Random.Range(1, 5);
        randomNum = randomRoute;
        switch (randomNum)
        {
            case 1:
                return BuildQueue(SpecialRoutes.listOfList[0].list);
            case 2:
                return BuildQueue(SpecialRoutes.listOfList[1].list);
            case 3:
                return BuildQueue(SpecialRoutes.listOfList[2].list);
            case 4:
                return BuildQueue(SpecialRoutes.listOfList[3].list);
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
