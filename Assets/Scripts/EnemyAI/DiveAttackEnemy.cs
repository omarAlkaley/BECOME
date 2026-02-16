using UnityEngine;

public class DiveAttackEnemy : EnemyAI, IDamageable
{
	[Header("Movement")]
	public float patrolSpeed = 2f;
	public float diveSpeed = 6f;
	public Transform patrolPath; // Parent of waypoints
	private Transform[] waypoints;
	private int currentWaypoint = 0;

	[Header("Combat")]
	public float health = 8f;
	public float attackCooldown = 2f;

	private bool diving = false;
	private float lastAttackTime = 0f;

	protected override void Awake()
	{
		base.Awake();

		// Collect waypoints
		if (patrolPath != null)
		{
			int count = patrolPath.childCount;
			waypoints = new Transform[count];
			for (int i = 0; i < count; i++)
				waypoints[i] = patrolPath.GetChild(i);
		}
		else
		{
			Debug.LogWarning("PatrolPath not assigned for DiveAttackEnemy.");
		}
	}

	protected override void Patrol()
	{
		if (diving || waypoints.Length == 0) return;

		Transform target = waypoints[currentWaypoint];
		Vector2 dir = ( target.position - transform.position ).normalized;
		rb.linearVelocity = dir * patrolSpeed;

		if (Vector2.Distance(transform.position , target.position) < 0.1f)
		{
			currentWaypoint = ( currentWaypoint + 1 ) % waypoints.Length;
		}
	}

	protected override void Chase()
	{
		if (Time.time < lastAttackTime + attackCooldown || diving) return;

		diving = true;
		lastAttackTime = Time.time;
	}

	protected override void Attack()
	{
		if (!diving) return;

		Vector2 dir = ( player.position - transform.position ).normalized;
		rb.linearVelocity = dir * diveSpeed;
	}

	private void Update()
	{
		if (currentState == State.Dead) return;

		float distance = Vector2.Distance(transform.position , player.position);

		switch (currentState)
		{
			case State.Patrol:
				Patrol();
				if (distance <= visionRange)
					currentState = State.Chase;
				break;

			case State.Chase:
				Chase();
				if (distance > visionRange)
					currentState = State.Patrol;
				else
					Attack();
				break;

			case State.Attack:
				// DiveAttackEnemy uses Attack inside Chase
				break;
		}
	}

	private void OnCollisionEnter2D( Collision2D collision )
	{
		// Stop dive on hitting ground or walls
		if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Obstacle"))
		{
			diving = false;
			rb.linearVelocity = Vector2.zero;
			// Return to patrol
			currentState = State.Patrol;
		}

		// Damage player on contact
		if (collision.gameObject.CompareTag("Player"))
		{
			Debug.Log("Player hit by dive!");
		}
	}

	public void TakeDamage( float amount )
	{
		health -= amount;
		if (health <= 0) Die();
	}

	void Die()
	{
		Destroy(gameObject);
	}
}
