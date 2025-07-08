using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private float _speed;

    private void Update()
    {
        float progress = Mathf.PingPong(Time.time * _speed, 1f);

        transform.position = Vector3.Lerp(_startPosition, _endPosition, progress);
    }
}
