using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundAttack_anim : StateMachineBehaviour
{
    static public bool Hound_anim_falg = false;
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("tlfd");
        Hound.instance.E_state = Hound.EnemyState.chase;
        Hound_anim_falg = true;

    }
}
