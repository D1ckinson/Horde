using System;
using UnityEngine;

public class SwordStrike : IAbility
{
    private const int MaxCountForStrike = 50;

    private readonly Transform _transform;
    private readonly SwordStrikeStatsContainer _statContainer;
    private readonly Collider[] _colliders;

    private SwordStrikeStats _stats;
    private float _cooldownTimer;

    public event Action Hitting;

    public SwordStrike(Transform transform, SwordStrikeStatsContainer statContainer, int level = Constants.Zero)
    {
        transform.ThrowIfNull();
        statContainer.ThrowIfNull();
        level.ThrowIfNegative();

        _transform = transform;
        _statContainer = statContainer;
        _colliders = new Collider[MaxCountForStrike];
        _stats = _statContainer.GetStats(level);

        _cooldownTimer = _stats.Cooldown;
    }

    public float Angle => _stats.Angle;

    public void Update(float time)
    {
        _cooldownTimer -= time;

        if (_cooldownTimer <= Constants.Zero)
        {
            Strike();
            Hitting?.Invoke();
        }
    }

    private void Strike()
    {
        _cooldownTimer = _stats.Cooldown;

        Vector3 position = _transform.position;
        Vector3 forward = _transform.forward;

        int enemyCount = Physics.OverlapSphereNonAlloc(position, _stats.Radius, _colliders);

        for (int i = Constants.Zero; i < enemyCount; i++)
        {
            if (_colliders[i].TryGetComponent(out Enemy enemy) == false)
            {
                continue;
            }

            Vector3 direction = (enemy.transform.position - position).normalized;

            if (Vector3.Dot(forward, direction) < _stats.AngleCos)
            {
                continue;
            }

            enemy.TakeDamage(_stats.Damage);

            if (_stats.IsSlowEnable)
            {
                enemy.ApplySlow(_stats.SpeedModifier);
            }
        }
    }

    public void LevelUp()
    {
        _stats = _statContainer.GetStats(_stats.Level + Constants.One);
    }

#if UNITY_EDITOR
    public void DrawAttackAreaGizmos()
    {
        CustomGizmos.DrawCone(_transform.position, _transform.forward, _stats.Radius, _stats.Angle);
    }
#endif
}
