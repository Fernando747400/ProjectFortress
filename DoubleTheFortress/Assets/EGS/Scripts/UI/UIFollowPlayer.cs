using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GameObject _player;

    private Vector3 _initialRotation;

    private void Awake()
    {
        _initialRotation = this.transform.rotation.eulerAngles;
    }

    private void FixedUpdate()
    {
        this.transform.rotation = Quaternion.Euler(_initialRotation.x, _player.transform.rotation.y, _initialRotation.z);
    }
}
