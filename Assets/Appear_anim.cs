using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Appear_anim : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Enemy1>().enabled = true;
        Enemy1.Instance.E_state = Enemy_Fun.EnemyState.chase;
    }
}
