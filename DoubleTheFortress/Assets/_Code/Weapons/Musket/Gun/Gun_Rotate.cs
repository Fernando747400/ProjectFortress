using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Rotate : MonoBehaviour
{
    protected float mouseX, mouseY;
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Application.targetFrameRate = 60;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        mouseX += Input.GetAxis("Mouse X");
        mouseY += Input.GetAxis("Mouse Y");
        
        transform.localRotation = Quaternion.Euler(-mouseY,mouseX,0f);
    }
}
