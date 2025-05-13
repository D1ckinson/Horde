using System;
using UnityEngine;

public static class ThrowHelper
{
    public static void ThrowIfNull<T>(this T argument)
    {
        if (argument == null)
        {
            throw new ArgumentNullException();
        }
    }

    public static void ThrowIfNegative(this int argument)
    {
        if (argument < Constants.Zero)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public static void ThrowIfNegative(this float argument)
    {
        if (argument < Constants.Zero)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public static void ThrowIfZeroOrLess(this float argument)
    {
        if (argument <= Constants.Zero)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public static void ThrowIfZeroOrLess(this int argument)
    {
        if (argument <= Constants.Zero)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public static void ThrowIfNotNormalize(this Vector3 argument)
    {
        if (Mathf.Approximately(argument.sqrMagnitude, Constants.One) == false)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public static void ThrowIfMoreThan(this int argument, int value)
    {
        if (argument > value)
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public static void ThrowIfDefault<T>(this T argument) where T : struct
    {
        if (argument.Equals(default(T)))
        {
            throw new ArgumentException();
        }
    }

    public static void ThrowIfAnimationMissing(this Animator animator, int hash, int layerIndex = Constants.Zero)
    {
        if (animator.HasState(layerIndex, hash) == false)
        {
            throw new MissingAnimationException();
        }
    }
}
