using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
	[Header("Heart System")]
	[SerializeField] private int maxHearts = 5;
	[SerializeField] private int healthPerHeart = 2; // e.g., half-heart = 1
	public UnityEvent onDeath;

	private int currentHealth; // total health points

	private void Start()
	{
		currentHealth = maxHearts * healthPerHeart;
	}

	public void TakeDamage( int damage )
	{
		currentHealth -= damage;
		currentHealth = Mathf.Max(currentHealth , 0);
		UpdateHeartsUI();

		if (currentHealth <= 0)
			Die();
	}

	public void Heal( int amount )
	{
		currentHealth += amount;
		currentHealth = Mathf.Min(currentHealth , maxHearts * healthPerHeart);
		UpdateHeartsUI();
	}

	private void Die()
	{
		Debug.Log("Player Died!");
		onDeath?.Invoke();
	}

	public int GetCurrentHearts()
	{
		return Mathf.CeilToInt((float) currentHealth / healthPerHeart);
	}

	public int GetCurrentHealthPoints()
	{
		return currentHealth;
	}

	private void UpdateHeartsUI()
	{
		HeartUI.Instance.UpdateHearts(currentHealth, maxHearts, healthPerHeart);
	}
}
