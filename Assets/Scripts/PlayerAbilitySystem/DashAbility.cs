using UnityEngine;

[CreateAssetMenu(fileName = "DashAbility" , menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
	public float dashDistance = 5f;
	public float dashDuration = 0.2f;
	protected override void Activate( GameObject user )
	{
		Rigidbody2D rb = user.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			Vector2 direction = new Vector2(user.transform.localScale.x , 0); // Facing direction
			user.GetComponent<MonoBehaviour>().StartCoroutine(Dash(rb , direction));
		}
	}

	private System.Collections.IEnumerator Dash( Rigidbody2D rb , Vector2 direction )
	{
		float timer = 0f;
		Vector2 startPos = rb.position;
		Vector2 targetPos = startPos + direction * dashDistance;

		while (timer < dashDuration)
		{
			rb.MovePosition(Vector2.Lerp(startPos , targetPos , timer / dashDuration));
			timer += Time.deltaTime;
			yield return null;
		}

		rb.MovePosition(targetPos);
	}
}