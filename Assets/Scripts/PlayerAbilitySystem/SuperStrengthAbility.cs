using UnityEngine;

[CreateAssetMenu(fileName = "SuperStrengthAbility" , menuName = "Abilities/SuperStrength")]
public class SuperStrengthAbility : Ability
{
	public float energyDrainPerSecond = 15f;

	public override void OnPressed( GameObject user )
	{
		PlayerStrength strength = user.GetComponent<PlayerStrength>();
		PlayerEnergy energy = user.GetComponent<PlayerEnergy>();

		if (strength != null && energy != null)
		{
			strength.StartStrength(energyDrainPerSecond , energy);
		}
	}

	public override void OnReleased( GameObject user )
	{
		PlayerStrength strength = user.GetComponent<PlayerStrength>();
		if (strength != null)
		{
			strength.StopStrength();
		}
	}

	protected override void Activate( GameObject user )
	{
		// Not used for hold ability
	}
}
