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
        Debug.Log(_player.transform.rotation);
        this.transform.rotation = Quaternion.Euler(_initialRotation.x, _player.transform.localEulerAngles.y, _initialRotation.z);
    }
}
