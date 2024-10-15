using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
	[SerializeField] private Animator animator;

	private Rigidbody2D rb2d;

	private Transform player;

	private bool see_right;

	[Header("Health")]
	[SerializeField] private float health;

	//[SerializeField] private barra de vida

	[SerializeField] private Transform controller_attack;

	[SerializeField] private float radius_attack;

	[SerializeField] private int damage_attack = 15;



	// Start is called before the first frame update
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		//barradevida.InicializarBarradevida(health);
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

	}
	// Update is called once per frame
	void Update()
	{
		

		float distance_player = Vector2.Distance(transform.position, player.position);
		animator.SetFloat("distance_player", distance_player);
		if (distance_player > 2.3)
		{
			animator.SetFloat("Run",1);
		}
		else
		{
			animator.SetFloat("Run",0);
		}
	}

	public void Take_damage(int damage)
	{
		health -= damage;

		//barradevida.Cambiarvidaactual(health);

		if(health <= 0)
		{
			animator.Play("Death",0);
		}
	}

	private void Death()
	{
		Destroy(gameObject);
	}

	public void See_player()
	{
		if((player.position.x > transform.position.x && !see_right) || (player.position.x < transform.position.x && see_right))
		{
			see_right = !see_right;
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
		}
	}
	public void attack()
	{
		Collider2D[] objects = Physics2D.OverlapCircleAll(controller_attack.position, radius_attack);

        foreach (Collider2D collider in objects)
        {
            if(collider.CompareTag("Player"))
            {
                collider.transform.GetComponent<CombatPlayerV2>().hit(damage_attack,new Vector2(0,0));
            }
        }

	}
	public Rigidbody2D GetEnemyAttack()
	{
		return rb2d;
	}

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller_attack.position, radius_attack);
    }	
	
}
