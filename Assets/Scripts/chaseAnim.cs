using UnityEngine;

public class chaseAnim : StateMachineBehaviour
{
    [SerializeField] private float speed_movement;

    [SerializeField] private float base_time;

    private float chase_time;

    private Transform player;

    private Persecutor persecutor;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chase_time = base_time;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        persecutor = animator.gameObject.GetComponent<Persecutor>();
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, player.position, speed_movement * Time.deltaTime);
        persecutor.Spin(player.position);
        chase_time -= Time.deltaTime;
        if(chase_time <= 0)
        {
            animator.SetTrigger("back");
        }
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
