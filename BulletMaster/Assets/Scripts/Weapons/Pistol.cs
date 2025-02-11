using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
            Shoot(0);
    }
}
