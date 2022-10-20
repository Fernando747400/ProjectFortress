using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    float rotateToX, rotateToY, rotateToZ;
    public int speedRotationX, speedRotationY, speedRotationZ;

    // Update is called once per frame
    void Update()
    {
        rotateToX += Time.deltaTime * speedRotationX;
        rotateToY += Time.deltaTime * speedRotationY;
        rotateToZ += Time.deltaTime * speedRotationZ;
        //transform.rotation = Quaternion.Euler(rotateToX,rotateToY,rotateToZ);
        transform.Rotate(new Vector3(speedRotationX,speedRotationY,speedRotationZ) * Time.deltaTime);
    }
}
