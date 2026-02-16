using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : MonoBehaviour
{
	[SerializeField] private PlayerEnergy playerEnergy;
	[SerializeField] private Image energyFill; // Image type: Filled
	[SerializeField] private float lerpSpeed = 5f; // speed of the smooth fill

	private void Update()
	{
		if (playerEnergy == null || energyFill == null)
			return;

		float targetFill = playerEnergy.CurrentEnergy / (float) playerEnergy.MaxEnergy;

		// Smoothly interpolate towards the target fill
		energyFill.fillAmount = Mathf.Lerp(energyFill.fillAmount , targetFill , Time.deltaTime * lerpSpeed);
	}
}