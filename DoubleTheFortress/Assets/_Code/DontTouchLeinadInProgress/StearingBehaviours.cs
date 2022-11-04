using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StearingBehaviours : MonoBehaviour
{
    [Header("Steering Dependencies")]
    public float speed;
    public Vector3 velocity;
    public float Mass;

    [Header("Seek")]
    public Vector3 desiredVelocity;

    [Header("Pursuit")]
    public Vector3 VPF; //Velocity per Frame
    public Vector3 tempPosition;
    public Vector3 futurePosition;
    public float changeInT;

    [Header("Avoid")]
    public float maxSeeAhead;
    public float maxAvoidForce;
    public GameObject avoidGameObject;

    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 distance = targetPos - transform.position;
        desiredVelocity = distance.normalized * speed;
        Vector3 steering = desiredVelocity - velocity;
        return steering;
    }

    public Vector3 Pursuit(Vector3 targetPos)
    {
        Vector3 actualVelocity = (targetPos - tempPosition) / Time.deltaTime;
        VPF = Vector3.Lerp(VPF, actualVelocity, 0.1f);
        tempPosition = targetPos;
        futurePosition = targetPos + (VPF * changeInT);
        return Seek(futurePosition);
    }

    public Vector3 Avoid()
    {
        Vector3 seeAhead = transform.position + (velocity.normalized * maxSeeAhead / 2);
        //to do rotate 
        Vector3 avoidanceForce = seeAhead - avoidGameObject.transform.position;
        avoidanceForce = (avoidanceForce.normalized) * maxAvoidForce;

        return avoidanceForce;
    }
}
