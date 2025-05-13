using UnityEngine;

public sealed class Follower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private Transform _transform;

    private void OnValidate()
    {
        _target.ThrowIfNull();
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        _transform.position = _target.position + _offset;
    }
}
