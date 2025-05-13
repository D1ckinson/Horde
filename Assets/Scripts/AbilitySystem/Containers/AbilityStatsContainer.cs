using System.Linq;
using UnityEngine;

public class AbilityStatsContainer<T> : ScriptableObject where T : ScriptableObject, IAbilityStats
{
    [Header("Abilities")]
    [SerializeField] private T[] _swordStrikeStats;

    private void OnValidate()
    {
        _swordStrikeStats.ThrowIfNull();
        _swordStrikeStats.Length.ThrowIfZeroOrLess();

        for (int i = 0; i < _swordStrikeStats.Length; i++)
        {
            _swordStrikeStats[i].ThrowIfNull();
        }
    }

    private void Awake()
    {
        _swordStrikeStats = _swordStrikeStats.OrderBy(stat => stat.Level).ToArray();
    }

    public T GetStats(int abilityLevel)
    {
        abilityLevel.ThrowIfNegative();
        abilityLevel.ThrowIfMoreThan(_swordStrikeStats.Length);

        return _swordStrikeStats[abilityLevel];
    }
}
