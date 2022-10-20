using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Vector3 Offset;
    public Transform Reference;
    private Vector3 startPos; 
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if (Reference != null)
        {
            transform.position = new Vector3(Reference.position.x, startPos.y, Reference.position.z) + Offset;
        }
    }
}
