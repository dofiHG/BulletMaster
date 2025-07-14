using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Start()
    {
        if (FindObjectsOfType<DontDestroyOnLoad>().Length > 1)
            Destroy(gameObject);

        else
            DontDestroyOnLoad(gameObject);
    }
}
