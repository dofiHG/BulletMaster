using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private static Bullet instance;

    private Vector2 _direction;
    private Rigidbody _rigidbody;
    private int maxSpeed = 10;

    [SerializeField] private float _speed;

    public Vector2 Direction { set => _direction = value; }

    private void Awake()
    {
        if (instance == null)
            { instance = this; }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveBullet();
    }

    private void MoveBullet()
    {
        _rigidbody.AddForce(_speed * Time.deltaTime * _direction);
        if (_rigidbody.velocity.magnitude > maxSpeed)
            _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
    }
}
