using UnityEngine;

public class hit : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnCollisionEnter2D (Collision2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<CombatPlayerV2>().hit(damage, other.GetContact(0).normal);
		}
	}	
}
