using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _exit;

    private bool _canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (_canTeleport)
        {
            _exit._canTeleport = false;
            Bullet bullet = other.GetComponent<Bullet>();

            bullet.GetComponent<TrailRenderer>().emitting = false;
            bullet.GetComponent<TrailRenderer>().Clear();
            bullet.gameObject.transform.position = _exit.transform.position;

            Vector3 localEntryDirection = transform.InverseTransformDirection(bullet.direction);
            Vector3 newWorldDirection = _exit.transform.TransformDirection(localEntryDirection);

            bullet.direction = newWorldDirection;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _canTeleport = true;
        other.GetComponent<TrailRenderer>().emitting = true;
    }
}
