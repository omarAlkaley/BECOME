using UnityEngine;

[CreateAssetMenu(fileName = "SwordAbility" , menuName = "Abilities/Sword")]
public class SwordAbility : Ability
{
	public float damage = 10f;
	public float range = 1.5f;
	public LayerMask hitMask;

	protected override void Activate( GameObject user )
	{
		Vector2 origin = user.transform.position;
		Vector2 direction = user.transform.right;

		RaycastHit2D[] hits = Physics2D.RaycastAll(
			origin ,
			direction ,
			range ,
			hitMask);

		foreach (var hit in hits)
		{
			IDamageable damageable = hit.collider.GetComponent<IDamageable>();
			if (damageable != null)
			{
				damageable.TakeDamage(damage);
			}

			ICuttable cuttable = hit.collider.GetComponent<ICuttable>();
			if (cuttable != null)
			{
				cuttable.Cut();
			}
		}
	}
}