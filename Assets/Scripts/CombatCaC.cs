using UnityEngine;

public class CombatCaC : MonoBehaviour
{
    [SerializeField] private Transform controller_attack;

    [SerializeField] private float radius_attack;

    [SerializeField] private int damage_attack;

    private void Bang()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(controller_attack.position, radius_attack);

        foreach (Collider2D collider in objects)
        {
            if(collider.CompareTag("Enemies"))
            {
                collider.transform.GetComponent<CombatPlayer>().take_hit(damage_attack);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller_attack.position, radius_attack);
    }
}
