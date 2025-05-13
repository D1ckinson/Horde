public readonly struct AnimationData
{
    public int Hash { get; }
    public int Priority { get; }

    public AnimationData(int hash, int priority)
    {
        priority.ThrowIfNegative();

        Hash = hash;
        Priority = priority;
    }
}
