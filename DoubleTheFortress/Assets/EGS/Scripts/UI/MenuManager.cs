using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public string Scene;
    public bool IsExitButton = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MenuCanonBall") || other.CompareTag("Bullet"))
        {
            Debug.Log("Me han disparado");
            if (!IsExitButton) SceneTransition.Instance.LoadScene(Scene);
            else SceneTransition.Instance.Quit();
        }

        if (other.CompareTag("Bullet") && IsExitButton)
        {
            SceneTransition.Instance.Quit();
        }
    }
}
