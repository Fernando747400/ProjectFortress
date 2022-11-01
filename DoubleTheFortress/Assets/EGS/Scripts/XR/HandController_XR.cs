using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController_XR : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer handSkinnedMesh;

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
