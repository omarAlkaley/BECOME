using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
	[SerializeField] private int damage = 10;

	private void OnCollisionEnter2D( Collision2D collision )
	{
		PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();

		if (player != null)
		{
			player.TakeDamage(damage);
		}
	}
}