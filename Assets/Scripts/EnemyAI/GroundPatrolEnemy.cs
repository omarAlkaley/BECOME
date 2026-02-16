using UnityEngine;

public class GroundPatrolEnemy : EnemyAI
{
	public float moveSpeed = 2f;
	public Transform groundCheck;
	public LayerMask groundMask;
	private bool movingRight = true;

	protected override void Patrol()
	{
		rb.linearVelocity = new Vector2(
			movingRight ? moveSpeed : -moveSpeed ,
			rb.linearVelocity.y);

		if (!Physics2D.Raycast(groundCheck.position , Vector2.down , 1f , groundMask))
		{
			Flip();
		}
	}

	protected override void Chase()
	{
		float dir = Mathf.Sign(player.position.x - transform.position.x);
		rb.linearVelocity = new Vector2(dir * moveSpeed * 1.5f , rb.linearVelocity.y);
	}

	protected override void Attack()
	{
		Debug.Log("Basic melee attack");
	}

	void Flip()
	{
		movingRight = !movingRight;
		transform.localScale = new Vector3(
			-transform.localScale.x ,
			transform.localScale.y ,
			transform.localScale.z);
	}
}