using UnityEngine;

public class BulletEnemy : MonoBehaviour
{

	[SerializeField] private float speed;

	[SerializeField] private int damage;

	private void Update()
	{
		transform.Translate(Time.deltaTime * speed * -Vector2.left);
		Destroy(gameObject,8f);
	}

	private void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{	
		other.gameObject.GetComponent<CombatPlayerV2>().hit(damage, other.GetContact(0).normal);
		Destroy(gameObject);
		}
	}	
}
