using System.Collections.Generic;
using UnityEngine;
using System;

namespace Munkur
{
    public class TransitionRefereneces : Singleton<TransitionRefereneces>
    {
        public List<ESceneTransitionToTransition> ESceneTransitionToTransitionList;
    }

    [Serializable]
    public struct ESceneTransitionToTransition
    {
        public ESceneTransition ESceneTransition;
        public Transition Transition;
        public GameObject BlackScreen;
    }
}
