using System;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ParticleSystem _bloodParticles;
    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public Action onDied;

    private Animator _animator;
    private AudioSource _deathSound;
    private int _force = 60;
    private Collider _mainCollider;

    protected void Setup()
    {
        _animator = GetComponent<Animator>();
        _mainCollider = GetComponent<Collider>();
        _deathSound = GetComponent<AudioSource>();
    }

    protected void GiveWeapon(int weaponNumber)
    {
        if ( weaponNumber != -1)
        {
            GameObject weapon = Instantiate(LevelSettings.instance.weapons[weaponNumber].gameObject, _weaponParent);
            LevelSettings.instance.weapon = weapon.GetComponent<Weapon>();
            gameObject.GetComponent<Animator>().SetInteger("Weapon", 1);
        }
    }

    protected Quaternion RorateAngle()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 direction = new Vector3(mousePosition.x, transform.position.y, mousePosition.z) - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);

        return rotation;
    }

    public void OnDied(Vector3 direction)
    {
        _animator.enabled = false;
        _rigidbody.AddForce(direction * _force, ForceMode.Impulse);
        _bloodParticles.Play();
        _deathSound.Play();
        _mainCollider.enabled = false;
        isAlive = false;

        onDied?.Invoke();
    }
}
