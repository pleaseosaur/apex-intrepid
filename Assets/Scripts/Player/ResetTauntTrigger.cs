using UnityEngine;

public class ResetTauntTrigger : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Taunting", false);
    }
}