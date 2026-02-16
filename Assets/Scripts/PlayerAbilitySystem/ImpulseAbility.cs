using UnityEngine;

[CreateAssetMenu(fileName = "ImpulseAbility" , menuName = "Abilities/Impulse")]
public class DoorAbility : Ability
{
	[Header("Impulse Settings")]
	public float forceAmount = 10f;          // How strong the impulse is
	public float checkDistance = 2f;
	public Vector2 forceDirection = Vector2.right; // Direction of the impulse
	public LayerMask layerMask;
	protected override void Activate( GameObject user )
	{
		// Raycast in front of the player to find a door
		Vector2 origin = user.transform.position;
		Vector2 direction = user.transform.right;

		RaycastHit2D hit = Physics2D.Raycast(origin , direction , checkDistance , layerMask);

		if (hit.collider != null)
		{
			Rigidbody2D rb = hit.collider.attachedRigidbody;

			if (rb != null)
			{
				// Apply impulse to the door
				rb.AddForce(direction.normalized * forceAmount , ForceMode2D.Impulse);
#if UNITY_EDITOR
				Debug.Log($"ImpulseAbility used on {hit.collider.name}");
#endif
			}
			else
			{
#if UNITY_EDITOR
				Debug.Log("Hit object has no Rigidbody2D");
#endif
			}
			IImpulseTarget targetBehavior = hit.collider.GetComponent<IImpulseTarget>();
			if (targetBehavior != null)
			{
				targetBehavior.OnImpulseHit(forceDirection.normalized * forceAmount , user);
			}
			else
			{
#if UNITY_EDITOR
				Debug.Log("No impulse behavior on object: " + hit.collider.name);
#endif
			}
		}
		else
		{
#if UNITY_EDITOR

			Debug.Log("No door found in front of player!");
#endif
		}
	}
}
