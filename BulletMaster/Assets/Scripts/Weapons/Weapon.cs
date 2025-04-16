using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;

    [SerializeField] private Transform gunStartPosition;
    [SerializeField] private Transform gunEndPosition;

    [SerializeField] private Vector3 _defaultLocalPosition;
    [SerializeField] private Vector3 _defaultLocalEulerAngles;

    private AudioSource _shootSound;
    private LineRenderer _lineRenderer;
    private Vector3 direction;
    [SerializeField][Range(0f, 100f)] protected float _maxLaserLength;

    public Action shoot;

    private void Start()
    {
        transform.localPosition = _defaultLocalPosition;
        transform.localEulerAngles = _defaultLocalEulerAngles;
        _lineRenderer = GetComponent<LineRenderer>();
        _shootSound = GetComponent<AudioSource>();
    }

    protected Vector3 CalculateDirection()
    {
        return Vector3.zero;
    }

    protected void DrawLaser()
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
    
    protected void HideLaser() => _lineRenderer.positionCount = 0;

    protected void Shoot(int type)
    {
        GameObject bullet = Instantiate(_bullet);
        bullet.transform.position = gunEndPosition.position;
        bullet.GetComponent<Bullet>().direction = direction;
        _shootSound.Play();

        shoot?.Invoke();
    }
}
