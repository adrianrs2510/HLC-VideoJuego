using UnityEngine;

public class Hability_Boss : MonoBehaviour
{
    [SerializeField] private int damage;

    [SerializeField] private Vector2 dimensions;

    [SerializeField] private Transform position;

    [SerializeField] private float time_health;


    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,time_health);
    }

    public void hit()
    {
        Collider2D[] objects = Physics2D.OverlapBoxAll(position.position, dimensions, 0f);

        foreach(Collider2D collision in objects)
        {
            if (collision.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<CombatPlayerV2>().hit(damage,new Vector2(0,0));
            }
        }
    }
    private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(position.position, dimensions);
	}
}
