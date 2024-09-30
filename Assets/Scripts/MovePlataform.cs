using UnityEngine;

public class MovePlataform : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private Transform controller_ground;

    [SerializeField] private float distance;

    [SerializeField] private bool flip;

    [SerializeField] private Rigidbody2D rigidbody2D;

    [SerializeField] private Animator animator;
   
    private void Start()
    {
        
    }

   
    private void FixedUpdate()
    {
     RaycastHit2D ground = Physics2D.Raycast(controller_ground.position, Vector2.down, distance);
     if (animator.GetFloat("distance_player") > 2.3 && animator.GetFloat("Attack") == 0)
     {
        rigidbody2D.velocity = new Vector2(speed,rigidbody2D.velocity.y);
     
     }
     if (ground == false) 
     {
        //Spin
        Spin();
     }   
    }
    private void Spin()
    {
        flip = !flip;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        speed *= -1;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controller_ground.position, controller_ground.transform.position + Vector3.down * distance);
    }
}
