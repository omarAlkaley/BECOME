using UnityEngine;

public class Rope : MonoBehaviour, ICuttable
{
	public GameObject cutVFX;
	private bool isCut;
	public void Cut()
	{
		if (!isCut) return;
		transform.parent = null;
		Instantiate(cutVFX , transform.position , Quaternion.identity);
		isCut = true;
	}
}