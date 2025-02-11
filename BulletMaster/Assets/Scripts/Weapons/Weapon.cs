using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;

    [SerializeField] private Transform gunStartPosition;
    [SerializeField] private Transform gunEndPosition;

    [SerializeField] private Vector3 _defaultLocalPosition;
    [SerializeField] private Vector3 _defaultLocalEulerAngles;

    private LineRenderer _lineRenderer;
    private Vector3 direction;
    [SerializeField][Range(0f, 100f)] protected float _maxLaserLength;

    private void Start()
    {
        transform.localPosition = _defaultLocalPosition;
        transform.localEulerAngles = _defaultLocalEulerAngles;
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
            DrawLaser();
        else
            HideLaser();
    }

    private void DrawLaser()
    {
        _lineRenderer.positionCount = 2;
        var laserLength = _maxLaserLength;

        direction = (gunEndPosition.position - gunStartPosition.position).normalized;

        Ray ray = new Ray(gunEndPosition.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            float distance = Vector3.Distance(gunEndPosition.position, hit.point);

            laserLength = distance > 0 ? distance : 0;
        }

        _lineRenderer.SetPosition(0, gunEndPosition.position);
        _lineRenderer.SetPosition(1, gunEndPosition.position + direction * laserLength);
    }

    private void HideLaser() => _lineRenderer.positionCount = 0;

    protected void Shoot(int type)
    {
        type = 0;

        Instantiate(_bullet, gunEndPosition);
        _bullet.GetComponent<Bullet>().Direction = direction;
    }
}
