using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public enum State
	{
		Patrol,
		Chase,
		Attack,
		Dead
	}

	public State currentState;

	[Header("Detection")]
	public float visionRange = 5f;
	public float attackRange = 1.5f;
	public LayerMask playerMask;

	protected Transform player;
	protected Rigidbody2D rb;

	protected virtual void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	protected virtual void Update()
	{
		if (currentState == State.Dead)
			return;

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
				if (distance <= attackRange)
					currentState = State.Attack;
				else if (distance > visionRange)
					currentState = State.Patrol;
				break;

			case State.Attack:
				Attack();
				if (distance > attackRange)
					currentState = State.Chase;
				break;
		}
	}

	protected virtual void Patrol() { }
	protected virtual void Chase() { }
	protected virtual void Attack() { }
}
