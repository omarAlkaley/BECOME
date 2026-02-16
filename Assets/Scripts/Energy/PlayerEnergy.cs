using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
	[SerializeField] private int maxEnergy = 100;
	[SerializeField] private float rechargeRate = 10f;

	private float currentEnergy;

	public float CurrentEnergy => currentEnergy;
	public int MaxEnergy => maxEnergy;

	private void Start()
	{
		currentEnergy = maxEnergy;
	}

	private void Update()
	{
		Recharge();
	}

	private void Recharge()
	{
		if (currentEnergy < maxEnergy)
		{
			currentEnergy += rechargeRate * Time.deltaTime;
			currentEnergy = Mathf.Clamp(currentEnergy , 0 , maxEnergy);
		}
	}

	public bool UseEnergy( float amount )
	{
		if (currentEnergy < amount)
			return false;

		currentEnergy -= amount;
		return true;
	}
}