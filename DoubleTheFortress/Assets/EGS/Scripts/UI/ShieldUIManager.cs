using System.Collections.Generic;
using UnityEngine;

public class ShieldUIManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private List<GameObject> _shieldList;

    public void UpdateShields(int index)
    {
        for (int i = 0; i < _shieldList.Count; i++)
        {
            if (i <= index - 1)
            {
                _shieldList[i].SetActive(true);
            }
            else
            {
                _shieldList[i].SetActive(false);
            }
        }

    }
}
