using System.Collections;
using UnityEngine;

public class PlataformFall : MonoBehaviour
{
	[SerializeField] private float time_wait;

	private Rigidbody2D rigidbody2D;

	[SerializeField] private float speed_rotation;

	private Animator animator;

	private bool fall = false;

	private Vector3 origin;

	[SerializeField] private GameObject plataform;


	// Start is called before the first frame update
	void Start()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		origin = transform.position;
	}
	void Update()
	{
		if(fall)
		{
			Vector3 s = new Vector3(0,0, -speed_rotation * Time.deltaTime);
			transform.Rotate(s);
		}
	}
	private void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			StartCoroutine(falling(other));
			Invoke(nameof(Regenation), 5f);
			Destroy(gameObject, 6f);
		}
	}
	private IEnumerator falling(Collision2D other)
	{
		//animator.SetTrigger("Desactivate");
		yield return new WaitForSeconds(time_wait);
		fall = true;
		rigidbody2D.constraints = RigidbodyConstraints2D.None;
		rigidbody2D.AddForce(new Vector2(0.1f, 0));
		//rigidbody2D.AddForce(Vector.right * 0.1);
	}
	private void Regenation()
	{
		rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
		Instantiate(plataform, origin, Quaternion.Euler(0,0,0));
	}
}
