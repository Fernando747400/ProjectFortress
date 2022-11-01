using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    private static Pooling instance;

    static Dictionary<int, Queue<GameObject>> pool = new Dictionary<int, Queue<GameObject>>();
    static Dictionary<int, GameObject> parents = new Dictionary<int, GameObject>();

    [SerializeField] private static bool devMode;


    private void Awake()
    {
        if(instance == null)
        { 
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static void Preload(GameObject objectToPool, int amount)
    {
        int id = objectToPool.GetInstanceID();

        GameObject parent = new GameObject();
        parent.name = objectToPool.name + " pool";
        parents.Add(id, parent);

        pool.Add(id, new Queue<GameObject>());

        for(int i = 0; i < amount; i++)
        {
            CreateObject(objectToPool);
        }
    }

    static void CreateObject(GameObject objecToPool)
    {
        int id = objecToPool.GetInstanceID();

        GameObject go = Instantiate(objecToPool) as GameObject;
        go.transform.SetParent(GetParent(id).transform);
        go.SetActive(false);
        pool[id].Enqueue(go);
        if (devMode) return;
        go.GetComponent<ZombiePursuit>().ZombieDieEvent += GameManager.Instance.AddKill;
    }

    static GameObject GetParent(int parentID)
    {
        GameObject parent;
        parents.TryGetValue(parentID, out parent);

        return parent;
    }

    public static GameObject GetObject(GameObject objectToPool)
    {
        int id = objectToPool.GetInstanceID();

        if(pool[id].Count == 0)
        {
            CreateObject(objectToPool);
        }

        GameObject go = pool[id].Dequeue();
        go.SetActive(true);

        return go;
    }

    public static void RecicleObject(GameObject objectToPool, GameObject objectToRecicle)
    {
        int id = objectToPool.GetInstanceID();

        pool[id].Enqueue(objectToRecicle);
        objectToRecicle.SetActive(false);
    }
}
