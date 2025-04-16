using UnityEngine;

public class Pistol : Weapon
{
    private void Update()
    {
        CheskLaserState();

        if (Input.GetMouseButtonUp(0) && LevelSettings.instance.canShoot)
            Shoot(1);
    }

    private void CheskLaserState()
    {
        if (Input.GetMouseButton(0) && LevelSettings.instance.canShoot)
            DrawLaser();
        else
            HideLaser();
    }
}
