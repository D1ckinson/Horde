using System;

public class Health
{
    public Health(float value)
    {
        value.ThrowIfZeroOrLess();
        Value = value;
    }

    public event Action GetHit;
    public event Action Died;

    public float Value { get; private set; }

    public void TakeDamage(float damage)
    {
        damage.ThrowIfZeroOrLess();

        float tempValue = Value - damage;
        GetHit?.Invoke();

        if (tempValue <= Constants.Zero)
        {
            Value = Constants.Zero;
            Died?.Invoke();
        }
        else
        {
            Value = tempValue;
        }
    }
}
