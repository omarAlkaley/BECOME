using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class VideoFadeController : MonoBehaviour
{
	[SerializeField] private VideoPlayer videoPlayer;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private float fadeDuration = 1.5f;

	private void Start()
	{
		videoPlayer.loopPointReached += OnVideoFinished;
	}

	private void OnVideoFinished( VideoPlayer vp )
	{
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		float time = 0f;
		float startAlpha = canvasGroup.alpha;

		while (time < fadeDuration)
		{
			time += Time.deltaTime;
			canvasGroup.alpha = Mathf.Lerp(startAlpha , 0f , time / fadeDuration);
			yield return null;
		}

		canvasGroup.alpha = 0f;
	}
}
