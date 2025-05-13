using UnityEngine;

public class Mover
{
    private readonly float _speed;
    private readonly Rigidbody _rigidbody;

    private float _multiplier;

    public Mover(Rigidbody rigidbody, float speed = 5)// убрать 5
    {
        rigidbody.ThrowIfNull();
        speed.ThrowIfZeroOrLess();

        _rigidbody = rigidbody;
        _speed = speed;
        ResetSpeedMultiplier();
    }

    public void Move(Vector3 direction)
    {
        direction.ThrowIfNotNormalize();

        Vector3 position = _rigidbody.position + direction * (_speed * _multiplier * Time.fixedDeltaTime);
        _rigidbody.MovePosition(position);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        multiplier.ThrowIfZeroOrLess();
        _multiplier = multiplier;
    }

    public void ResetSpeedMultiplier()
    {
        _multiplier = Constants.One;
    }
}
