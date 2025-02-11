using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public static LevelSettings instance;

    public GameObject[] weapons;

    private void Awake()
    {
        if (instance == null )
            instance = this;
    }
}
