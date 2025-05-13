using System.Collections.Generic;
using UnityEngine;

public class SpeedModifierHandler
{
    private readonly Mover _mover;
    private readonly List<float> _multipliers;
    private readonly List<float> _endTime;

    public SpeedModifierHandler(Mover mover)
    {
        mover.ThrowIfNull();
        _mover = mover;
        _multipliers = new();
        _endTime = new();
    }

    public void Update()
    {
        if (_multipliers.Count == Constants.Zero)
        {
            return;
        }

        for (int i = _multipliers.Count - Constants.One; i >= Constants.Zero; i--)
        {
            if (Time.time >= _endTime[i])
            {
                _multipliers.RemoveAt(i);
                _endTime.RemoveAt(i);

                UpdateSpeed();
            }
        }
    }

    public void ApplyModifier(SpeedModifier speedModifier)
    {
        speedModifier.ThrowIfDefault();

        _multipliers.Add(speedModifier.Multiplier);
        _endTime.Add(Time.time + speedModifier.Duration);

        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        float totalMultiplier = Constants.One;

        if (_multipliers.Count == Constants.Zero)
        {
            _mover.ResetSpeedMultiplier();

            return;
        }

        foreach (float multiplier in _multipliers)
        {
            totalMultiplier *= multiplier;
        }

        _mover.SetSpeedMultiplier(totalMultiplier);
    }

    public void CancelAllSlows()
    {
        _multipliers.Clear();
        _endTime.Clear();

        _mover.ResetSpeedMultiplier();
    }
}
