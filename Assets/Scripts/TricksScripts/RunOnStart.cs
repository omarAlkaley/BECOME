using UnityEngine;
using UnityEngine.Events;

public class RunOnStart : MonoBehaviour
{
	[SerializeField] public UnityEvent OnStartAction;
	void Start()
	{
		OnStartAction.Invoke();
	}
}
