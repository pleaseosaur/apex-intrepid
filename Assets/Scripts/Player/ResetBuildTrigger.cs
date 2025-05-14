using UnityEngine;

public class ResetBuildTrigger : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Building", false);
    }
}
