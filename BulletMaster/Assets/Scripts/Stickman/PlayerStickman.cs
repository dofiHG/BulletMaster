using UnityEngine;

public class PlayerStickman : Stickman
{
    [SerializeField] private int _weaponNumber;

    public Camera _camera;

    private void Awake()
    {
        GiveWeapon(_weaponNumber);
        Setup();
    }

    private void Update()
    {
        RotatePlayer();     
    }

    private void RotatePlayer()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out var distance))
        {
            var mouseWorldPos = ray.GetPoint(distance);

            transform.LookAt(new Vector3(mouseWorldPos.x, transform.position.y, mouseWorldPos.z));
        }
    }
}
