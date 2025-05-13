using System;
using UnityEngine;

public class Health
{
    public Health(float value)
    {
        value.ThrowIfZeroOrLess();
        Value = value;
    }

    public event Action Died;

    public float Value { get; private set; }

    public void TakeDamage(float damage)
    {
        damage.ThrowIfZeroOrLess();
        float tempValue = Value - damage;

        if (tempValue <= Constants.Zero)
        {
            Value = Constants.Zero;
            Died?.Invoke();
            //Debug.Log("Умер");
        }
        else
        {
            Value = tempValue;
            //Debug.Log("Получил урон");
        }
    }
}
