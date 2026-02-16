using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
	[SerializeField] private float maxHealth = 30;
	public UnityEvent onDeath;

	private float currentHealth;

	private void Start()
	{
		currentHealth = maxHealth;
	}

	public void TakeDamage( float amount )
	{
		currentHealth -= amount;

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		onDeath?.Invoke();
		Destroy(gameObject);
	}
}
