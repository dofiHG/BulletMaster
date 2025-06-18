using UnityEngine;

public class Shotgun : Weapon
{
    private void Update()
    {
        CheskLaserState();

        if (Input.GetMouseButtonUp(0) && LevelSettings.instance.canShoot)
            Shoot(2);
    }

    private void CheskLaserState()
    {
        if (Input.GetMouseButton(0) && LevelSettings.instance.canShoot)
            DrawLaser();
        else
            HideLaser();
    }
}
