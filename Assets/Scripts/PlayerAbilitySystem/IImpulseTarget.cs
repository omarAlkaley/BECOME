using UnityEngine;

public interface IImpulseTarget
{
	// Called when the object is hit by an impulse
	void OnImpulseHit( Vector2 force , GameObject source );
}