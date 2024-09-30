using UnityEngine;

public class movement : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] private Vector2 direction;
    private Rigidbody2D rb2D;
    private float xMovement;
    private float yMovement;
    private Animator animator;
    private bool spin;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xMovement = Input.GetAxisRaw("Horizontal");
        yMovement =  Input.GetAxisRaw("Vertical");
        animator.SetFloat("xMovement", xMovement);
        animator.SetFloat("yMovement", yMovement);
        direction = new Vector2(xMovement, yMovement).normalized;
        if(xMovement < 0 && !spin)
        {
            Spin();
        }
        else if (xMovement > 0 && spin)
        {
            Spin();
        }

        if (xMovement!=0 || yMovement !=0)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run",false);
        }
    }

private void Spin()
{
    spin = !spin;
    Vector3 scale = transform.localScale;
    scale.x *= -1;
    transform.localScale=scale;
}
    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + direction * movementSpeed * Time.fixedDeltaTime);
    }
}
