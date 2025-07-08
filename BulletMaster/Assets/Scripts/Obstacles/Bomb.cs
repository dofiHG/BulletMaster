using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private SphereCollider _bombCollider;
    [SerializeField] private SphereCollider _explosionCollider;
    [SerializeField] private GameObject _particlesPrefab;

    private List<GameObject> _objectsInTrigger = new List<GameObject>();

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ammo")
        {
            GameObject obj = Instantiate(_particlesPrefab);
            obj.transform.position = _bombCollider.GetComponentInParent<Transform>().position;
        }

        Explore();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged") || other.tag == "Wall" || other.tag == "Ammo")
            return;

        _objectsInTrigger.Add(other.gameObject);
    }

    private void Explore()
    {
        foreach (GameObject obj in _objectsInTrigger)
        {
            if (obj == null) continue;
            if (obj.TryGetComponent(out Stickman stickman))
            {
                var direction = (stickman.transform.position - transform.position).normalized;
                stickman.OnDied(direction);
            }
        }
    }
}


/*using System.Collections.Generic;
using UnityEngine;

namespace Bullet_Master_3D.Scripts.Game
{
    public class Bomb : Ammo
    {
        [Tooltip("Prefab with effect and sound of explosion")]
        [SerializeField] private GameObject _explosionParticlePrefab;

        private readonly List<GameObject> _gameObjectsInTrigger = new List<GameObject>();
        private bool _isExploded;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Untagged")) return;
            //Add gameObjects that are in the area of grenade impact to the list
            _gameObjectsInTrigger.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Untagged")) return;
            //Remove gameObjects that have left the grenade impact zone from list
            _gameObjectsInTrigger.Remove(other.gameObject);
        }

        private void OnCollisionEnter(Collision other)
        {
            //Detonate the grenade in collision with any object
            Explode();
        }

        private void Explode()
        {
            //Checking before boolean variable to prevent the method from working twice
            if (_isExploded) return;
            _isExploded = true;

            //Running through the list of gameObjects in the grenade impact zone and let all the stickmen know they’re dead
            foreach (var gameObject in _gameObjectsInTrigger)
            {
                if (gameObject == null) continue;
                if (gameObject.TryGetComponent(out Stickman stickman))
                {
                    var direction = (stickman.transform.position - transform.position).normalized;
                    stickman.OnStickmanDied(direction, RepulsiveForce);
                }
            }
            //Create a separate object with blast effect and sound; separate, because our grenade is immediately destroyed
            Instantiate(_explosionParticlePrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}*/
