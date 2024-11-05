using UnityEngine;

public class BossRun : StateMachineBehaviour
{
    private Enemy_Attack enemy;

    private Rigidbody2D rigidbody2D;

    [SerializeField] private float speed_move;

   

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy_Attack>();
        rigidbody2D = enemy.GetEnemyAttack();
        if(rigidbody2D.CompareTag("Boss"))
        {
        enemy.See_player();
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         if(rigidbody2D.CompareTag("Boss"))
        {
        rigidbody2D.velocity = new Vector2(speed_move, rigidbody2D.velocity.y) * animator.transform.right;
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         if(rigidbody2D.CompareTag("Boss"))
        {
        rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }
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
