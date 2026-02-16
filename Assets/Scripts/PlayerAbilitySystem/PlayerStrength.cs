using UnityEngine;

public class PlayerStrength : MonoBehaviour
{
	public bool IsStrengthActive { get; private set; }

	private float energyDrain;
	private PlayerEnergy playerEnergy;

	public void StartStrength( float drainPerSecond , PlayerEnergy energy )
	{
		energyDrain = drainPerSecond;
		playerEnergy = energy;
		IsStrengthActive = true;
	}

	public void StopStrength()
	{
		IsStrengthActive = false;
	}

	private void Update()
	{
		if (!IsStrengthActive || playerEnergy == null)
			return;

		float drainAmount = energyDrain * Time.deltaTime;

		// If not enough energy -> stop
		if (!playerEnergy.UseEnergy(Mathf.CeilToInt(drainAmount)))
		{
			StopStrength();
		}
	}
}
