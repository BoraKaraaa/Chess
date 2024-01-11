using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Object = UnityEngine.Object;

public static class ExtensionBase
{
    public static void ExecuteTasksOnce(this Transform taskHolder)
    {
        IExecuteable[] executeableTasks = taskHolder.GetComponents<IExecuteable>();
        
        foreach (var task in executeableTasks)
        {
            task.Run();
        }
    }

    public static void PlayAndCheckAnimationFinish(this MonoBehaviour monoBehaviour, Animator animator, String animationName,
        Action OnAnimationFinished)
    {
        animator.Rebind();
        animator.Play(animationName);
        monoBehaviour.StartCoroutine(CheckUntilAnimationFinish(animator, animationName, 0, OnAnimationFinished));
    }
    
    private static IEnumerator CheckUntilAnimationFinish(Animator animator, String animationName, int layerIndex, Action OnAnimationFinished)
    {
        while (!animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animationName) ||
               animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1.0f)
        {
            yield return null;
        }

        OnAnimationFinished?.Invoke();
    }
    
    // Destory related //
    public static void DestroyBefore(this Transform transform, Transform other)
    {
        Object.Destroy(transform);
        Object.Destroy(other);
    }
    
    public static void DestroyAfter(this Transform transform, Transform other)
    {
        Object.Destroy(other);
        Object.Destroy(transform);
    }

    // Tween related //
    public static void DORotateX(this Transform transform, float endValue, float duration)
    {
        Vector3 endVector = transform.position;
        endVector.SetX(endValue);

        transform.DORotate(endVector, duration);
    }

    public static void DORotateY(this Transform transform, float endValue, float duration)
    {
        Vector3 endVector = transform.position;
        endVector.SetY(endValue);

        transform.DORotate(endVector, duration);
    }

    public static void DORotateZ(this Transform transform, float endValue, float duration)
    {
        Vector3 endVector = transform.position;
        endVector.SetZ(endValue);

        transform.DORotate(endVector, duration);
    }
}
