using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class EntityAnimator<T> where T : Enum
{
    private const float TransitionDuration = 0.1f;

    private readonly Animator _animator;
    private readonly IReadOnlyDictionary<T, AnimationData> _animationData;

    private AnimationData _currentAnimationInfo;

    public EntityAnimator(Animator animator)
    {
        animator.ThrowIfNull();
        _animator = animator;

        _animationData = CreateAnimationHashes();
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

    private IReadOnlyDictionary<T, AnimationData> CreateAnimationHashes()
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
}
