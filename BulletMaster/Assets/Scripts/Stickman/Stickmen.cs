using System;
using UnityEngine;

public class Stickman : MonoBehaviour
{
    [SerializeField] private Transform _weaponParent;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Rigidbody[] _allRigidbodies;
    [SerializeField] private ParticleSystem _bloodParticles;
    [HideInInspector] public Action onDied;
    [HideInInspector] public GameObject currentWeapon;

    private Animator _animator;
    private AudioSource _deathSound;
    private int _force = 60;
    private Collider _mainCollider;

    protected void Setup()
    {
        _animator = GetComponent<Animator>();
        _mainCollider = GetComponent<Collider>();
        _deathSound = GetComponent<AudioSource>();
        ConvertRigidbodies(true);
    }

    protected void GiveWeapon(int weaponNumber)
    {
        if ( weaponNumber != -1)
        {
            GameObject weapon = Instantiate(LevelSettings.instance.weapons[weaponNumber].gameObject, _weaponParent);
            currentWeapon = weapon;
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
        ConvertRigidbodies(false);
        _rigidbody.AddForce(direction * _force, ForceMode.Impulse);
        _bloodParticles.Play();
        _deathSound.Play();
        _mainCollider.enabled = false;
        
        onDied?.Invoke();

        this.enabled = false;
    }

    private void ConvertRigidbodies(bool isKinematic)
    {
        foreach (Rigidbody rigidbody in _allRigidbodies)
            rigidbody.isKinematic = isKinematic;
    }
}
