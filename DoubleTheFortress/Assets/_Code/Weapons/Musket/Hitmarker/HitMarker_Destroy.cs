using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker_Destroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(corDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator corDestroy()
    {
        yield return new WaitForSeconds(5f);
        
        GameObject.Destroy(gameObject);

    }
}
