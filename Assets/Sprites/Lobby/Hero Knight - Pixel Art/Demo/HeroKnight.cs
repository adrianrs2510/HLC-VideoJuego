using System.Runtime.InteropServices;
using UnityEngine;

public class Move : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb;

	private Vector2 input;

	[Header("Move")]

	private float horizontal_move = 0f;
	private float vertical_move = 0f;

	[SerializeField] private float move_speed;

	[Range(0f, 0.3f)]
	[SerializeField]
	private float smoothed_speed;

	[SerializeField] private Vector3 speed = Vector3.zero;

	private bool spin;

	[Header("Jump")]

	[SerializeField] private float jump_streght;

	[SerializeField] private LayerMask groundMask;

	[SerializeField] private Transform controllerGround;

	[SerializeField] private Vector3 dimensions_box;

	[SerializeField] private bool in_ground;

	private bool jump = false;

	[Header("Animation")]
	private Animator animator;

	[Header("Climb")]

	[SerializeField] private float speed_Climb;

	private PolygonCollider2D polygonCollider2D;

	private float gravity_start;

	private bool climbing;

	[Header("Experimental")]

	[SerializeField] float	m_rollForce = 6.0f;
	private bool                m_grounded = false;
	private bool                m_rolling = false;
	private int                 m_facingDirection = 1;
	private int                 m_currentAttack = 0;
	private float               m_timeSinceAttack = 0.0f;
	private float               m_delayToIdle = 0.0f;
	private float               m_rollDuration = 8.0f / 14.0f;
	private float               m_rollCurrentTime;

	[Header("Damage")]
	public bool canMove = true;

	[SerializeField] private Vector2 speed_rebound;
	

	// Update is called once per frame
	void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		polygonCollider2D = GetComponent<PolygonCollider2D>();
		gravity_start = rb.gravityScale;
	}
	void Update()
	{
		input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");

		horizontal_move = input.x * move_speed;
		vertical_move = input.y * move_speed;
		
		if(Input.GetButtonDown("Jump"))
		{
			jump = true;
		}

		//Check if character just landed on the ground
		if (!m_grounded && in_ground)
		{
			m_grounded = true;
			animator.SetBool("Grounded", true);
		}

		//Check if character just started falling
		if (m_grounded)
		{
			m_grounded = false;
			animator.SetBool("Grounded", true);
		}
		

		// Increase timer that controls attack combo
		m_timeSinceAttack += Time.deltaTime;

		// Increase timer that checks roll duration
		if(m_rolling)
		{
			m_rollCurrentTime += Time.deltaTime;
		}
		// Disable rolling if timer extends duration
		if(m_rollCurrentTime > m_rollDuration)
		{
			m_rolling = false;
		}
		//Attack
		else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
		{
			m_currentAttack++;

		// Loop back to one after third attack
			if (m_currentAttack > 3)
				m_currentAttack = 1;

			// Reset Attack combo if time since last attack is too large
			if (m_timeSinceAttack > 1.0f)
				m_currentAttack = 1;

			// Call one of three attack animations "Attack1", "Attack2", "Attack3"
			animator.SetTrigger("Attack" + m_currentAttack);

			// Reset timer
			m_timeSinceAttack = 0.0f;
		}
		
		//Block
		else if (Input.GetMouseButtonDown(1) && !m_rolling)
		{
			animator.SetTrigger("Block");
			animator.SetBool("IdleBlock", true);
		}

		else if (Input.GetMouseButtonUp(1))
			animator.SetBool("IdleBlock", false);

		// Roll
		else if (Input.GetKeyDown("left shift") && !m_rolling)
		{
			m_rolling = true;
			animator.SetTrigger("Roll");
			rb.velocity = new Vector2(m_facingDirection * m_rollForce, rb.velocity.y);
			m_rolling = false;
		}

		//Run
		if(horizontal_move > 0 || -horizontal_move > 0)
		{
			m_delayToIdle = 0.05f;
			animator.SetInteger("AnimState", 1);
		}
		else if (-horizontal_move == 0)
		{
			m_delayToIdle -= Time.deltaTime;
			if(m_delayToIdle < 0)
			{
				animator.SetInteger("AnimState", 0);
			}
		}
		if(vertical_move > 0 || -vertical_move > 0)
		{
			m_delayToIdle = 0.05f;
			animator.SetInteger("AnimState", 1);
		}
		else if (-vertical_move == 0)
		{
			m_delayToIdle -= Time.deltaTime;
			if(m_delayToIdle < 0)
			{
				animator.SetInteger("AnimState", 0);
			}
		}
		//Set AirSpeed in animator
		animator.SetFloat("AirSpeedY", rb.velocity.y);

		//Death
		if (Input.GetKeyDown("e") && !m_rolling)
		{
			animator.SetTrigger("Death");
		}
			
		//Hurt
		else if (Input.GetKeyDown("q") && !m_rolling)
			animator.SetTrigger("Hurt");
	
	}
	private void FixedUpdate()
	{
		in_ground = Physics2D.OverlapBox(controllerGround.position, dimensions_box, 0f, groundMask);
		//Mover
		if(canMove)
		{
			MotionH(horizontal_move * Time.fixedDeltaTime, jump);
		}
		
		Climbing();
	
		jump = false;
	}
	/*private void OnTriggerExit(Collider other)
	{   
		if (other.TryGetComponent(out IClimbeable climbEntity))
		{
			climbEntity.CanClimb = true
		}
		
	}*/

	private void MotionH(float move, bool jump) 
	{
		Vector3 speed_objetive = new Vector2(move, rb.velocity.y);
		rb.velocity = Vector3.SmoothDamp(rb.velocity, speed_objetive, ref speed, smoothed_speed);

		if(move < 0 && !spin)
		{
			///Spin
			Spin();
			m_facingDirection = -1;
		}
		else if (move > 0 && spin)
		{
			//spin
			Spin();
			m_facingDirection = 1;
		}
		if(in_ground && jump && !m_rolling)
		{
			animator.SetTrigger("Jump");
			m_grounded = false;
			animator.SetBool("Grounded", m_grounded);
			rb.velocity = new Vector2(rb.velocity.x, jump_streght);
			in_ground = false;
		}
	}

	private void Climbing()
	{
		if((input.y !=0 || climbing) && (polygonCollider2D.IsTouchingLayers(LayerMask.GetMask("ground"))))
		{
			Vector2 speed_up = new Vector2(rb.velocity.x, input.y * speed_Climb);
			rb.velocity = speed_up;
			rb.gravityScale = 0;
			climbing = true;
		}
		else
		{
			rb.gravityScale = gravity_start;
			climbing = false;
		}

		if(in_ground)
		{
			climbing = false;
		}
	}

	public void Rebound(Vector2 pointHit)
	{
		rb.velocity = new Vector2(-speed_rebound.x * pointHit.x, speed_rebound.y);
	}
	private void Spin()
	{
		spin = !spin;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(controllerGround.position, dimensions_box);
	}
}
