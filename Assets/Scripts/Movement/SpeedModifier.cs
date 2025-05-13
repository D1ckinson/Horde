public readonly struct SpeedModifier
{
    public float Multiplier { get; }
    public float Duration { get; }

    public SpeedModifier(float multiplier, float duration)
    {
        multiplier.ThrowIfZeroOrLess();
        duration.ThrowIfZeroOrLess();

        Multiplier = multiplier;
        Duration = duration;
    }
}
