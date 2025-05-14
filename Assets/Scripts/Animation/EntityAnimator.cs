using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EntityAnimator<T> where T : Enum
{
    private const float TransitionDuration = 0.1f;

    private readonly Animator _animator;
    private readonly Sequence _getHitSequence;
    private readonly IReadOnlyDictionary<T, AnimationData> _animationData;

    private AnimationData _currentAnimationInfo;

    public EntityAnimator(Animator animator, Material material)
    {
        animator.ThrowIfNull();
        material.ThrowIfNull();

        _animator = animator;

        _animationData = CreateAnimationData();
        _getHitSequence = CreateHitSequence(material);
    }

    public EntityAnimator(Material material)//тест
    {
        material.ThrowIfNull();
        _getHitSequence = CreateHitSequence(material);
    }

    public void VisualizeHit()
    {
        _getHitSequence.Restart();
    }

    public void Play(T animation)
    {
        animation.ThrowIfNull();

        if (_animationData.TryGetValue(animation, out AnimationData animationInfo) == false)
        {
            throw new InvalidOperationException();
        }

        if (animationInfo.Hash == _currentAnimationInfo.Hash)
        {
            return;
        }

        if (animationInfo.Priority > _currentAnimationInfo.Priority || IsAnimationFinished())
        {
            _animator.CrossFade(animationInfo.Hash, TransitionDuration);
            _currentAnimationInfo = animationInfo;
        }
    }

    private bool IsAnimationFinished()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(Constants.Zero);
        bool isCurrentState = stateInfo.shortNameHash == _currentAnimationInfo.Hash;

        return isCurrentState && (stateInfo.normalizedTime >= Constants.One || stateInfo.loop);
    }

    private IReadOnlyDictionary<T, AnimationData> CreateAnimationData()
    {
        T[] animationsNames = (T[])Enum.GetValues(typeof(T));
        Dictionary<T, AnimationData> animationHashes = new(animationsNames.Length);

        foreach (T name in animationsNames)
        {
            string animationName = name.ToString();
            int hash = Animator.StringToHash(animationName);

            _animator.ThrowIfAnimationMissing(hash);

            int priority = Convert.ToInt32(name);
            AnimationData animationInfo = new(hash, priority);

            animationHashes.Add(name, animationInfo);
        }

        return animationHashes;
    }

    private Sequence CreateHitSequence(Material material)
    {
        Sequence sequence = DOTween.Sequence();
        Color originalColor = material.color;

        sequence
            .Append(material.DOColor(Color.white, 0.1f))
            .Append(material.DOColor(originalColor, 0.3f))
            .SetAutoKill(false)
            .Pause();

        return sequence;
    }
}
