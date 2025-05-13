using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordStrikeStats", menuName = "Game/SwordStrikeStats")]
public class SwordStrikeStats : ScriptableObject, IAbilityStats
{
    [Header("Level")]
    [SerializeField][Min(Constants.One)] private int _level = 1;

    [Header("Damage")]
    [SerializeField][Min(Constants.One)] private float _damage = 50f;

    [Header("Cooldown")]
    [SerializeField][Min(Constants.Epsilon)] private float _cooldown = 2f;

    [Header("Area Settings")]
    [SerializeField][Min(Constants.One)] private float _radius = 4f;
    [SerializeField][Min(Constants.One)] private float _angle = 50f;

    [Header("Slow Settings")]
    [SerializeField] private bool _isSlowEnable = false;
    [SerializeField][Min(Constants.Zero)] private float _slowDuration = 0f;
    [SerializeField][Min(Constants.Zero)] private float _slowMultiplier = 0f;

    private float _angleCos;
    private SpeedModifier _speedModifier;

    public int Level => _level;
    public float Damage => _damage;
    public float Cooldown => _cooldown;
    public float Radius => _radius;
    public float Angle => _angle;
    public float AngleCos => _angleCos;
    public bool IsSlowEnable => _isSlowEnable;
    public SpeedModifier SpeedModifier => _speedModifier;

    private void OnEnable()
    {
        _angleCos = Mathf.Cos(_angle * Mathf.Deg2Rad);

        if (_isSlowEnable == false)
        {
            return;
        }

        if (_slowMultiplier == Constants.Zero || _slowDuration == Constants.Zero)
        {
            throw new ArgumentOutOfRangeException();
        }

        _speedModifier = new(_slowMultiplier, _slowDuration);
    }
}
