using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Hero : MonoBehaviour
{
    [SerializeField][Min(Constants.One)] private float _healthValue = 100;
    [SerializeField][Min(Constants.Zero)] private float _invincibilityDuration = 0.6f;
    [SerializeField] private SwordStrikeStatsContainer _swordStrikeStatsContainer;
    [SerializeField] private ParticleSystem _swordSwingEffect;
    [SerializeField] Renderer _renderer;

    private Health _health;
    private Mover _mover;
    private Rotator _rotator;
    private float _invincibleTimer = 0f;
    private InputControls _inputActions;
    private Vector3 _moveDirection = default;
    private EntityAnimator<HeroAnimation> _animator;
    private SwordStrike _swordStrike;
    private Transform _transform;

    private bool IsMoving => _inputActions.Player.Move.inProgress;
    private bool IsInvincible => _invincibleTimer > Constants.Zero;

    private void OnValidate()
    {
        //_swordStrikeStatsContainer.ThrowIfNull();
        //_swordSwingEffect.ThrowIfNull();
        _renderer.ThrowIfNull();
    }

    private void Awake()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Animator animator = GetComponent<Animator>();

        _health = new(_healthValue);
        _transform = GetComponent<Transform>();
        _mover = new(rigidbody);
        _rotator = new(rigidbody);
        _inputActions = new();
        _animator = new(animator, _renderer.material);
        _health.GetHit += _animator.VisualizeHit;

        _swordStrike = new(transform, _swordStrikeStatsContainer);
        _swordStrike.Hitting += PlayAttackAnimation;

        ParticleSystem.ShapeModule a = _swordSwingEffect.shape;
        a.radius = _swordStrike.Radius;
    }

    private void PlayAttackAnimation()
    {
        _animator.Play(HeroAnimation.Attack);

        VisualizeSwing();
    }

    private void VisualizeSwing()
    {
        float rotationTime = 0.15f;
        float halfAngle = _swordStrike.Angle / 2f;

        Quaternion startRotation = _transform.rotation * Quaternion.Euler(0, halfAngle, 0);
        Quaternion endRotation = _transform.rotation * Quaternion.Euler(0, -halfAngle, 0);

        _swordSwingEffect.transform.SetPositionAndRotation(_transform.position, startRotation);
        _swordSwingEffect.Play();

        _swordSwingEffect.transform
            .DORotateQuaternion(endRotation, rotationTime)
            .OnComplete(() => _swordSwingEffect.Stop());
    }


    private void Update()
    {
        float deltaTime = Time.deltaTime;
        _swordStrike.Update(deltaTime);

        if (IsInvincible)
        {
            _invincibleTimer -= deltaTime;
        }

        if (IsMoving)
        {
            ReadInput();
        }
    }

    private void ReadInput()
    {
        Vector2 moveDirection = _inputActions.Player.Move.ReadValue<Vector2>();

        _moveDirection.x = moveDirection.x;
        _moveDirection.z = moveDirection.y;
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            _mover.Move(_moveDirection);
            _rotator.Rotate(_moveDirection);
            _animator.Play(HeroAnimation.Run);
        }
        else
        {
            _animator.Play(HeroAnimation.Idle);
        }
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (_swordStrike == null)
        {
            return;
        }

        _swordStrike.DrawAttackAreaGizmos();
    }
#endif

    private void HandleCollision(Collision collision)
    {
        collision.ThrowIfNull();

        if (collision.gameObject.TryGetComponent(out Enemy enemy) == false)
        {
            return;
        }

        if (IsInvincible)
        {
            return;
        }

        _invincibleTimer = _invincibilityDuration;
        _health.TakeDamage(enemy.Damage);
    }
}