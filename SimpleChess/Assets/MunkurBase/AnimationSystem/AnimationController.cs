using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using EasyButtons;
using System;

namespace Munkur
{
    [RequireComponent(typeof(Animator))]
    public abstract class AnimationController<EAnimationType> : MonoBehaviour
    {
        [Serializable]
        public class AnimationTypeNamePair
        {
            public EAnimationType AnimationType;
            public String AnimationName;

            public AnimationTypeNamePair(EAnimationType animationType, String animationName)
            {
                AnimationType = animationType;
                AnimationName = animationName;
            }
        }
        
        [SerializeField] private Animator animator;

        [SerializeField] private EAnimationType currentAnimationType;

        public EAnimationType CurrentAnimationType => currentAnimationType;

        [SerializeField] private List<AnimationTypeNamePair> animationTypeNameList;
        
        private Dictionary<EAnimationType, String> animationTypeNameDictionary;

        protected virtual void Awake()
        {
            animationTypeNameDictionary = new Dictionary<EAnimationType, string>();
            
            foreach (var animationTypeNamePair in animationTypeNameList)
            {
                animationTypeNameDictionary.Add(animationTypeNamePair.AnimationType, animationTypeNamePair.AnimationName);
            }
        }

        public void PlayAnimation(EAnimationType animationType, int layer = 0, 
            Action OnAnimationFinished = null)
        {
            StopAllCoroutines();
            
            if (currentAnimationType.Equals(animationType))
            {
                animator.Rebind();
                animator.Play(animationTypeNameDictionary[currentAnimationType], layer);
            }
            else
            {
                currentAnimationType = animationType;
                animator.Play(animationTypeNameDictionary[animationType], layer);
            }

            StartCoroutine(CheckUntilAnimationFinish(animationTypeNameDictionary[animationType], layer,
                OnAnimationFinished));
        }

        private IEnumerator CheckUntilAnimationFinish(String animationName, int layerIndex, Action OnAnimationFinished)
        {
            while (!animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animationName) ||
                   animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime < 1.0f)
            {
                yield return null;
            }
            
            OnAnimationFinished?.Invoke();
        }

    #if UNITY_EDITOR

        [Button]
        protected void SetProperAnimations()
        {
            if (animationTypeNameList != null)
            {
                animationTypeNameList.Clear();
            }

            animator = GetComponent<Animator>();

            AnimationClip[] animationClips = animator.runtimeAnimatorController.animationClips;

            animationTypeNameList = new List<AnimationTypeNamePair>();

            Array animationTypeArray = Enum.GetValues(typeof(EAnimationType));

            foreach (var animationClip in animationClips)
            {
                foreach (var animationType in animationTypeArray)
                {
                    if (animationClip.name.Contains(animationType.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        animationTypeNameList.Add(new AnimationTypeNamePair((EAnimationType)animationType, animationClip.name));
                        break;
                    }
                }
            }
            
            HMDebug.Log("Animations Initialized");
        }
        
    #endif
        
    }
}