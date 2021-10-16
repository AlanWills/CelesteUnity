using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Celeste.Animation
{
    public class DisableOnExitStateBehaviour : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.gameObject.SetActive(false);
        }
    }
}