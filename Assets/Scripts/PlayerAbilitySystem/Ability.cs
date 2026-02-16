using UnityEngine;

public abstract class Ability : ScriptableObject
{
	public float energyCost = 10;
	public float cooldown = 0.2f;

	protected float lastUseTime;

	public virtual void OnPressed( GameObject user )
	{
		TryUse(user);
	}

	public virtual void OnReleased( GameObject user )
	{
		// Only override for hold abilities
	}

	protected void TryUse( GameObject user )
	{
		PlayerEnergy energy = user.GetComponent<PlayerEnergy>();

		if (energy == null)
			return;

		if (Time.time < lastUseTime + cooldown)
			return;

		if (!energy.UseEnergy(energyCost))
			return;

		lastUseTime = Time.time;
		Activate(user);
	}

	protected abstract void Activate( GameObject user );
}
