using UnityEngine;

public class BatEnemyAI : EnemyAI, IDamageable
{
	[Header("Bat Settings")]
	public float patrolSpeed = 2f;
	public float diveSpeed = 5f;
	public float health = 5f;

	public float patrolDistance = 3f;
	private Vector2 startPos;
	private bool movingRight = true;
	private bool diving = false;

	protected override void Awake()
	{
		base.Awake();
		startPos = transform.position;
	}

	protected override void Patrol()
	{
		if (diving) return;

		float dir = movingRight ? 1f : -1f;
		transform.Translate(Vector2.right * dir * patrolSpeed * Time.deltaTime);

		if (Mathf.Abs(transform.position.x - startPos.x) >= patrolDistance)
		{
			movingRight = !movingRight;
		}
	}

	protected override void Chase()
	{
		diving = true;
		Vector2 dir = ( player.position - transform.position ).normalized;
		rb.linearVelocity = dir * diveSpeed;
	}

	protected override void Attack()
	{
		diving = false;
		rb.linearVelocity = Vector2.zero;
		Debug.Log("Bat attacks player with dive!");
	}

	public void TakeDamage( float amount )
	{
		health -= amount;
		if (health <= 0)
			Destroy(gameObject);
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		// Stop dive on ground or walls
		if (collision.gameObject.CompareTag("Ground"))
		{
			diving = false;
			rb.linearVelocity = Vector2.zero;
		}
	}
}