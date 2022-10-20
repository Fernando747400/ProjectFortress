using UnityEngine;
using System;


public interface IGrabbable 
{
    Vector3 Position
    {
        get;
        set;
    }

    Vector3 Rotation
    {
        get;
        set;
    }


    public void ReposObject()
    {
        Debug.Log("hola");
    }
   public void DoAction()
   {
       Debug.Log("hola");
   }

}
