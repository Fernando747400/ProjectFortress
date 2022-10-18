using System.Collections;
using UnityEngine;

public class Debug_Hitmarker : MonoBehaviour
{
    //FOR DEBUGGING PURPOSES ONLY
    void Start()
    {
        StartCoroutine(corDestroy());
    }
    IEnumerator corDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
