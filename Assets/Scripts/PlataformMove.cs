using UnityEngine;

public class PlataformMove : MonoBehaviour
{
	[SerializeField] private bool move = false;
	[SerializeField] private Transform[] point_moving;

	[SerializeField] private float speed_moving;

	private int next_plataform = 1;

	private bool order_plataform = true;

	private bool moving = false;

	private Rigidbody2D rigi2D;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (order_plataform && next_plataform + 1 >= point_moving.Length)
		{
			order_plataform = false;
		}
		if (!order_plataform && next_plataform <= 0)
		{
			order_plataform = true;
		}

		if(Vector2.Distance(transform.position, point_moving[next_plataform].position) < 0.1f)
		{
			if (order_plataform)
			{
				next_plataform += 1;
			}
			else
			{
				next_plataform -= 1;
			}
		}
		if (moving)
		transform.position  = Vector2.MoveTowards(transform.position, point_moving[next_plataform].position, speed_moving * Time.deltaTime);
	}
	
	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			moving = true;
			other.transform.SetParent(this.transform);
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
		{
			other.transform.SetParent(null);
		}
	}
}
