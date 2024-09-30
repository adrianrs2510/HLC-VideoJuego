using UnityEngine;

public class BossAttack : MonoBehaviour
{
	[SerializeField] private Animator animator;

	[SerializeField] private CombatPlayer combatPlayer;

	private Rigidbody2D rb2d;

	private Transform player;

	private bool see_right;

	[Header("Health")]

	[SerializeField] private Transform controller_attack;

	[SerializeField] private float radius_attack;

	[SerializeField] private int damage_attack = 15;


	[Header("HEALTHBAR")]
    //HEALTHBAR
    
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;
	[SerializeField] private GameObject bossHealth;


	[Header("Sound")]
	private bool laughPlayed = false;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip laughSound;
	[SerializeField] private AudioClip hitSound;


	// Start is called before the first frame update
	void Start()
	{
		rb2d = GetComponent<Rigidbody2D>();
		
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //HEALTHBAR
        currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);

	}
	// Update is called once per frame
	void Update()
	{
		HealthBar();
		
		animator.SetInteger("Hability",Random.Range(0,1000));

		float distance_player = Vector2.Distance(new Vector2(transform.position.x+1,transform.position.y), player.position);
		animator.SetFloat("distance_player", distance_player);
		if (distance_player > 2.3)
		{
			animator.SetFloat("Run",1);
		}
		else
		{
			animator.SetFloat("Run",0);
		}
		if (transform.rotation.y == player.transform.rotation.y)
		{
			animator.SetBool("Attack",true);
		}
		else
		{
			animator.SetBool("Attack",false);
		}
	}
	
	public void Take_damage(int damage)
	{
		audioSource.PlayOneShot(hitSound);
		currentHealth -= damage;
        //HEALTHBAR
		//barradevida.Cambiarvidaactual(health);

		if(currentHealth <= 0)
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

	public void HealthBar()
	{
		currentHealth = (int)combatPlayer.GetHealthEnemy();
		healthBar.SetHealth(currentHealth);
		if (animator.GetFloat("Distance") <=20)
		{
			if (!laughPlayed)
			{
				audioSource.PlayOneShot(laughSound);
				laughPlayed=true;
			}
			bossHealth.SetActive(true);
		}
		else if (animator.GetFloat("Distance") >38)
		{
			bossHealth.SetActive(false);
			laughPlayed = false;
		}
	}

	public Transform GetPlayer()
	{
		return player;
	}

	private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller_attack.position, radius_attack);
    }	
	
}
