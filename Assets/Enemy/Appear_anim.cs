using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Appear_anim : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.name.Contains("Initiate_E-main"))
        {
        animator.GetComponent<Enemy1>().enabled = true;
        Enemy1.Instance.E_state = Enemy_Fun.EnemyState.chase;
        }
        if (animator.name.Contains("Immolator-main"))
        {
            animator.GetComponent<Immolator>().enabled = true;
            Immolator.Instance.E_state = Enemy_Fun.EnemyState.chase;
        }
    }
}
