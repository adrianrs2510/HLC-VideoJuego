using System.Collections;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
	[SerializeField] private float health;

	private Animator animator;

	private Rigidbody2D rigidbody2D;
	private MoveV2 move;

	private bool Death;

	[SerializeField] private float time_loseControl = 0.5f;	

	[SerializeField] private HealthPotion healthPotion;


	[Header("Sound")]
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip hitSound;


	private void Start()
	{
		move = GetComponent<MoveV2>();
		animator = GetComponent<Animator>();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	public void hit(float damage)
	{
			health -= damage;
			if(health > 0)
			{
				animator.SetTrigger("Hurt");
				StartCoroutine(LoseControl());
			}
			else
			{
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemies"),true);
				animator.SetTrigger("Death");
				healthPotion.DeadEnemy();
			}
	}
	private IEnumerator LoseControl()
	{
		move.SetCanMove(false);
		yield return new WaitForSeconds(time_loseControl);
		move.SetCanMove(true);
	}
	public void take_hit(float damage)
	{
	health -= damage;
	if(health > 0)
	{
		audioSource.PlayOneShot(hitSound);
		animator.SetTrigger("Hurt");
	}
	else
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemies"),true);
		rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
		animator.SetTrigger("Death");
		healthPotion.DeadEnemy();
	}
	}

	public float GetHealthEnemy()
	{
		return health;
	}
	public void Destroy()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Enemies"),false);
		Destroy(gameObject);
	}
}
