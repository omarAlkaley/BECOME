using UnityEngine;

public class CrateHitBehavior : MonoBehaviour, IImpulseTarget
{
	[Header("VFX & SFX")]
	public GameObject breakVFX;
	public AudioClip breakSFX;
	private bool isBroken = false;

	public void OnImpulseHit( Vector2 force , GameObject source )
	{
		if (!isBroken)
		{
			isBroken = true;
#if UNITY_EDITOR
			Debug.Log($"{gameObject.name} was pushed by impulse!");
#endif
			// Example: break into pieces
			if (breakVFX != null)
				Instantiate(breakVFX , transform.position , Quaternion.identity);

			if (breakSFX != null)
				AudioSource.PlayClipAtPoint(breakSFX , transform.position);

			Destroy(gameObject);
		}
	}
}