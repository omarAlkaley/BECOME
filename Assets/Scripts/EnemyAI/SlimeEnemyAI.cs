using UnityEngine;

public class SlimeEnemyAI : EnemyAI, IDamageable
{
	[Header("Slime Settings")]
	public float moveSpeed = 2f;
	public GameObject smallSlimePrefab;
	public int splitCount = 2;
	public float health = 10f;

	private bool movingRight = true;
	public Transform groundCheck;
	public LayerMask groundMask;

	protected override void Patrol()
	{
		rb.linearVelocity = new Vector2(
			movingRight ? moveSpeed : -moveSpeed ,
			rb.linearVelocity.y);

		// Flip if no ground ahead
		if (!Physics2D.Raycast(groundCheck.position , Vector2.down , 1f , groundMask))
			Flip();
	}

	protected override void Chase()
	{
		float dir = Mathf.Sign(player.position.x - transform.position.x);
		rb.linearVelocity = new Vector2(dir * moveSpeed * 1.5f , rb.linearVelocity.y);
	}

	protected override void Attack()
	{
		rb.linearVelocity = Vector2.zero;
		if (Vector2.Distance(transform.position , player.position) <= attackRange)
		{
			Debug.Log("Slime attacks player!");
			// Here you would call player.TakeDamage(...)
		}
	}

	void Flip()
	{
		movingRight = !movingRight;
		transform.localScale = new Vector3(
			-transform.localScale.x ,
			transform.localScale.y ,
			transform.localScale.z);
	}

	public void TakeDamage( float amount )
	{
		health -= amount;
		if (health <= 0)
		{
			Die();
		}
	}

	void Die()
	{
		// Split into smaller slimes
		if (smallSlimePrefab != null)
		{
			for (int i = 0; i < splitCount; i++)
			{
				Vector3 spawnPos = transform.position + new Vector3(( i * 0.5f ) - 0.25f , 0 , 0);
				Instantiate(smallSlimePrefab , spawnPos , Quaternion.identity);
			}
		}

		Destroy(gameObject);
	}
}
