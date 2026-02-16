using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
	public static MusicManager Instance;

	[SerializeField] private float fadeDuration = 2f;

	private AudioSource currentSource;
	private Coroutine fadeRoutine;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	public void PlayZoneMusic( AudioSource newSource )
	{
		if (currentSource == newSource)
			return;

		if (fadeRoutine != null)
			StopCoroutine(fadeRoutine);

		fadeRoutine = StartCoroutine(Crossfade(newSource));
	}

	private IEnumerator Crossfade( AudioSource newSource )
	{
		AudioSource oldSource = currentSource;
		currentSource = newSource;

		if (newSource != null)
		{
			newSource.volume = 0f;
			newSource.Play();
		}

		float time = 0f;

		while (time < fadeDuration)
		{
			time += Time.deltaTime;
			float t = time / fadeDuration;

			if (oldSource != null)
				oldSource.volume = Mathf.Lerp(1f , 0f , t);

			if (newSource != null)
				newSource.volume = Mathf.Lerp(0f , 1f , t);

			yield return null;
		}

		if (oldSource != null)
		{
			oldSource.Stop();
			oldSource.volume = 1f;
		}

		if (newSource != null)
			newSource.volume = 1f;
	}
}
