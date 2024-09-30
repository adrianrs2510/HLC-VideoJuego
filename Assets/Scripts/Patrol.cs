using UnityEngine;

public class Patrol : MonoBehaviour
{
	[SerializeField] private float speed_move;

	[SerializeField] private Transform[] point_move;

	[SerializeField] private float distance;

	private int num_random;
	private int next_move = 0;

	private SpriteRenderer spriteRenderer;

	private Animator animator;



	// Start is called before the first frame update
	void Start()
	{
		num_random = Random.Range(0, point_move.Length);
		spriteRenderer = GetComponent<SpriteRenderer>();
		Spin();	
		animator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		if(animator.GetFloat("distance_player") > 2.3)
		{
		transform.position = Vector2.MoveTowards(transform.position, point_move[num_random].position, speed_move * Time.deltaTime);
		}
		animator.SetFloat("Run", 1);
		if(Vector2.Distance(transform.position, point_move[num_random].position) <  distance)
		{
			num_random = Random.Range(0, point_move.Length);
			next_move +=1;
			if(next_move >= point_move.Length)
			{
				next_move = 0;
			}
			Spin();
		}

	}
	private void Spin()
	{
		if(transform.position.x < point_move[next_move].position.x)
		{
			transform.rotation = Quaternion.Euler(0,180,0);
		}
		else
		{
			transform.rotation = Quaternion.Euler(0,0,0);
		}
	}
}
