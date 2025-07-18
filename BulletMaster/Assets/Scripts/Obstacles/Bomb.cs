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
