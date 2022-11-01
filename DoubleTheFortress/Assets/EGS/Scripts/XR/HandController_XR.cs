using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController_XR : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer handSkinnedMesh;
    private Vector3 _handPos;
    
    public Vector3 HandPos
    {
        get { return transform.position; }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void HandleHandsVisible(bool visible)
    {
        handSkinnedMesh.enabled = visible;
    }
    
    
    
}
