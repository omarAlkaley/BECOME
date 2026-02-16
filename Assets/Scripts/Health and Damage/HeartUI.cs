using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
	public static HeartUI Instance;

	[SerializeField] private Image[] hearts;
	[SerializeField] private Sprite fullHeart;
	[SerializeField] private Sprite halfHeart;
	[SerializeField] private Sprite emptyHeart;

	private void Awake()
	{
		Instance = this;
	}

	public void UpdateHearts( int currentHealth , int maxHearts , int healthPerHeart )
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			if (i < maxHearts)
			{
				hearts[i].enabled = true;

				int heartHealth = Mathf.Min(healthPerHeart , currentHealth - i * healthPerHeart);

				if (heartHealth >= healthPerHeart)
					hearts[i].sprite = fullHeart;
				else if (heartHealth > 0)
					hearts[i].sprite = halfHeart;
				else
					hearts[i].sprite = emptyHeart;
			}
			else
			{
				hearts[i].enabled = false;
			}
		}
	}
}
