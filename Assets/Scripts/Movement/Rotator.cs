using UnityEngine;

public class Rotator
{
    private readonly Rigidbody _rigidbody;
    private readonly float _degreesPerSecond;

    public Rotator(Rigidbody rigidbody, float rotationSpeed = 200f)
    {
        rigidbody.ThrowIfNull();
        rotationSpeed.ThrowIfZeroOrLess();

        _rigidbody = rigidbody;
        _degreesPerSecond = rotationSpeed;
    }

    public void Rotate(Vector3 direction)
    {
        direction.ThrowIfNotNormalize();
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion rotation = Quaternion.RotateTowards(_rigidbody.rotation, targetRotation, _degreesPerSecond * Time.fixedDeltaTime);

        _rigidbody.MoveRotation(rotation);
    }
}
