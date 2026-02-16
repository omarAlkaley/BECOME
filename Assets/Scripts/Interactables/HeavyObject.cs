using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HeavyObject : MonoBehaviour
{
	private Rigidbody2D rb;

	[Header("Push Settings")]
	public float pushForce = 8f;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.mass = 100f; // heavy by default
	}

	private void OnCollisionStay2D( Collision2D collision )
	{
		PlayerStrength strength = collision.gameObject.GetComponent<PlayerStrength>();

		if (strength != null && strength.IsStrengthActive)
		{
			Vector2 direction = ( transform.position - collision.transform.position ).normalized;
			rb.AddForce(direction * pushForce);
		}
	}
}
