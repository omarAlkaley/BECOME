using UnityEngine;
using UnityEngine.Events;

public class OneTimeTrigger2D : MonoBehaviour
{
	[SerializeField] private UnityEvent onTriggered;
	[SerializeField] private bool destroyAfterTrigger = true;

	private bool hasTriggered = false;

	private void OnTriggerEnter2D( Collider2D other )
	{
		if (hasTriggered)
			return;

		if (!other.CompareTag("Player"))
			return;

		hasTriggered = true;
		onTriggered?.Invoke();

		if (destroyAfterTrigger)
			Destroy(gameObject);
	}
}