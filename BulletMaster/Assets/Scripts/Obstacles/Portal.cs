using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _exit;

    private bool _canTeleport = true;
    private float _delayBeforeTeleport = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (_canTeleport)
        {
            _exit._canTeleport = false;

            other.GetComponent<TrailRenderer>().emitting = false;
            other.GetComponent<TrailRenderer>().Clear();
            other.gameObject.transform.position = _exit.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _canTeleport = true;
        other.GetComponent<TrailRenderer>().emitting = true;
    }
}
