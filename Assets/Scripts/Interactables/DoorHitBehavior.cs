using UnityEngine;

public class DoorHitBehavior : MonoBehaviour, IImpulseTarget
{
	[Header("VFX & SFX")]
	public GameObject hitVFXPrefab;
	public AudioClip hitSFX;

	private bool hasBeenHit = false;

	public void OnImpulseHit( Vector2 force , GameObject source )
	{
		if (!hasBeenHit)
		{
			hasBeenHit = true;
#if UNITY_EDITOR 
			Debug.Log($"{gameObject.name} was pushed by impulse!");
#endif
		}
		else
		{
			// Spawn VFX
			if (hitVFXPrefab != null)
				Instantiate(hitVFXPrefab , transform.position , Quaternion.identity);

			// Play sound
			if (hitSFX != null)
				AudioSource.PlayClipAtPoint(hitSFX , transform.position);

			// Destroy object
			Destroy(gameObject);
		}
	}
}
