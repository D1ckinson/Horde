using UnityEngine;
using DG.Tweening;

public class Bootstrap : MonoBehaviour
{
    private const int TweenersCapacity = 200;
    private const int SequencesCapacity = 50;

    private void Awake()
    {
        DOTween.SetTweensCapacity(TweenersCapacity, SequencesCapacity);
    }
}
