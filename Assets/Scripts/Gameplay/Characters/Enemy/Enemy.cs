using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IPollable
{
    [SerializeField] private float _damage = 1;
    [SerializeField] private float _moveSpeed = 5;

    private Transform _transform;
    private Transform _moveTarget;
    private Mover _mover;
    private Health _health;
    private SpeedModifierHandler _speedModifierHandler;

    public float Damage => _damage;

    private void OnValidate()
    {
        _damage.ThrowIfZeroOrLess();
        _moveSpeed.ThrowIfZeroOrLess();
    }

    private void Awake()
    {
        _transform = transform;
        _health = new(100);
        _mover = new(GetComponent<Rigidbody>(), _moveSpeed);
        _health.Died += Stop;//тест
        _speedModifierHandler = new(_mover);
    }

    private void Stop()//тест
    {
        _moveTarget = null;
    }

    private void Update()
    {
        _speedModifierHandler.Update();
    }

    private void FixedUpdate()
    {
        if (_moveTarget == null)
        {
            return;
        }

        _mover.Move(CalculateDirection());
    }

    public void SetPosition(Vector3 position)
    {
        position.ThrowIfNull();
        _transform.position = position;
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Follow(Transform target)
    {
        target.ThrowIfNull();
        _moveTarget = target;
    }

    private Vector3 CalculateDirection()
    {
        if (_moveTarget == null)
            throw new InvalidOperationException();

        Vector3 direction = _moveTarget.position - _transform.position;

        return direction.normalized;
    }

    public void TakeDamage(float damage)
    {
        damage.ThrowIfZeroOrLess();
        _health.TakeDamage(damage);
    }

    public void ApplySlow(SpeedModifier speedModifier)
    {
        speedModifier.ThrowIfDefault();
        _speedModifierHandler.ApplyModifier(speedModifier);
    }
}
