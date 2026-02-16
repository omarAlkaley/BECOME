using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;
public class VidPlayer : MonoBehaviour
{
	[SerializeField] private string videoFileName;
	[SerializeField] private MonoBehaviour playerController; // drag your PlayerController here
	[SerializeField] public UnityEvent OnVideoFinishedEvent;
	private VideoPlayer videoPlayer;

	private void Awake()
	{
		videoPlayer = GetComponent<VideoPlayer>();
	}

	private void Start()
	{
		videoPlayer.loopPointReached += OnVideoFinished;
	}

	public void PlayVideo()
	{
		if (videoPlayer != null)
		{
			// Disable player control
			if (playerController != null)
				playerController.enabled = false;

			string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath , videoFileName);
			videoPlayer.url = videoPath;
			videoPlayer.Play();
		}
	}

	private void OnVideoFinished( VideoPlayer vp )
	{
		// Re-enable player control
		if (playerController != null)
			playerController.enabled = true;
		OnVideoFinishedEvent.Invoke();
	}

}
