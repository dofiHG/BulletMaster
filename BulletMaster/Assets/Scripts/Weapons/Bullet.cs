using UnityEngine;

public class Bullet : MonoBehaviour
{
    private static Bullet instance;
    private Rigidbody _rigidbody;
    private int _reboundCount = 3;

    public Vector3 direction;
    
    [SerializeField] private float _speed;

    private void Awake()
    {
        if (instance == null)
        { instance = this; }
    }

    private void Start() => _rigidbody = GetComponent<Rigidbody>();

    private void FixedUpdate() => MoveBullet();

    private void MoveBullet() => _rigidbody.velocity = direction * _speed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Stickman stickman))
            stickman.OnDied(direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            direction = Vector3.Reflect(direction, collision.contacts[0].normal).normalized;
            _reboundCount--;
            if (_reboundCount == 0)
                Destroy(gameObject);
        }
    }
}
