using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{

    Dictionary<int, Queue<GameObject>> pool = new Dictionary<int, Queue<GameObject>>();
    Dictionary<int, GameObject> parents = new Dictionary<int, GameObject>();

    public List<GameObject> poolList = new List<GameObject>();

    public event Action <GameObject> OnAddEvent;

    public void Preload(GameObject objectToPool, int amount)
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

    void CreateObject(GameObject objecToPool)
    {
        int id = objecToPool.GetInstanceID();

        GameObject go = Instantiate(objecToPool) as GameObject;
        go.transform.SetParent(GetParent(id).transform);
        go.SetActive(false);
        pool[id].Enqueue(go);
        poolList.Add(objecToPool);
        OnAddEvent?.Invoke(objecToPool);
    }

    GameObject GetParent(int parentID)
    {
        GameObject parent;
        parents.TryGetValue(parentID, out parent);

        return parent;
    }

    public GameObject GetObject(GameObject objectToPool)
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

    public void RecicleObject(GameObject objectToPool, GameObject objectToRecicle)
    {
        int id = objectToPool.GetInstanceID();

        pool[id].Enqueue(objectToRecicle);
        objectToRecicle.SetActive(false);
    }
}
