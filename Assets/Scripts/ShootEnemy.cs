using UnityEngine;

public class ShootEnemy : MonoBehaviour
{
	[SerializeField] private Transform controller_Shoot;

	[SerializeField] private float distance_line;

	[SerializeField] private LayerMask player;

	[SerializeField] private bool player_range;

	[SerializeField] private float time_shoot;

	[SerializeField] private float time_last_shoot;

	[SerializeField] private GameObject bullet_Enemy;

	[SerializeField] private float time_wait_shoot;

	[SerializeField] private Animator animator;

	[SerializeField] private Transform playerGame;

	private bool see_right;


	// Update is called once per frame
	void Update()
	{
		Shoot();
		Spin(playerGame.position);
	}
	public void Spin(Vector3 objetive)
    {
    	if ((objetive.x > transform.position.x && !see_right) || (objetive.x < transform.position.x && see_right))
		{
			see_right = !see_right;
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
		}
    }
	private void Shoot()
	{
		player_range = Physics2D.Raycast(controller_Shoot.position, transform.right,distance_line,player);

		if(player_range)
		{
			if(Time.time > time_shoot + time_last_shoot)
			{
				time_last_shoot = Time.time;
				animator.SetTrigger(nameof(Shoot));
				Invoke(nameof(SpawnBullet), time_wait_shoot);
			}
		}
	}

	private void SpawnBullet()
	{
		Instantiate(bullet_Enemy, controller_Shoot.position, controller_Shoot.rotation);
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(controller_Shoot.position, controller_Shoot.position + transform.right * distance_line);
	}
}
