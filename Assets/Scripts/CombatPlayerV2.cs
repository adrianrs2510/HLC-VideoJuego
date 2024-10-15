using System.Collections;
using UnityEngine;

public class CombatPlayerV2 : MonoBehaviour
{
    [SerializeField] private int maxHealth = 200;

	[SerializeField] private int currentHealth;

	[SerializeField] private Animator animator;

	[SerializeField] private Rigidbody2D body2D;

	[SerializeField] private MoveV2 move;

	[SerializeField] private float time_loseControl = 0.5f;

	[SerializeField] private HealthBar healthBar;
	private bool die = false;

	[Header("Sound")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip hitSound;


	// Start is called before the first frame update
	private void Start()
	{
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
	}
	// Update is called once per frame
	private void Update()
	{
		if (currentHealth <= 0)
		{
			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemies"),true);
			animator.Play("Death");
		}
	}
	public void hit(int damage, Vector2 position)
	{
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);
		move.Rebound(position);

		if(currentHealth > 0)
		{
			animator.Play("Hurt",0);
			audioSource.PlayOneShot(hitSound);
			StartCoroutine(LoseControl());
		}
		else
		{
			animator.Play("Death");
		}
	}
	private IEnumerator LoseControl()
	{
		move.SetCanMove(false);
		yield return new WaitForSeconds(time_loseControl);
		move.SetCanMove(true);
	}
	
	private void Death()
	{
		animator.SetBool("Death", true);
		die = true;
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemies"),false);
	}
	public bool Isdead()
	{
		return die;
	}
	public void SetDead(bool isdead)
	{
		die = isdead;
	}

	public int GetMaxHealth()
	{
		return maxHealth;
	}
	public int GetCurrentHealth()
	{
		return currentHealth;
	}
	public void SetCurrentHealth(int health)
	{
		currentHealth = health;
	}
	public void Destroy()
	{
		Destroy(gameObject);

	}
	
}

