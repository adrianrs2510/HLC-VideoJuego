using UnityEngine;

public class MoveV2 : MonoBehaviour
{
	private Transform position;
	private const int MAX_STAMINA = 1000;

	private const int cost_stamine = 250;

    [SerializeField] private Rigidbody2D rb;

	private Vector2 input;

	[Header("Move")]

	private float horizontal_move = 0f;

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
	[SerializeField]private Animator animator;

	[Header("Climb")]

	[SerializeField] private float speed_Climb;
	[SerializeField] private PolygonCollider2D polygonCollider2D;
	private float gravity;
	private bool climbing;

	[Header("Experimental")]

	[SerializeField] private int currentStamine;

	[SerializeField] private StamineBar stamineBar;
	
	[SerializeField] private float	_rollForce = 6.0f;
	private bool _grounded = false;
	private bool _rolling = false;
	private int _facingDirection = 1;
	private int _currentAttack = 0;
	private float _timeSinceAttack = 1.0f;
	private float _delayToIdle = 0.0f;
	private float _rollDuration = 8.0f / 14.0f;
	private float _rollCurrentTime;

	[Header("Damage")]
	private bool canMove = true;

	[SerializeField] private Vector2 speed_rebound;

	[SerializeField] private CombatPlayerV2 combatPlayerV2;

	[SerializeField] private GameDataControlerV2 gamedatacontroler;

	[Header("Sound")]

	[SerializeField] private AudioSource audioSource;

	[SerializeField] private AudioClip jumpSound;
	[SerializeField] private AudioClip rollSound;


	private void Start()
	{
		currentStamine = MAX_STAMINA;
		stamineBar.SetMaxStamine(MAX_STAMINA);
		gravity = rb.gravityScale;
	}

	private void Awake()
	{
		if (LoadState.load)
		{
			gamedatacontroler.LoadData();
		}
		else
		{
			rb.transform.position = new Vector3(-107f,-4.6f,0);
			gamedatacontroler.SaveData(); 
		}
	}
	private void Update()
	{
		
		if ( combatPlayerV2.GetCurrentHealth() > 0)
		{

			input.x = Input.GetAxisRaw("Horizontal");
			input.y = Input.GetAxisRaw("Vertical");

			horizontal_move = input.x * move_speed;

			if(currentStamine < MAX_STAMINA)
			{
				currentStamine +=1;
				stamineBar.SetStamine(currentStamine);
			}

			if(Input.GetButtonDown("Jump"))
			{
				audioSource.PlayOneShot(jumpSound);
				jump = true;
			}

			//Check if character just landed on the ground
			if (!_grounded && in_ground)
			{
				_grounded = true;
				animator.SetBool("Grounded", true);
			}

			//Check if character just started falling
			if (_grounded)
			{
				_grounded = false;
				animator.SetBool("Grounded", true);
			}
			

			// Increase timer that controls attack combo
			_timeSinceAttack += Time.deltaTime;

			// Increase timer that checks roll duration
			if(_rolling)
			{
				_rollCurrentTime += Time.deltaTime;
			}
			// Disable rolling if timer extends duration
			if(_rollCurrentTime > _rollDuration)
			{
				_rolling = false;
			}
			//Attack
			else if(Input.GetMouseButtonDown(0) && _timeSinceAttack > 0.25f && !_rolling)
			{
				_currentAttack++;

			// Loop back to one after third attack
				if (_currentAttack > 3)
					_currentAttack = 1;

				// Reset Attack combo if time since last attack is too large
				if (_timeSinceAttack > 1.0f)
					_currentAttack = 1;

				// Call one of three attack animations "Attack1", "Attack2", "Attack3"
				animator.SetTrigger("Attack" + _currentAttack);

				// Reset timer
				_timeSinceAttack = 0.0f;
			}
			
			// Roll
			else if (Input.GetKeyDown("q") && !_rolling)
			{
				if(currentStamine > cost_stamine)
				{
				_rolling = true;
				animator.SetTrigger("Roll");
				audioSource.PlayOneShot(rollSound);
				rb.velocity = new Vector2(_facingDirection * _rollForce, rb.velocity.y);
				_rolling = false;
				}
				if(currentStamine >= cost_stamine)
				{
				currentStamine -= cost_stamine;
				stamineBar.SetStamine(currentStamine);
				}
			}

			//Run
			if(horizontal_move > 0 || -horizontal_move > 0)
			{
				_delayToIdle = 0.05f;
				animator.SetInteger("AnimState", 1);
			}
			else if (-horizontal_move == 0)
			{
				_delayToIdle -= Time.deltaTime;
				if(_delayToIdle < 0)
				{
					animator.SetInteger("AnimState", 0);
				}
			}
			//Set AirSpeed in animator
			animator.SetFloat("AirSpeedY", rb.velocity.y);

			Climb();
		}
	}
	private void FixedUpdate()
	{
		in_ground = Physics2D.OverlapBox(controllerGround.position, dimensions_box, 0f, groundMask);
		//Mover
		if(canMove)
		{
			Motion(horizontal_move * Time.fixedDeltaTime, jump);
		}
		if (!in_ground)
		{
			animator.SetBool("Grounded", false);
		}
		
		jump = false;
	}

/*
	public void OnTriggerStay2D(Collider2D other)
	{
		if (other.TryGetComponent(out IInteractable interactable))
		{
			interactable.Interact();
		}
	}
	*/

	private void Motion(float move, bool jump) 
	{
		Vector3 speed_objetive = new Vector2(move, rb.velocity.y);
		rb.velocity = Vector3.SmoothDamp(rb.velocity, speed_objetive, ref speed, smoothed_speed);

		if(move < 0 && !spin)
		{
			///Spin
			Spin();
			_facingDirection = -1;
		}
		else if (move > 0 && spin)
		{
			//spin
			Spin();
			_facingDirection = 1;
		}
		if(in_ground && jump && !_rolling)
		{
			animator.Play("Jump");
			_grounded = false;
			animator.SetBool("Grounded", _grounded);
			rb.velocity = new Vector2(rb.velocity.x, jump_streght);
			in_ground = false;
		}
	}


	public void Rebound(Vector2 pointHit)
	{
		rb.velocity = new Vector2(-speed_rebound.x * pointHit.x, speed_rebound.y);
	}
	private void Spin()
	{
		spin = !spin;
		//Vector3 scale = transform.localScale;
		//scale.x *= -1;
		//transform.localScale = scale;
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(controllerGround.position, dimensions_box);
	}

	public bool GetCanMove()
	{
		return canMove;
	}
	public void SetCanMove(bool move)
	{
		canMove = move;
	}
	private void Climb()
	{
		if ((input.y != 0 || climbing) && (polygonCollider2D.IsTouchingLayers(LayerMask.GetMask("Stair"))))
		{
			Vector2 climbingSpeed = new Vector2(rb.velocity.x, input.y * speed_Climb);
			rb.velocity = climbingSpeed;
			rb.gravityScale = 0;
			climbing = true;
		}
		else
		{
			rb.gravityScale = gravity;
			climbing = false;
		}
		if (in_ground)
		{
			climbing = false;
		}
	}
}
