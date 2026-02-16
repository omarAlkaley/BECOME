using UnityEngine;

public class MusicZone : MonoBehaviour
{
	[SerializeField] private AudioSource zoneTrack;

	private void OnTriggerEnter2D( Collider2D other )
	{
		if (!other.CompareTag("Player"))
			return;

		MusicManager.Instance.PlayZoneMusic(zoneTrack);
	}
}