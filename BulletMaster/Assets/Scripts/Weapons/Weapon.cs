using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private int _angelForShotgun = 30;
    private int _bulletsInShoutgun = 5;
    [SerializeField][Range(0f, 100f)] protected float _maxLaserLength;

    public Action shoot;

    private void Start()
    {
        transform.localPosition = _defaultLocalPosition;
        transform.localEulerAngles = _defaultLocalEulerAngles;
        _lineRenderer = GetComponent<LineRenderer>();
        _shootSound = GetComponent<AudioSource>();
    }
    

    protected void DrawLaser()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
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
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (type == 1)
        {
            GameObject bullet = Instantiate(_bullet);
            bullet.transform.position = gunEndPosition.position;
            bullet.GetComponent<Bullet>().direction = direction;
            _shootSound.Play();
            shoot?.Invoke();
        }
        else if (type == 2)
        {
            float angleStep = _angelForShotgun / (_bulletsInShoutgun - 1);
            float startAngle = -_angelForShotgun / 2f;

            for (int i = 0; i != _bulletsInShoutgun; i++)
            {
                GameObject bullet = Instantiate(_bullet);
                bullet.transform.position = gunEndPosition.position;

                float currentAngle = startAngle + (angleStep * i);

                Vector3 bulletDirection = Quaternion.Euler(0, currentAngle, 0) * direction;

                bullet.GetComponent<Bullet>().direction = bulletDirection;
            }
            _shootSound.Play();
            shoot?.Invoke();
        }
    }
}
