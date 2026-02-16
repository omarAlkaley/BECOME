using UnityEngine;

public class Grass : MonoBehaviour, ICuttable
{
	public GameObject cutVFX;

	public void Cut()
	{
		Instantiate(cutVFX , transform.position , Quaternion.identity);
		Destroy(gameObject);
	}
}