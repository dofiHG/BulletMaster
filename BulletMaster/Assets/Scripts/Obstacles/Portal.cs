using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _exit;
    [SerializeField] private Vector3 _newDirection;

    private bool _canTeleport = true;
    private float _delayBeforeTeleport = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (_canTeleport)
        {
            _exit._canTeleport = false;

            other.GetComponent<TrailRenderer>().enabled = false;
            other.gameObject.transform.position = _exit.transform.position;
            other.gameObject.gameObject.GetComponent<Bullet>().enabled = true;
            other.gameObject.gameObject.GetComponent<Bullet>().direction = _newDirection;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _canTeleport = true;

    }
}
