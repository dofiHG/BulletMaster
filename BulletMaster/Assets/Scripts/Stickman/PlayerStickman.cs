using Unity.VisualScripting;
using UnityEngine;

public class PlayerStickman : Stickman
{
    [SerializeField] private int _weaponNumber;

    private Camera _camera;

    private void Start()
    {
        GameObject dontDestroy = GameObject.Find("DontDestroyOnLoad");
        _camera = dontDestroy.transform.Find("Main Camera").GetComponent<Camera>();
        GiveWeapon(_weaponNumber);
        Setup();
    }

    private void Update() => RotatePlayer();

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
