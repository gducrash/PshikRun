using UnityEngine;

public class AnimBlendTreePickRandom : StateMachineBehaviour
{

    [SerializeField]
    private string parameterName = "";
    [SerializeField]
    private int from = 0;
    [SerializeField]
    private float to = 3;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int choice = Mathf.FloorToInt(Random.Range(from, to));
        animator.SetFloat(parameterName, choice);
    }
}
